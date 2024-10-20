-- This script must be run right after the ones before on
-- a clean database, otherwise the IDs wont match up

BEGIN TRANSACTION

INSERT INTO NonPerishableProduct(ProductName, CompanyID, Category, Price, ProductDescription, Stock, CompanyName, Weight)
VALUES ('Mesa', 1, 'DecoracionCasa', 400, 'Mesa de madera', 5, 'Bichiware Solutions', 20.1);
GO

INSERT INTO PerishableProduct(ProductName, CompanyID, Category, Price, ProductDescription, DeliveryDays, ProductionLimit, CompanyName, Weight)
VALUES ('Pan Natillero', 1, 'Alimentos', 2, 'Pan natillero casero', 'Martes', 20, 'Bichiware Solutions', 0.01);
GO

INSERT INTO Delivery(ProductID, BatchNumber, ReservedUnits, ExpirationDate)
VALUES (4, 1, 50, '2024-10-31');
GO

INSERT INTO Fee(Name, KmMin, KmMax, KGLimit, CostNormalKG, CostExtraKG)
VALUES ('GAM', 0, 60, 1, 2100.0, 1300.0);
GO

INSERT INTO ShoppingCart(UserID, ProductCost, ShippingCost)
VALUES (1, 4, 2.4);
GO

INSERT INTO PerishableCart(ProductID, UserID, BatchNumber, Quantity, ProductPrice ProductName)
VALUES(4, 1, 1, 2, 2, 'Pan Natillero');
GO

INSERT INTO NonPerishableCart(ProductID, UserID, Quantity, ProductPrice, ProductName)
VALUES(3, 1, 1, 400.0, 'Mesa');
GO

INSERT INTO Orders(UserID, AddressID, FeeID, Tax, ShippingCost, ProductCost)
VALUES(1, 1, 1, 10.0, 1200.0, 4);
GO

INSERT INTO OrderedPerishable(ProductID, OrderID, BatchNumber, Quantity, ProductPrice, ProductName)
VALUES(4, 1, 1, 2, 2.0, 'Pan Natillero');
GO

INSERT INTO OrderedNonPerishable(ProductID, OrderID, Quantity, ProductPrice, ProductName)
VALUES(3, 1, 1, 400.0, 'Mesa');
GO

COMMIT;