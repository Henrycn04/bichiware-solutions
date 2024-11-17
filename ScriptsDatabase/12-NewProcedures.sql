
CREATE PROCEDURE GetActiveOrdersByUserID
    @UserID INT
AS
BEGIN
    SELECT 
        OrderID,
        UserID,
        AddressID,
        FeeID,
        Tax,
        ShippingCost,
        ProductCost,
        OrderStatus,
        DeliveryDate
    FROM 
        [dbo].[Orders]
    WHERE 
        UserID = @UserID AND OrderStatus = 2 OR OrderStatus = 4;
END;

GO
CREATE PROCEDURE GetProductsByOrderID
    @OrderID INT
AS
BEGIN
    SELECT 
        ProductID,
        ProductName,
        Quantity,
        ProductPrice,
        BatchNumber,
        'Perishable' AS ProductType
    FROM 
        [dbo].[OrderedPerishable]
    WHERE 
        OrderID = @OrderID

    UNION ALL

    SELECT 
        ProductID,
        ProductName,
        Quantity,
        ProductPrice,
        NULL AS BatchNumber,
        'NonPerishable' AS ProductType
    FROM 
        [dbo].[OrderedNonPerishable]
    WHERE 
        OrderID = @OrderID;
END;
