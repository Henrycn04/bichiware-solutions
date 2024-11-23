ALTER TABLE Delivery
ADD Deleted bit DEFAULT 0
GO

UPDATE PerishableProduct
SET Deleted = 0

UPDATE NonPerishableProduct
SET Deleted = 0

Update Profile
set Deleted = 0

Update Address
set Deleted = 0

UPDATE PerishableProduct
SET ImageURL = 'https:/image'
where ProductID = 4

UPDATE NonPerishableProduct
SET ImageURL = 'https:/image'
where ProductID = 3

UPDATE Delivery
SET Deleted = 0





