CREATE TABLE Fee(
    FeeID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    Name nvarchar(50),
    KmMin int unique NOT NULL,
    KmMax int unique NOT NULL,
    KGLimit int NOT NULL,
    CostNormalKG dec(38,2) NOT NULL,
    CostExtraKG dec(38,2) NOT NULL,
);
GO

CREATE TABLE ShoppingCart(
    UserID int NOT NULL PRIMARY KEY,
    ProductCost dec(38,2) NOT NULL,
    ShippingCost dec(38,2) NOT NULL,
    CONSTRAINT FK_ShoppingCart_Profile FOREIGN KEY (UserID) 
        REFERENCES Profile(UserID) ON DELETE CASCADE
);
GO

CREATE TABLE PerishableCart(
    ProductID int NOT NULL,
    UserID int NOT NULL,
    BatchNumber int NOT NULL,
    ProductName nvarchar(50) NOT NULL,
    Quantity int NOT NULL DEFAULT 1,
    ProductPrice dec (38,2) NOT NULL,
    CONSTRAINT PK_PerishableCart 
        PRIMARY KEY (ProductID, UserID, BatchNumber),
    CONSTRAINT FK_PC_PerishableProduct FOREIGN KEY (ProductID) 
        REFERENCES PerishableProduct(ProductID),
    CONSTRAINT FK_PC_Batch FOREIGN KEY (ProductID, BatchNumber)
        REFERENCES Delivery(ProductID, BatchNumber),
    CONSTRAINT FK_PC_SC FOREIGN KEY (UserID)
        REFERENCES ShoppingCart(UserID)
);
GO

CREATE TABLE NonPerishableCart(
    ProductID int NOT NULL,
    UserID int NOT NULL,
    ProductName nvarchar(50) NOT NULL,
    Quantity int NOT NULL DEFAULT 1,
    ProductPrice dec(38,2) NOT NULL,
    CONSTRAINT PK_NPC PRIMARY KEY (ProductID, UserID),
    CONSTRAINT FK_NPC_SC FOREIGN KEY (UserID)
        REFERENCES ShoppingCart(UserID),
    CONSTRAINT FFK_NPC_Product FOREIGN KEY (ProductID)
        REFERENCES NonPerishableProduct(ProductID)
);
GO

CREATE TABLE Order(
    OrderID int IDENTITY(1,1) NOT NULL PRIMARY Key,
    UserID int,
    AddressID int,
    FeeID int,
    Tax dec(38,2) NOT NULL,
    ShippingCost dec(38,2) NOT NULL,
    Cost dec(38,2) NOT NULL,
    CONSTRAINT FK_Order_Profile FOREIGN KEY (UserID)
        REFERENCES Profile(UserID),
    CONSTRAINT FK_Order_Address FOREIGN KEY (AddressID)
        REFERENCES Address(AddressID),
    CONSTRAINT FK_Order_Fee FOREIGN KEY (FeeID)
        REFERENCES Fee(FeeID)
);
GO

CREATE TABLE OrderedPerishable(
    ProductID int NOT NULL,
    OrderID int NOT NULL,
    BatchNumber int NOT NULL,
    ProductName nvarchar(50) NOT NULL,
    Quantity int NOT NULL DEFAULT 1,
    ProductPrice dec (38,2) NOT NULL,
    CONSTRAINT PK_OrderedPerishable 
        PRIMARY KEY (ProductID, OrderID, BatchNumber),
    CONSTRAINT FK_OP_PerishableProduct FOREIGN KEY (ProductID) 
        REFERENCES PerishableProduct(ProductID),
    CONSTRAINT FK_OP_Batch FOREIGN KEY (ProductID, BatchNumber)
        REFERENCES Delivery(ProductID, BatchNumber),
    CONSTRAINT FK_OP_Order FOREIGN KEY (OrderID)
        REFERENCES Order(OrderID)
);
GO

CREATE TABLE OrderedNonPerishable(
    ProductID int NOT NULL,
    OrderID int NOT NULL,
    ProductName nvarchar(50) NOT NULL,
    Quantity int NOT NULL DEFAULT 1,
    ProductPrice dec(38,2) NOT NULL,
    CONSTRAINT PK_NPC PRIMARY KEY (ProductID, OrderID),
    CONSTRAINT FK_NPC_Order FOREIGN KEY (OrderID)
        REFERENCES Order(OrderID),
    CONSTRAINT FFK_NPC_Product FOREIGN KEY (ProductID)
        REFERENCES NonPerishableProduct(ProductID)
);
GO

