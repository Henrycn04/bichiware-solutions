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
	WHERE UserID = @UID;
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