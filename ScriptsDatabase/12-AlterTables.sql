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

--This drops all constraints on OrderStatus
DECLARE @constraintName NVARCHAR(255)
DECLARE @tableName NVARCHAR(255) = 'Orders'

DECLARE default_cursor CURSOR FOR
SELECT dc.name
FROM sys.default_constraints dc
JOIN sys.columns c ON dc.parent_object_id = c.object_id
WHERE c.name = 'OrderStatus' AND c.object_id = OBJECT_ID(@tableName);

OPEN default_cursor
FETCH NEXT FROM default_cursor INTO @constraintName

WHILE @@FETCH_STATUS = 0
BEGIN
    -- Drop the default constraint
    EXEC('ALTER TABLE ' + @tableName + ' DROP CONSTRAINT ' + @constraintName);
    
    FETCH NEXT FROM default_cursor INTO @constraintName
END

CLOSE default_cursor
DEALLOCATE default_cursor

-- Drop Check Constraint on OrderStatus
DECLARE check_cursor CURSOR FOR
SELECT cc.name
FROM sys.check_constraints cc
JOIN sys.columns c ON cc.parent_object_id = c.object_id
WHERE c.name = 'OrderStatus' AND c.object_id = OBJECT_ID(@tableName);

OPEN check_cursor
FETCH NEXT FROM check_cursor INTO @constraintName

WHILE @@FETCH_STATUS = 0
BEGIN
    -- Drop the check constraint
    EXEC('ALTER TABLE ' + @tableName + ' DROP CONSTRAINT ' + @constraintName);
    
    FETCH NEXT FROM check_cursor INTO @constraintName
END

CLOSE check_cursor
DEALLOCATE check_cursor
GO

ALTER TABLE Orders
DROP COLUMN OrderStatus;
GO

ALTER TABLE Orders
ADD OrderStatus INT NOT NULL DEFAULT 1 CHECK (OrderStatus IN (1, 2, 3, 4, 5));
GO