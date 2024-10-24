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