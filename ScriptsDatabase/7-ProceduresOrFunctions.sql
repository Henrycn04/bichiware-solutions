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