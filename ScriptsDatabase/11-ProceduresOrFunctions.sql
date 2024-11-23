use FeriaDelEmprendedor
GO

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

CREATE PROCEDURE UpdateProfileData
	@UID int,
	@NewName NVARCHAR(60),
	@NewNumber INT,
	@NewEmail NVARCHAR(50)
	AS
	BEGIN
	UPDATE Profile
	SET
	ProfileName = @NewName,
	Email = @NewEmail,
	PhoneNumber = @NewNumber
	WHERE UserID = @UID AND Deleted != 1;
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
GO
CREATE PROCEDURE UpdateAddressData
    @AddressID INT,
    @Province NVARCHAR(50),
    @Canton NVARCHAR(50),
    @District NVARCHAR(50),
    @ExactAddress NVARCHAR(300),
    @Latitude DECIMAL(13, 10),
    @Longitude DECIMAL(13, 10)
AS
BEGIN
    UPDATE Address
    SET
        Province = @Province,
        Canton = @Canton,
        District = @District,
        ExactAddress = @ExactAddress,
        Latitude = @Latitude,
        Longitude = @Longitude
    WHERE AddressID = @AddressID;
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
	@DeliveryDays nvarchar(100),
	@ProductionLimit int
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
	ProductionLimit = @ProductionLimit
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

CREATE PROCEDURE FindOrderedPerishables
	@OID int
	AS
	BEGIN
	SELECT
        op.ProductName,
		pp.Category,
		c.CompanyName,
		op.ProductPrice,
		op.Quantity,
		pp.ImageURL,
		c.CompanyID
    FROM OrderedPerishable op
    INNER JOIN PerishableProduct pp ON op.ProductID = pp.ProductID
	INNER JOIN Company c ON pp.CompanyID = c.CompanyID
	WHERE op.OrderID = @OID;
END;
GO

CREATE PROCEDURE FindOrderedNonPerishables
    @OID int
	AS
	BEGIN
	SELECT
		onp.ProductName,
		np.Category,
		c.CompanyName,
		onp.ProductPrice,
		onp.Quantity,
		np.ImageURL,
		c.CompanyID
	FROM OrderedNonPerishable onp
	INNER JOIN NonPerishableProduct np ON onp.ProductID = np.ProductID
	INNER JOIN Company c ON np.CompanyID = c.CompanyID
	WHERE onp.OrderID = @OID;
END;

GO
CREATE PROCEDURE FindCompaniesDataFromOrderID
    @OrderID int
AS
BEGIN
    SELECT c.CompanyID, c.CompanyName, c.EmailAddress
    FROM Orders o
    INNER JOIN OrderedNonPerishable onp ON onp.OrderID = o.OrderID
    INNER JOIN NonPerishableProduct npp ON npp.ProductID = onp.ProductID
    INNER JOIN Company c ON c.CompanyID = npp.CompanyID
    WHERE o.OrderID = @OrderID

    UNION

    SELECT c.CompanyID, c.CompanyName, c.EmailAddress
    FROM Orders o
    INNER JOIN OrderedPerishable op ON op.OrderID = o.OrderID
    INNER JOIN PerishableProduct pp ON pp.ProductID = op.ProductID
    INNER JOIN Company c ON c.CompanyID = pp.CompanyID
    WHERE o.OrderID = @OrderID
END;

GO
CREATE PROCEDURE FindOrderedProductsRelatedToACompany
	@OrderID int,
	@CompanyID int
AS
BEGIN
	SELECT onp.ProductName, npp.Category, c.CompanyName, onp.ProductPrice, onp.Quantity, npp.ImageURL, c.CompanyID
    FROM Orders o
    INNER JOIN OrderedNonPerishable onp ON onp.OrderID = o.OrderID
    INNER JOIN NonPerishableProduct npp ON npp.ProductID = onp.ProductID
    INNER JOIN Company c ON c.CompanyID = npp.CompanyID
	WHERE o.OrderID = @OrderID AND npp.CompanyID = @CompanyID

	UNION

	SELECT onp.ProductName, npp.Category, c.CompanyName, onp.ProductPrice, onp.Quantity, npp.ImageURL, c.CompanyID
    FROM Orders o
    INNER JOIN OrderedPerishable onp ON onp.OrderID = o.OrderID
    INNER JOIN PerishableProduct npp ON npp.ProductID = onp.ProductID
    INNER JOIN Company c ON c.CompanyID = npp.CompanyID
	WHERE o.OrderID = @OrderID AND npp.CompanyID = @CompanyID
END;
GO