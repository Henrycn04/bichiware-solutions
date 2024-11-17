CREATE PROCEDURE logicNonPerishableProductDelete
    @ProductID INT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE NonPerishableProduct
        SET Deleted = 1
        WHERE ProductID = @ProductID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

CREATE PROCEDURE NonPerishableProductDelete
    @ProductID INT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        DELETE FROM NonPerishableProduct
        WHERE ProductID = @ProductID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO

CREATE PROCEDURE PerishableProductDelete
    @ProductID INT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        DELETE FROM Delivery
        WHERE ProductID = @ProductID;

        DELETE FROM PerishableProduct
        WHERE ProductID = @ProductID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO

CREATE PROCEDURE logicPerishableProductDelete
    @ProductID INT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE PerishableProduct
        SET Deleted = 1
        WHERE ProductID = @ProductID;

        DECLARE @BatchNumber INT;

        DECLARE batchCursor CURSOR FOR
        SELECT BatchNumber
        FROM OrderedPerishable
        WHERE ProductID = @ProductID;

        OPEN batchCursor;
        FETCH NEXT FROM batchCursor INTO @BatchNumber;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            UPDATE Delivery
            SET Deleted = 1
            WHERE ProductID = @ProductID AND BatchNumber = @BatchNumber;

            FETCH NEXT FROM batchCursor INTO @BatchNumber;
        END;

        CLOSE batchCursor;
        DEALLOCATE batchCursor;

        DELETE FROM Delivery
        WHERE ProductID = @ProductID
          AND BatchNumber NOT IN (
              SELECT BatchNumber
              FROM OrderedPerishable
              WHERE ProductID = @ProductID
          );

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO
CREATE PROCEDURE logicDeliveryDelete
    @ProductID INT,
	@BatchNumber INT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE Delivery
        SET Deleted = 1
        WHERE ProductID = @ProductID AND BatchNumber = @BatchNumber;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

CREATE PROCEDURE DeliveryDelete
    @ProductID INT,
	@BatchNumber INT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        DELETE FROM Delivery
        WHERE ProductID = @ProductID AND BatchNumber = @BatchNumber;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO
CREATE PROCEDURE Top10ProductsLastOrder
    @UserId INT
AS
BEGIN
    SET TRANSACTION ISOLATION LEVEL REPEATABLE READ;
    BEGIN TRANSACTION;

    BEGIN TRY
        CREATE TABLE #OrderTempTable (
            OrderId INT,
            CreationDate DATETIME,
            RowNum INT
        );

        INSERT INTO #OrderTempTable (OrderId, CreationDate, RowNum)
        SELECT OrderID, CreationDate,
               ROW_NUMBER() OVER (ORDER BY CreationDate DESC) AS RowNum
        FROM Orders
        WHERE UserID = @UserId;

   
        DECLARE @ProductCount INT = 0;
        DECLARE @CurrentOrderId INT;

        DECLARE @Products TABLE (
            ProductId INT PRIMARY KEY
        );

   
        DECLARE @RowNum INT = 1;

        WHILE @ProductCount < 10 AND @RowNum <= (SELECT COUNT(*) FROM #OrderTempTable)
        BEGIN

            SELECT @CurrentOrderId = OrderId
            FROM #OrderTempTable
            WHERE RowNum = @RowNum;

            INSERT INTO @Products (ProductId)
            SELECT TOP (10 - @ProductCount) op.ProductId
            FROM OrderedPerishable op
            WHERE op.OrderId = @CurrentOrderId
              AND op.ProductId NOT IN (SELECT ProductId FROM @Products)
            ORDER BY op.ProductId;

            IF @ProductCount < 10
            BEGIN
                INSERT INTO @Products (ProductId)
                SELECT TOP (10 - @ProductCount) onp.ProductId
                FROM OrderedNonPerishable onp
                WHERE onp.OrderId = @CurrentOrderId
                  AND onp.ProductId NOT IN (SELECT ProductId FROM @Products)
                ORDER BY onp.ProductId;
            END

            SET @ProductCount = (SELECT COUNT(*) FROM @Products);

            SET @RowNum = @RowNum + 1;
        END;

        SELECT TOP 10 ProductId FROM @Products;

        DROP TABLE #OrderTempTable;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;