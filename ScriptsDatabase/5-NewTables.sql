CREATE TABLE Fee(
    FeeID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    Name nvarchar(50),
    KmMin int NOT NULL,
    KmMax int NOT NULL,
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
    ProductName nvarchar(50) NOT NULL,
    Quantity int NOT NULL DEFAULT 1,
    ProductPrice dec (38,2) NOT NULL,
    CONSTRAINT PK_PerishableCart 
        PRIMARY KEY (ProductID, UserID, BatchNumber),
    CONSTRAINT FK_PC_PerishableProduct FOREIGN KEY (ProductID) 
        REFERENCES PerishableProduct(ProductID) ON DELETE CASCADE,
    CONSTRAINT FK_PC_SC FOREIGN KEY (UserID)
        REFERENCES ShoppingCart(UserID) ON DELETE CASCADE
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
        REFERENCES ShoppingCart(UserID) ON DELETE CASCADE,
    CONSTRAINT FFK_NPC_Product FOREIGN KEY (ProductID)
        REFERENCES NonPerishableProduct(ProductID) ON DELETE CASCADE
);
GO

CREATE TABLE Orders(
    OrderID int IDENTITY(1,1) NOT NULL PRIMARY Key,
    UserID int,
    AddressID int,
    FeeID int,
    Tax dec(38,2) NOT NULL,
    ShippingCost dec(38,2) NOT NULL,
    ProductCost dec(38,2) NOT NULL,
    OrderStatus int IN (1,2,3) DEFAULT 1,
    CONSTRAINT FK_Orders_Profile FOREIGN KEY (UserID)
        REFERENCES Profile(UserID) ON DELETE NO ACTION,
    CONSTRAINT FK_Orders_Address FOREIGN KEY (AddressID)
        REFERENCES Address(AddressID) ON DELETE NO ACTION,
    CONSTRAINT FK_Orders_Fee FOREIGN KEY (FeeID)
        REFERENCES Fee(FeeID) ON DELETE NO ACTION
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
        REFERENCES PerishableProduct(ProductID) ON DELETE NO ACTION,
    CONSTRAINT FK_OP_Batch FOREIGN KEY (ProductID, BatchNumber)
        REFERENCES Delivery(ProductID, BatchNumber) ON DELETE NO ACTION,
    CONSTRAINT FK_OP_Orders FOREIGN KEY (OrderID)
        REFERENCES Orders(OrderID) ON DELETE NO ACTION
);
GO

CREATE TABLE OrderedNonPerishable(
    ProductID int NOT NULL,
    OrderID int NOT NULL,
    ProductName nvarchar(50) NOT NULL,
    Quantity int NOT NULL DEFAULT 1,
    ProductPrice dec(38,2) NOT NULL,
    CONSTRAINT PK_ONP PRIMARY KEY (ProductID, OrderID),
    CONSTRAINT FK_ONP_Orders FOREIGN KEY (OrderID)
        REFERENCES Orders(OrderID) ON DELETE NO ACTION,
    CONSTRAINT FK_ONP_Product FOREIGN KEY (ProductID)
        REFERENCES NonPerishableProduct(ProductID) ON DELETE NO ACTION
);
GO

