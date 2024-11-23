-- Logical deletion
ALTER TABLE Profile
ADD Deleted bit DEFAULT 0
GO

UPDATE Profile
SET Deleted = 0;
GO

ALTER TABLE Company
ADD Deleted bit DEFAULT 0
GO

UPDATE Company
SET Deleted = 0;
GO

ALTER TABLE PerishableProduct
ADD Deleted bit DEFAULT 0
GO

UPDATE PerishableProduct
SET Deleted = 0;
GO

ALTER TABLE NonPerishableProduct
ADD Deleted bit DEFAULT 0
GO

UPDATE NonPerishableProduct
SET Deleted = 0;
GO

ALTER TABLE Address
ADD Deleted bit DEFAULT 0
GO

UPDATE Address
SET Deleted = 0;
GO


--Dates for orders (used in reports)
ALTER TABLE Orders 
ADD CreationDate DATE DEFAULT GETDATE()
GO

UPDATE Orders
SET CreationDate = GETDATE();
GO

ALTER TABLE Orders 
ADD CancellationDate DATE 
GO

UPDATE Orders
SET CreationDate = GETDATE();
GO

ALTER TABLE Orders
ADD SentDate DATE

ALTER TABLE Orders 
ADD DeliveredDate DATE
GO

ALTER TABLE Orders
ADD CancelledBy int
GO

ALTER TABLE Orders
DROP COLUMN OrderStatus
GO

ALTER TABLE Orders
ADD OrderStatus INT DEFAULT 1 CHECK (OrderStatus IN (1, 2, 3, 4, 5))
GO
