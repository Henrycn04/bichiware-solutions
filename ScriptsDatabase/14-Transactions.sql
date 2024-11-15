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