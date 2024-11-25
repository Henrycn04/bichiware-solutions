CREATE PROCEDURE GetOrdersByFilters
    @CompanyID INT = NULL
AS
BEGIN
SELECT 
        o.OrderID,
        STUFF((
            SELECT ', ' + c.CompanyName
            FROM (
                SELECT DISTINCT c.CompanyName  
                FROM (
                    SELECT op.OrderID, co.CompanyName
                    FROM OrderedPerishable op
                    INNER JOIN PerishableProduct pp ON pp.ProductID = op.ProductID
                    INNER JOIN Company co ON co.CompanyID = pp.CompanyID
                    WHERE op.OrderID = o.OrderID

                    UNION ALL

                    SELECT onp.OrderID, co.CompanyName
                    FROM OrderedNonPerishable onp
                    INNER JOIN NonPerishableProduct npp ON npp.ProductID = onp.ProductID
                    INNER JOIN Company co ON co.CompanyID = npp.CompanyID
                    WHERE onp.OrderID = o.OrderID
                ) c
                GROUP BY c.CompanyName  
            ) c
            FOR XML PATH('')
        ), 1, 2, '') AS AllCompanies,
        ISNULL(( 
            SELECT SUM(c.Quantity)
            FROM (
                SELECT op.OrderID, op.Quantity
                FROM OrderedPerishable op
                WHERE op.OrderID = o.OrderID

                UNION ALL

                SELECT onp.OrderID, onp.Quantity
                FROM OrderedNonPerishable onp
                WHERE onp.OrderID = o.OrderID
            ) c
        ), 0) AS Quantity,

        o.CreationDate, o.SentDate, o.DeliveredDate, 
        (o.ProductCost + o.Tax) AS ProductCost, o.ShippingCost,
        (o.ProductCost + o.Tax + o.ShippingCost) AS Total
FROM Orders o
WHERE 
    o.OrderStatus = 5
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