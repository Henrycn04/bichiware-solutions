
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

CREATE PROCEDURE ClientGetOrders
    @OrderStatus INT,
	@UID INT = NULL,
	@CreationStartDate DATE = NULL,
	@CreationEndDate DATE = NULL,
    @SentStartDate DATE = NULL,
    @SentEndDate DATE = NULL,
    @DeliveredStartDate DATE = NULL,
    @DeliveredEndDate DATE = NULL,
	@CancelledStartDate DATE = NULL,
	@CancelledEndDate DATE = NULL,
	@CancelledBy INT = NULL, 
	@minShippingCost DECIMAL(18, 2) = NULL,
	@maxShippingCost DECIMAL(18, 2) = NULL,
	@minProductCost DECIMAL(18, 2) = NULL,
	@maxProductCost DECIMAL(18, 2) = NULL,
    @minTotal DECIMAL(18, 2) = NULL,
	@maxTotal DECIMAL(18, 2) = NULL,
	@minQuantity INT = NULL,
	@maxQuantity INT = NULL,
	@OrderID INT = NULL,
	@CompanyName nvarchar(20) = NULL,
	@AllCompanies nvarchar(255) = NULL
AS
BEGIN
    SELECT 
        o.OrderID, 

        -- Concatenación de empresas asociadas
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
        (@UID IS NULL OR o.UserID = @UID)  
        AND (@OrderID IS NULL OR o.OrderID = @OrderID)
        AND (@AllCompanies IS NULL OR 
            CHARINDEX(@AllCompanies, 
                ISNULL((
                    SELECT STUFF((
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
                    ), 1, 2, '')
                ), '')
            ) > 0
        )
        AND (@minQuantity IS NULL OR 
            ISNULL((
                SELECT SUM(c.Quantity)
                FROM (
                    SELECT op.Quantity
                    FROM OrderedPerishable op
                    WHERE op.OrderID = o.OrderID

                    UNION ALL

                    SELECT onp.Quantity
                    FROM OrderedNonPerishable onp
                    WHERE onp.OrderID = o.OrderID
                ) c
            ), 0) >= @minQuantity
        )
		AND (@maxQuantity IS NULL OR 
            ISNULL((
                SELECT SUM(c.Quantity)
                FROM (
                    SELECT op.Quantity
                    FROM OrderedPerishable op
                    WHERE op.OrderID = o.OrderID

                    UNION ALL

                    SELECT onp.Quantity
                    FROM OrderedNonPerishable onp
                    WHERE onp.OrderID = o.OrderID
                ) c
            ), 0) <= @maxQuantity
        )
        AND (@CreationStartDate IS NULL OR o.CreationDate >= @CreationStartDate)
		AND (@CreationEndDate IS NULL OR o.CreationDate <= @CreationEndDate)
        AND (@SentStartDate IS NULL OR o.SentDate >= @SentStartDate)
        AND (@SentEndDate IS NULL OR o.SentDate <= @SentEndDate)
        AND (@DeliveredStartDate IS NULL OR o.DeliveredDate >= @DeliveredStartDate)
        AND (@DeliveredEndDate IS NULL OR o.DeliveredDate <= @DeliveredEndDate)
        AND (@minProductCost IS NULL OR (o.ProductCost + o.Tax) >= @minProductCost)
		AND (@maxProductCost IS NULL OR (o.ProductCost + o.Tax) <= @maxProductCost)
        AND (@minShippingCost IS NULL OR o.ShippingCost >= @minShippingCost)
		AND (@maxShippingCost IS NULL OR o.ShippingCost <= @maxShippingCost)
        AND (@minTotal IS NULL OR (o.ProductCost + o.Tax + o.ShippingCost) >= @minTotal)
		AND (@maxTotal IS NULL OR (o.ProductCost + o.Tax + o.ShippingCost) <= @maxTotal)
        AND (@CreationStartDate IS NULL OR o.CreationDate >= @CreationStartDate)
        AND (@CreationEndDate IS NULL OR o.CreationDate <= @CreationEndDate)
        AND (o.OrderStatus = @OrderStatus 
		OR  (@OrderStatus = 2 AND o.OrderStatus IN (1,2,4)) )
        AND (@CompanyName IS NULL OR 
            EXISTS (
                -- Filtro adicional para CompanyID
                SELECT 1
                FROM OrderedPerishable op
                INNER JOIN PerishableProduct pp ON pp.ProductID = op.ProductID
                WHERE op.OrderID = o.OrderID AND pp.CompanyID = @CompanyName
                UNION
                SELECT 1
                FROM OrderedNonPerishable onp
                INNER JOIN NonPerishableProduct npp ON npp.ProductID = onp.ProductID
                WHERE onp.OrderID = o.OrderID AND npp.CompanyID = @CompanyName
            )
        )
    GROUP BY
        o.OrderID, o.CreationDate, o.SentDate, o.DeliveredDate, 
        o.ProductCost, o.ShippingCost, o.Tax;
END;


