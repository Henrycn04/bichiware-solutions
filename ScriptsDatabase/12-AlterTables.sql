-- Logical deletion
ALTER TABLE Profile
ADD Deleted bit DEFAULT 0
GO

ALTER TABLE Company
ADD Deleted bit DEFAULT 0
GO

ALTER TABLE PerishableProduct
ADD Deleted bit DEFAULT 0
GO

ALTER TABLE NonPerishableProduct
ADD Deleted bit DEFAULT 0
GO

ALTER TABLE Address
ADD Deleted bit DEFAULT 0
GO

--Dates for orders (used in reports)
ALTER TABLE Orders 
ADD CreationDate DATE DEFAULT GETDATE()
GO

ALTER TABLE Orders 
ADD CancellationDate DATE 
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
DROP CONSTRAINT DF__Orders__OrderSta__71D1E811;

ALTER TABLE Orders
DROP CONSTRAINT CK__Orders__OrderSta__72C60C4A;

ALTER TABLE Orders
DROP COLUMN OrderStatus;

ALTER TABLE Orders
ADD OrderStatus INT DEFAULT 1 CHECK (OrderStatus IN (1, 2, 3, 4, 5));
