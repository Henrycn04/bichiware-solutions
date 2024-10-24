use FeriaDelEmprendedor

GO
CREATE PROCEDURE UpdateCompanyData
	@ID int,
	@CompanyName NVARCHAR(20),
	@PhoneNumber INT,
	@LegalID INT,
	@Email VARCHAR(50)
	AS
	BEGIN
	UPDATE Company
	SET
	CompanyName = @CompanyName,
	LegalID = @LegalID,
	EmailAddress = @Email,
	PhoneNumber = @PhoneNumber
	WHERE CompanyID = @ID;
END;
GO

CREATE PROCEDURE UpdateDeliveryData
    @ID INT,
    @BatchNumber INT,
	@OldBatchNumber INT,
    @ExpirationDate DATE
AS
BEGIN
    UPDATE Delivery
    SET
        BatchNumber = @BatchNumber,
        ExpirationDate = @ExpirationDate
    WHERE ProductID = @ID AND  BatchNumber = @OldBatchNumber;
END;
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
        p.ProductID = @ID OR np.ProductID = @ID;
END;
GO
CREATE PROCEDURE UpdatePerishableProductData
    @ProductID INT,
    @ProductName NVARCHAR(50),
    @Price DECIMAL(38,2),
    @ImageURL NVARCHAR(500),
    @Weight DECIMAL(38,2),
	@ProductDescription nvarchar(300),
	@Stock int
AS
BEGIN
    UPDATE PerishableProduct
    SET
    ProductName =  @ProductName,
    Price = @Price,
    ImageURL = @ImageURL,
    Weight = @Weight,
	ProductDescription = @ProductDescription,
	DeliveryDays = @DeliveryDays,
	Stock = @Stock
    WHERE ProductID = @ProductID;
END;
GO
CREATE PROCEDURE UpdateNonPerishableProductData
    @ProductID INT,
    @ProductName NVARCHAR(50),
    @Price DECIMAL(38,2),
    @ImageURL NVARCHAR(500),
    @Weight DECIMAL(38,2),
	@ProductDescription nvarchar(300),
	@Stock int
AS
BEGIN
    UPDATE NonPerishableProduct
    SET
    ProductName =  @ProductName,
    Price = @Price,
    ImageURL = @ImageURL,
    Weight = @Weight,
	ProductDescription = @ProductDescription,
	Stock = @Stock
    WHERE ProductID = @ProductID;
END;
GO
