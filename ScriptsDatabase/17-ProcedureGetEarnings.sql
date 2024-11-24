CREATE PROCEDURE GetLastYearOrders
    @CompanyID INT = NULL
AS
BEGIN
    SELECT 
        o.OrderID,
        o.CreationDate, o.SentDate, o.DeliveredDate, 
        (o.ProductCost + o.Tax) AS ProductCost, 
        o.ShippingCost,
        (o.ProductCost + o.Tax + o.ShippingCost) AS Total
    FROM Orders o
    WHERE 
        o.OrderStatus = 5
        AND o.CreationDate >= DATEADD(YEAR, -1, GETDATE())
        AND (@CompanyID IS NULL OR 
            EXISTS (
                SELECT 1
                FROM OrderedPerishable op
                INNER JOIN PerishableProduct pp ON pp.ProductID = op.ProductID
                WHERE op.OrderID = o.OrderID AND pp.CompanyID = @CompanyID
                UNION
                SELECT 1
                FROM OrderedNonPerishable onp
                INNER JOIN NonPerishableProduct npp ON npp.ProductID = onp.ProductID
                WHERE onp.OrderID = o.OrderID AND npp.CompanyID = @CompanyID
            )
        )
    GROUP BY
        o.OrderID, o.CreationDate, o.SentDate, o.DeliveredDate, 
        o.ProductCost, o.ShippingCost, o.Tax;
END;
select * from Profile