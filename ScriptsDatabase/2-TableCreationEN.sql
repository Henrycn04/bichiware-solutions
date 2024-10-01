-- Profile Table
CREATE TABLE Profile(
	UserID int NOT NULL PRIMARY KEY IDENTITY(1,1),
	ProfileName nvarchar(20) NOT NULL,
	Email nvarchar(50) NOT NULL,
	userPassword nvarchar(20) NOT NULL,
	BankAccount varchar(50),
	PhoneNumber int NOT NULL,
	accountStatus char(20) CHECK (accountStatus IN ('Inactive', 'Active', 'Blocked')) DEFAULT 'Inactive',
	CreationDateTime datetime2(2),
	ConfirmationCode nvarchar(128)
);
GO

-- User Table
CREATE TABLE UserData(
	UserName nvarchar(20) NOT NULL,
	UserType int NOT NULL DEFAULT 1, 
	UserID int NOT NULL PRIMARY KEY FOREIGN KEY REFERENCES Profile(UserID) ON DELETE CASCADE, 
	Email nvarchar(50) NOT NULL,
	IDNumber int NOT NULL
);
GO

-- Company table
CREATE TABLE Company(
	CompanyID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	CompanyName nvarchar(20) NOT NULL,
	LegalID int NOT NULL,
	PhoneNumber int NOT NULL,
	EmailAddress varchar(50) NOT NULL
);
GO

-- CompanyMembers table
CREATE TABLE CompanyMembers (
	UserID int,
	CompanyID int,
	CONSTRAINT CMPrimaries PRIMARY KEY (UserID, CompanyID),
	CONSTRAINT CMUser FOREIGN KEY (UserID)
	 REFERENCES UserData(UserID)
	 ON DELETE CASCADE,
	CONSTRAINT CMCompany FOREIGN KEY (CompanyID)
	 REFERENCES Company(CompanyID)
	 ON DELETE CASCADE
);
GO

-- CompanyProfiles table
CREATE TABLE CompanyProfiles (
	UserID int,
	CompanyID int,
	CONSTRAINT CPPrimaries PRIMARY KEY (UserID, CompanyID),
	CONSTRAINT CPUser FOREIGN KEY (UserID)
	 REFERENCES Profile(UserID)
	 ON DELETE CASCADE,
	CONSTRAINT CPCompany FOREIGN KEY (CompanyID)
	 REFERENCES Company(CompanyID)
	 ON DELETE CASCADE
);
GO

-- Address table
CREATE TABLE Address(
	AddressID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Province nvarchar(50) NOT NULL,
	Canton nvarchar(50) NOT NULL,
	District nvarchar(50) NOT NULL,
	ExactAddress nvarchar(300) NOT NULL
);
GO

-- Non-perishable product table
CREATE TABLE NonPerishableProduct(
	ProductID int PRIMARY KEY IDENTITY(1,2),
	ProductName nvarchar(50) NOT NULL,
	CompanyID int,
    CompanyName nvarchar(20) NOT NULL,
	ImageURL nvarchar(500),
	Category nvarchar(50) CHECK (Category IN ('Alimentos', 'Electronicos', 'DecoracionCasa', 'Automoviles', 'DecoracionExteriores', 'Ropa', 'Joyeria', 'Limpieza')),
	Price decimal(38,2) NOT NULL,
	ProductDescription nvarchar(300),
	Stock int NOT NULL,
	CONSTRAINT NPProduct_Company FOREIGN KEY (CompanyID)
		REFERENCES Company (CompanyID)
		ON DELETE CASCADE
);
GO

-- Perishable product table
CREATE TABLE PerishableProduct(
	ProductID int PRIMARY KEY IDENTITY(2,2),
	ProductName nvarchar(50) NOT NULL,
	CompanyID int,
    CompanyName nvarchar(20) NOT NULL,
	ImageURL nvarchar(500),
	Category nvarchar(50) CHECK (Category IN 
	('Alimentos', 'Electronicos', 'DecoracionCasa', 'Automoviles', 'DecoracionExteriores', 'Ropa', 'Joyeria', 'Limpieza')),
	Price decimal(38,2) NOT NULL,
	ProductDescription nvarchar(300),
	DeliveryDays nvarchar(100) NOT NULL,
	ProductionLimit int NOT NULL,
	CONSTRAINT PProduct_Company FOREIGN KEY (CompanyID)
	REFERENCES Company(CompanyID)
	ON DELETE CASCADE
);
GO

-- Delivery table
CREATE TABLE Delivery(
	ProductID int,
	BatchNumber int,
	ReservedUnits int NOT NULL,
	ExpirationDate date NOT NULL,
	CONSTRAINT DeliveryPrimaries PRIMARY KEY (ProductID, BatchNumber),
	CONSTRAINT DeliveryProduct FOREIGN KEY (ProductID)
	REFERENCES PerishableProduct(ProductID)
	ON DELETE CASCADE
);
GO

-- User address table
CREATE TABLE UserAddress (
	UserID int,
	AddressID int,
	CONSTRAINT UAPrimaries PRIMARY KEY (UserID, AddressID),
	CONSTRAINT UAUser FOREIGN KEY (UserID)
	 REFERENCES UserData(UserID)
	 ON DELETE CASCADE,
	CONSTRAINT UAAddress FOREIGN KEY (AddressID)
	 REFERENCES Address(AddressID)
	 ON DELETE CASCADE
);
GO

-- Company address table
CREATE TABLE CompanyAddress (
	CompanyID int,
	AddressID int,
	CONSTRAINT CAPrimaries PRIMARY KEY (AddressID, CompanyID),
	CONSTRAINT CACompany FOREIGN KEY (CompanyID)
	 REFERENCES Company(CompanyID)
	 ON DELETE CASCADE,
	CONSTRAINT CAAddress FOREIGN KEY (AddressID)
	 REFERENCES Address(AddressID)
	 ON DELETE CASCADE
);
GO


