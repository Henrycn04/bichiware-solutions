CREATE TRIGGER trigger_CompanyName_PerishableProduct
ON PerishableProduct
AFTER INSERT
AS
BEGIN
    UPDATE pp
    SET pp.CompanyName = c.CompanyName
    FROM PerishableProduct pp
    INNER JOIN INSERTED i ON pp.ProductID = i.ProductID
    INNER JOIN Company c ON i.CompanyID = c.CompanyID;
END;
GO

CREATE TRIGGER trigger_CompanyName_NonPerishableProduct
ON NonPerishableProduct
AFTER INSERT
AS
BEGIN
    UPDATE npp
    SET npp.CompanyName = c.CompanyName
    FROM NonPerishableProduct npp
    INNER JOIN INSERTED i ON npp.ProductID = i.ProductID
    INNER JOIN Company c ON i.CompanyID = c.CompanyID;
END;
GO

GO
CREATE TRIGGER Trigger_Rejected_Orders
ON Orders
AFTER UPDATE
AS
BEGIN

	IF UPDATE(OrderStatus)
	BEGIN

		IF EXISTS (
			SELECT 1 
			FROM inserted i
			JOIN deleted d ON i.OrderID = d.OrderID
			WHERE i.OrderStatus = 3 AND d.OrderStatus = 1
		)
		BEGIN

			DECLARE @OrderID int
			SELECT @OrderID = i.OrderID 
			FROM inserted i
			
			-- First, restore the quantity in the non perishable products

			DECLARE @ReservedUnit_ID int
			DECLARE @ReservedUnit_Quantity int

			DECLARE @Trigger_Rejected_Orders_NonPerishable_Cursor AS CURSOR

			SET @Trigger_Rejected_Orders_NonPerishable_Cursor = CURSOR FOR
				SELECT np.Quantity, np.ProductID
				FROM OrderedNonPerishable np
				INNER JOIN inserted i ON i.OrderID = np.OrderID

			OPEN @Trigger_Rejected_Orders_NonPerishable_Cursor
			FETCH NEXT FROM @Trigger_Rejected_Orders_NonPerishable_Cursor	INTO @ReservedUnit_Quantity, @ReservedUnit_ID

			WHILE @@FETCH_STATUS = 0
				BEGIN

					UPDATE NonPerishableProduct 
					SET Stock = Stock + @ReservedUnit_Quantity
					WHERE ProductID = @ReservedUnit_ID

					DELETE FROM OrderedNonPerishable
					WHERE OrderID = @OrderID

				FETCH NEXT FROM @Trigger_Rejected_Orders_NonPerishable_Cursor	INTO @ReservedUnit_Quantity, @ReservedUnit_ID
				END
				
			CLOSE @Trigger_Rejected_Orders_NonPerishable_Cursor
			DEALLOCATE @Trigger_Rejected_Orders_NonPerishable_Cursor

			-- Next the perishable products

			DECLARE @BatchNumber int

			DECLARE @Trigger_Rejected_Orders_Perishable_Cursor AS CURSOR

			SET @Trigger_Rejected_Orders_Perishable_Cursor = CURSOR FOR
				SELECT op.Quantity, op.ProductID, op.BatchNumber
				FROM OrderedPerishable op
				INNER JOIN inserted i ON i.OrderID = op.OrderID

			OPEN @Trigger_Rejected_Orders_Perishable_Cursor
			FETCH NEXT FROM @Trigger_Rejected_Orders_Perishable_Cursor	INTO @ReservedUnit_Quantity, @ReservedUnit_ID, @BatchNumber

			WHILE @@FETCH_STATUS = 0
				BEGIN

					UPDATE Delivery 
					SET ReservedUnits = ReservedUnits - @ReservedUnit_Quantity
					WHERE ProductID = @ReservedUnit_ID AND BatchNumber = @BatchNumber

					DELETE FROM OrderedPerishable
					WHERE OrderID = @OrderID

				FETCH NEXT FROM @Trigger_Rejected_Orders_Perishable_Cursor	INTO @ReservedUnit_Quantity, @ReservedUnit_ID, @BatchNumber
				END
				
			CLOSE @Trigger_Rejected_Orders_Perishable_Cursor
			DEALLOCATE @Trigger_Rejected_Orders_Perishable_Cursor

		END
	END
END;
GO

CREATE TRIGGER trg_UpdateStockOnInsert
ON OrderedPerishable
AFTER INSERT
AS
BEGIN
    UPDATE Delivery
    SET ReservedUnits = ReservedUnits + inserted.Quantity
    FROM Delivery
    INNER JOIN inserted ON Delivery.ProductID = inserted.ProductID
        AND Delivery.BatchNumber = inserted.BatchNumber;
END;
GO

CREATE TRIGGER trg_UpdateStockOnInsertNonPerishable
ON OrderedNonPerishable
AFTER INSERT
AS
BEGIN
    UPDATE NonPerishableProduct
    SET Stock = Stock - inserted.Quantity
    FROM NonPerishableProduct
    INNER JOIN inserted ON NonPerishableProduct.ProductID = inserted.ProductID;
END;
GO

GO
CREATE TRIGGER Trigger_Reject_Orders_By_Entrepreneur
ON Orders
AFTER UPDATE
AS
BEGIN

	IF UPDATE(OrderStatus)
	BEGIN

		IF EXISTS (
			SELECT 1 
			FROM inserted i
			JOIN deleted d ON i.OrderID = d.OrderID
			WHERE i.OrderStatus = 3 AND d.OrderStatus = 2
		)
		BEGIN

			DECLARE @OrderID int
			SELECT @OrderID = i.OrderID 
			FROM inserted i
			
			-- First, restore the quantity in the non perishable products

			DECLARE @ReservedUnit_ID int
			DECLARE @ReservedUnit_Quantity int

			DECLARE @Trigger_Rejected_Orders_NonPerishable_Cursor AS CURSOR

			SET @Trigger_Rejected_Orders_NonPerishable_Cursor = CURSOR FOR
				SELECT np.Quantity, np.ProductID
				FROM OrderedNonPerishable np
				INNER JOIN inserted i ON i.OrderID = np.OrderID

			OPEN @Trigger_Rejected_Orders_NonPerishable_Cursor
			FETCH NEXT FROM @Trigger_Rejected_Orders_NonPerishable_Cursor	INTO @ReservedUnit_Quantity, @ReservedUnit_ID

			WHILE @@FETCH_STATUS = 0
				BEGIN

					UPDATE NonPerishableProduct 
					SET Stock = Stock + @ReservedUnit_Quantity
					WHERE ProductID = @ReservedUnit_ID

				FETCH NEXT FROM @Trigger_Rejected_Orders_NonPerishable_Cursor	INTO @ReservedUnit_Quantity, @ReservedUnit_ID
				END
				
			CLOSE @Trigger_Rejected_Orders_NonPerishable_Cursor
			DEALLOCATE @Trigger_Rejected_Orders_NonPerishable_Cursor

			-- Next the perishable products

			DECLARE @BatchNumber int

			DECLARE @Trigger_Rejected_Orders_Perishable_Cursor AS CURSOR

			SET @Trigger_Rejected_Orders_Perishable_Cursor = CURSOR FOR
				SELECT op.Quantity, op.ProductID, op.BatchNumber
				FROM OrderedPerishable op
				INNER JOIN inserted i ON i.OrderID = op.OrderID

			OPEN @Trigger_Rejected_Orders_Perishable_Cursor
			FETCH NEXT FROM @Trigger_Rejected_Orders_Perishable_Cursor	INTO @ReservedUnit_Quantity, @ReservedUnit_ID, @BatchNumber

			WHILE @@FETCH_STATUS = 0
				BEGIN

					UPDATE Delivery 
					SET ReservedUnits = ReservedUnits - @ReservedUnit_Quantity
					WHERE ProductID = @ReservedUnit_ID AND BatchNumber = @BatchNumber

				FETCH NEXT FROM @Trigger_Rejected_Orders_Perishable_Cursor	INTO @ReservedUnit_Quantity, @ReservedUnit_ID, @BatchNumber
				END
				
			CLOSE @Trigger_Rejected_Orders_Perishable_Cursor
			DEALLOCATE @Trigger_Rejected_Orders_Perishable_Cursor

		END
	END
END;
GO