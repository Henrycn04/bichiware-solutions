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