CREATE PROCEDURE logicNonPerishableProductDelete
    @ProductID INT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE NonPerishableProduct
        SET Deleted = 1
        WHERE ProductID = @ProductID;

		 DELETE FROM NonPerishableCart
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

		 DELETE FROM NonPerishableCart
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

		DELETE FROM PerishableCart
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

		DELETE FROM PerishableCart
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
        WHERE UserID = @UserID;

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
CREATE PROCEDURE userDataDelete
    @UserID INT
AS
BEGIN
    SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
    BEGIN TRANSACTION;

    BEGIN TRY
    
        DELETE FROM Profile
        WHERE UserID = @UserID;

        DELETE FROM UserData
        WHERE UserID = @UserID;

       
        DELETE FROM CompanyMembers
        WHERE UserID = @UserID;

  
        DELETE FROM CompanyProfiles
        WHERE UserID = @UserID;

  
        DELETE FROM UserAddress
        WHERE UserID = @UserID;

      
        DELETE FROM ShoppingCart
        WHERE UserID = @UserID;

  
        DELETE FROM PerishableCart
        WHERE UserID = @UserID;

 
        DELETE FROM NonPerishableCart
        WHERE UserID = @UserID;


        WITH AddressToDelete AS (
            SELECT UA.AddressID
            FROM UserAddress UA
            WHERE UA.UserID = @UserID
              AND NOT EXISTS (
                  SELECT 1
                  FROM UserAddress UA2
                  WHERE UA2.AddressID = UA.AddressID
                    AND UA2.UserID <> @UserID
              )
        )
        DELETE FROM Address
        WHERE AddressID IN (SELECT AddressID FROM AddressToDelete);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW; 
    END CATCH;
END;
GO
CREATE PROCEDURE logicUserDataDelete
    @UserID INT
AS
BEGIN
    SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
    BEGIN TRANSACTION;

    BEGIN TRY
 
        UPDATE Profile
        SET Deleted = 1
        WHERE UserID = @UserID;

   
        DELETE FROM CompanyMembers
        WHERE UserID = @UserID;

   
        DELETE FROM CompanyProfiles
        WHERE UserID = @UserID;

   
        DELETE FROM ShoppingCart
        WHERE UserID = @UserID;

      
        DELETE FROM PerishableCart
        WHERE UserID = @UserID;

  
        DELETE FROM NonPerishableCart
        WHERE UserID = @UserID;

       
        WITH AddressUsage AS (
            SELECT UA.AddressID
            FROM UserAddress UA
            WHERE UA.UserID = @UserID
              AND NOT EXISTS (
                  SELECT 1
                  FROM UserAddress UA2
                  WHERE UA2.AddressID = UA.AddressID
                    AND UA2.UserID <> @UserID
              )
        )
        UPDATE Address
        SET Deleted = 1
        WHERE AddressID IN (SELECT AddressID FROM AddressUsage);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

Go
create procedure CompanyHardDelete
	@CompanyId int
as begin
	begin try
		begin transaction

        create table #AddressTempTable (
            AdressId int
        )
        insert into #AddressTempTable(AdressId)
        select AddressId from CompanyAddress where CompanyID = @CompanyId

        delete from Address where AddressID in (select * from #AddressTempTable)
		delete from CompanyAddress where AddressID in (select * from #AddressTempTable)

		delete from CompanyMembers where CompanyID = @CompanyId
		delete from CompanyProfiles where CompanyID = @CompanyId

	    delete from Company where CompanyID = @CompanyId

		commit transaction
	end try
	begin catch
		rollback transaction
		throw
	end catch
end

Go
create procedure CompanySoftDelete
    @CompanyId int
as begin
    begin try
		begin transaction
        
        create table #AddressTempTable (
            AdressId int
        )
        insert into #AddressTempTable(AdressId)
        select AddressId from CompanyAddress where CompanyID = @CompanyId
        
        delete from Address where AddressID in (select * from #AddressTempTable)
		delete from CompanyAddress where AddressID in (select * from #AddressTempTable)

        delete from CompanyMembers where CompanyID = @CompanyId
	    delete from CompanyProfiles where CompanyID = @CompanyId

	    update Company set Deleted = 1 where CompanyID = @CompanyId

		commit transaction
	end try
	begin catch
		rollback transaction
		throw
	end catch
end

drop procedure CompanyHardDelete
drop procedure CompanySoftDelete

