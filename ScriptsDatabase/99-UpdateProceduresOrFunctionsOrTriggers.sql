DROP Proc GetCombinedProducts
GO
CREATE PROCEDURE GetCombinedProducts
    @ID INT
AS
BEGIN
    SELECT 
        COALESCE(p.ProductID, np.ProductID) AS ProductID,
        COALESCE(p.ProductName, np.ProductName) AS Name,
        COALESCE(p.Price, np.Price) AS Price,
        COALESCE(p.Category, np.Category) AS Category,
        COALESCE(p.CompanyName, np.CompanyName) AS CompanyName,
        COALESCE(p.ImageURL, np.ImageURL) AS ImageURL,
        COALESCE(p.ProductDescription, np.ProductDescription) AS Description,
        COALESCE(p.CompanyID, np.CompanyID) AS CompanyID,
        CASE 
            WHEN p.DeliveryDays IS NOT NULL THEN p.DeliveryDays 
            ELSE NULL
        END AS DeliveryDays,
        COALESCE(np.Weight, p.Weight) AS Weight,
        CASE 
            WHEN np.Stock IS NOT NULL THEN np.Stock 
            ELSE NULL
        END AS Stock,
        CASE 
            WHEN p.ProductionLimit IS NOT NULL THEN p.ProductionLimit 
            ELSE NULL
        END AS ProductionLimit
    FROM 
        PerishableProduct p
    FULL OUTER JOIN 
        NonPerishableProduct np
    ON 
        p.ProductID = np.ProductID
    WHERE 
        (p.ProductID = @ID OR np.ProductID = @ID) AND (p.Deleted = 0 OR np.Deleted = 0);
END;
GO
DROP Proc GetCompanyProducts
GO
create procedure GetCompanyProducts
	@companyId int
as begin
	select NP.ProductID from NonPerishableProduct as NP where NP.CompanyID = @companyId AND NP.Deleted = 0
	union
	select P.ProductID from PerishableProduct as P where P.CompanyID = @companyId AND P.Deleted = 0
end;
GO
