-- This script only works if its run before any other insertion

BEGIN TRANSACTION

-- Uncomment if you want to delete everything
--DELETE FROM CompanyAddress;
--DELETE FROM UserAddress;
--DELETE FROM Delivery;
--DELETE FROM PerishableProduct;
--DELETE FROM NonPerishableProduct;
--DELETE FROM Address;
--DELETE FROM CompanyProfiles;
--DELETE FROM CompanyMembers;
--DELETE FROM Company;
--DELETE FROM Profile;
--DELETE FROM Address;

 --Inserts test data into Perfil table
 --Password is Hola_1234
INSERT INTO Profile(ProfileName, Email, userPassword, BankAccount, PhoneNumber, accountStatus, ConfirmationCode, CreationDateTime) VALUES
	('Andre Salas','andre.salas.cr.201@gmail.com',
	'42a75569da9132cd2141ec4d636c761097c6e4ec437f756bee355ceecadae77f4a6eb226b239f769a72058a5e16fc834dc80a035698b59ce6f2c9fb11e1d9593',
	'CRC012341',86430421,'Active'
	,'318f3f005a97a26456bb9b77024eabbeba2ad71ca423636a16fd3430bc9e9f4b347d9b9856ebc238262b0bd688b1d6333653e0d6faf77abbc3622c523326c5c7'
	,'2024-11-09 12:30:49')
GO


-- Inserts test data into User, linking with the data in Profile
-- 1: Customer
-- 2: Enterpreneur
-- 3: Admin
INSERT INTO UserData(UserID, UserName, UserType, Email, IDNumber) VALUES
	(1, 'Andre Salas', 2, 'andre.salas.cr.201@gmail.com', 134324221)
GO

-- Inserts test data into Company
INSERT INTO Company(CompanyName, LegalID, PhoneNumber, EmailAddress, Deleted)
VALUES('Bichiware Solutions', 123456789, 22222222, 'aaa@ucr.ac.cr', 0);
GO

-- Inserts test data into CompanyMembers, linking with User and Company
INSERT INTO CompanyMembers
VALUES (1, 1);
GO

-- Inserts test data into CompanyProfiles, linking with Profile and Company
INSERT INTO CompanyProfiles
VALUES(1, 1);
GO

-- Inserts test data into Address
INSERT INTO Address(Province, Canton, District, ExactAddress, Latitude, Longitude) VALUES
	('San José', 'Central', 'Merced', 'Avenida 2, Calle 5, Casa 7', 9.934326876343688, -84.07648086547853),
	('San Jose', 'Montes de Oca', 'San Pedro', 'Universidad Calle 57 José María Muñoz 11501', 9.934665051190777, -84.05168116092683);
GO

-- Inserts test data into NonPerishableProduct, linking with data in Company
INSERT INTO NonPerishableProduct(ProductName, CompanyID, ImageURL, Category, Price, ProductDescription, Stock, CompanyName)
VALUES ('Computadora', 1, 'http://computadora.jpg', 'Electronicos', 800, 'Laptop HP with 16GB RAM and 512GB SSD', 10, 'Bichiware Solutions');
GO

-- Inserts test data into PerishableProduct, linking with data in Company
INSERT INTO PerishableProduct(ProductName, CompanyID, ImageURL, Category, Price, ProductDescription, DeliveryDays, ProductionLimit, CompanyName)
VALUES ('Pan Baguette', 1, 'http://example.com/images/pan_baguette.jpg', 'Alimentos', 2, 'Fresh and crispy baguette', 'Lunes', 100, 'Bichiware Solutions');
GO

-- Inserts test data into Delivery, linking with data in PerishableProduct
INSERT INTO Delivery(ProductID, BatchNumber, ReservedUnits, ExpirationDate)
VALUES (2, 1001, 50, '2025-10-01');
GO

-- Inserts test data into UserAddress, linking with data in User and Address
INSERT INTO UserAddress(UserID, AddressID) VALUES
	(1, 1);
GO

-- Inserts test data into CompanyAddress, linking with data in Company and Address
INSERT INTO CompanyAddress(CompanyID, AddressID)
VALUES (1, 1);
GO

-- Inserts the fees. First one GAM fee, second one has coverage in all the country
INSERT INTO Fee(KmMin, KmMax, CostNormalKG, CostExtraKG, KGLimit) VALUES
	(0, 60, 2000, 700, 0),
	(60, 99999, 3000, 900, 0)
GO


-- Commits all changes
COMMIT;

-- Uncomment to check the tables
--SELECT * FROM CompanyAddress;
--SELECT * FROM UserAddress;
--SELECT * FROM Delivery;
--SELECT * FROM PerishableProduct;
--SELECT * FROM NonPerishableProduct;
--SELECT * FROM Address;
--SELECT * FROM CompanyProfiles;
--SELECT * FROM CompanyMembers;
--SELECT * FROM Company;
--SELECT * FROM Profile;
--SELECT * FROM UserData;
