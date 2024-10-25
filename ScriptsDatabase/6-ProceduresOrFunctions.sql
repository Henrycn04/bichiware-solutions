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
		(op.ProductPrice * op.Quantity) AS Cost,
		op.Quantity,
		pp.CompanyID
    FROM OrderedPerishable op
    INNER JOIN PerishableProduct pp ON op.ProductID = pp.ProductID
	WHERE op.OrderID = @OID;
END;
GO

CREATE PROCEDURE FindOrderedNonPerishables
    @OID int
	AS
	BEGIN
	SELECT
		onp.ProductName,
		(onp.ProductPrice * onp.Quantity) AS Cost,
		onp.Quantity,
		np.CompanyID
	FROM OrderedNonPerishable onp
	INNER JOIN NonPerishableProduct np ON onp.ProductID = np.ProductID
	WHERE onp.OrderID = @OID;
END;
GO