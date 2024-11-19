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
        WHERE UserID = 1;

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
            INNER JOIN PerishableProduct pp ON op.ProductId = pp.ProductId
            WHERE op.OrderId = @CurrentOrderId
              AND pp.deleted = 0
              AND op.ProductId NOT IN (SELECT ProductId FROM @Products)
            ORDER BY op.ProductId;


            SET @ProductCount = (SELECT COUNT(*) FROM @Products);


            IF @ProductCount < 10
            BEGIN
                INSERT INTO @Products (ProductId)
                SELECT TOP (10 - @ProductCount) onp.ProductId
                FROM OrderedNonPerishable onp
                INNER JOIN NonPerishableProduct npp ON onp.ProductId = npp.ProductId
                WHERE onp.OrderId = @CurrentOrderId
                  AND npp.deleted = 0
                  AND onp.ProductId NOT IN (SELECT ProductId FROM @Products)
                ORDER BY onp.ProductId;
            END;

 
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
GO
CREATE PROCEDURE TotalProfits
    @Year INT,
    @CompanyIDs NVARCHAR(MAX)
AS
BEGIN
    SET TRANSACTION ISOLATION LEVEL READ COMMITTED;

    BEGIN TRY
 
        IF OBJECT_ID('tempdb..#OrderSummary') IS NOT NULL
            DROP TABLE #OrderSummary;

        BEGIN TRANSACTION;


        CREATE TABLE #OrderSummary (
            CompanyID INT,
            CompanyName NVARCHAR(255),
            ProductID INT,
            OrderID INT, 
            TotalPrice DECIMAL(38, 2),
            [Month] INT,
            [Year] INT,
            ShippingCost DECIMAL(38, 2),
            TotalOrderPrice DECIMAL(38, 2)
        );

   
        INSERT INTO #OrderSummary
        SELECT pp.CompanyID, 
               pp.CompanyName,
               op.ProductID,
               o.OrderID,
               (op.Quantity * op.ProductPrice) + (op.Quantity * op.ProductPrice * 0.13) AS TotalPrice,
               MONTH(o.DeliveredDate) AS [Month],
               @Year AS [Year],
               o.ShippingCost,
               ((op.Quantity * op.ProductPrice) + (op.Quantity * op.ProductPrice * 0.13)) + o.ShippingCost AS TotalOrderPrice
        FROM Orders o
        INNER JOIN OrderedPerishable op ON op.OrderID = o.OrderID
        INNER JOIN PerishableProduct pp ON op.ProductID = pp.ProductID
        WHERE YEAR(o.DeliveredDate) = @Year
          AND o.OrderStatus = 5
          AND pp.CompanyID IN (SELECT value FROM OPENJSON(@CompanyIDs));

        INSERT INTO #OrderSummary
        SELECT np.CompanyID, 
               np.CompanyName,
               onp.ProductID,
               o.OrderID, -- Insertamos el OrderID
               (onp.Quantity * onp.ProductPrice) + (onp.Quantity * onp.ProductPrice * 0.13) AS TotalPrice,
               MONTH(o.DeliveredDate) AS [Month],
               @Year AS [Year],
               o.ShippingCost,
               ((onp.Quantity * onp.ProductPrice) + (onp.Quantity * onp.ProductPrice * 0.13)) + o.ShippingCost AS TotalOrderPrice
        FROM Orders o
        INNER JOIN OrderedNonPerishable onp ON onp.OrderID = o.OrderID
        INNER JOIN NonPerishableProduct np ON onp.ProductID = np.ProductID
        WHERE YEAR(o.DeliveredDate) = @Year
          AND o.OrderStatus = 5
          AND np.CompanyID IN (SELECT value FROM OPENJSON(@CompanyIDs));

        SELECT 
            CompanyID,
            CompanyName,
            OrderID,
            SUM(TotalPrice) AS TotalPrice,
            [Month],
            [Year],
            MAX(ShippingCost) AS ShippingCost, 
            (SUM(TotalPrice) + MAX(ShippingCost)) AS TotalOrderPrice 
        INTO #OrderGrouped
        FROM #OrderSummary
        GROUP BY 
            CompanyID,
            CompanyName,
            OrderID,
            [Month],
            [Year];

        SELECT 
            CompanyID,
            CompanyName,
            [Month],
            [Year],
            SUM(TotalPrice) AS TotalPrice,
            SUM(ShippingCost) AS TotalShippingCost,
            SUM(TotalOrderPrice) AS TotalOrderPrice
        FROM #OrderGrouped
        GROUP BY 
            CompanyID,
            CompanyName,
            [Month],
            [Year];

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        IF OBJECT_ID('tempdb..#OrderSummary') IS NOT NULL
            DROP TABLE #OrderSummary;
        IF OBJECT_ID('tempdb..#OrderGrouped') IS NOT NULL
            DROP TABLE #OrderGrouped;
        THROW;
    END CATCH;
END;
GO


