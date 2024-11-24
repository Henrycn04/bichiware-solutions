DROP Proc GetCombinedProducts
GO
CREATE PROCEDURE GetCombinedProducts
    @ID INT
AS
BEGIN
    SELECT 
        COALESCE(p.ProductID, np.ProductID) AS ProductID,
        COALESCE(p.ProductName, np.ProductName) AS Name,
        COALESCE(p.Price, np.Price) AS Price,
        COALESCE(p.Category, np.Category) AS Category,
        COALESCE(p.CompanyName, np.CompanyName) AS CompanyName,
        COALESCE(p.ImageURL, np.ImageURL) AS ImageURL,
        COALESCE(p.ProductDescription, np.ProductDescription) AS Description,
        COALESCE(p.CompanyID, np.CompanyID) AS CompanyID,
        CASE 
            WHEN p.DeliveryDays IS NOT NULL THEN p.DeliveryDays 
            ELSE NULL
        END AS DeliveryDays,
        COALESCE(np.Weight, p.Weight) AS Weight,
        CASE 
            WHEN np.Stock IS NOT NULL THEN np.Stock 
            ELSE NULL
        END AS Stock,
        CASE 
            WHEN p.ProductionLimit IS NOT NULL THEN p.ProductionLimit 
            ELSE NULL
        END AS ProductionLimit
    FROM 
        PerishableProduct p
    FULL OUTER JOIN 
        NonPerishableProduct np
    ON 
        p.ProductID = np.ProductID
    WHERE 
        (p.ProductID = @ID OR np.ProductID = @ID) AND (p.Deleted = 0 OR np.Deleted = 0);
END;
GO
DROP Proc GetCompanyProducts
GO
create procedure GetCompanyProducts
	@companyId int
as begin
	select NP.ProductID from NonPerishableProduct as NP where NP.CompanyID = @companyId AND NP.Deleted = 0
	union
	select P.ProductID from PerishableProduct as P where P.CompanyID = @companyId AND P.Deleted = 0
end;
GO

DROP Trigger trigger_CompanyName_PerishableProduct
CREATE TRIGGER trigger_CompanyName_PerishableProduct
ON PerishableProduct
AFTER INSERT
AS
BEGIN
    UPDATE pp
    SET pp.CompanyName = c.CompanyName
    FROM PerishableProduct pp
    INNER JOIN INSERTED i ON pp.ProductID = i.ProductID
    INNER JOIN Company c ON i.CompanyID = c.CompanyID
		where c.Deleted != 1;
END;
GO

DROP Trigger trigger_CompanyName_NonPerishableProduct
CREATE TRIGGER trigger_CompanyName_NonPerishableProduct
ON NonPerishableProduct
AFTER INSERT
AS
BEGIN
    UPDATE npp
    SET npp.CompanyName = c.CompanyName
    FROM NonPerishableProduct npp
    INNER JOIN INSERTED i ON npp.ProductID = i.ProductID
    INNER JOIN Company c ON i.CompanyID = c.CompanyID
		where c.Deleted != 1;
END;

DROP PROC FindOrderedPerishables
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
	WHERE op.OrderID = @OID AND c.Deleted = 0;
END;
GO

DROP PROC FindOrderedNonPerishables
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
	WHERE onp.OrderID = @OID and c.Delted = 0;
END;
GO

DROP PROC FindCompaniesDataFromOrderID
CREATE PROCEDURE FindCompaniesDataFromOrderID
    @OrderID int
AS
BEGIN
    SELECT c.CompanyID, c.CompanyName, c.EmailAddress
    FROM Orders o
    INNER JOIN OrderedNonPerishable onp ON onp.OrderID = o.OrderID
    INNER JOIN NonPerishableProduct npp ON npp.ProductID = onp.ProductID
    INNER JOIN Company c ON c.CompanyID = npp.CompanyID
    WHERE o.OrderID = @OrderID AND c.Deleted = 0

    UNION

    SELECT c.CompanyID, c.CompanyName, c.EmailAddress
    FROM Orders o
    INNER JOIN OrderedPerishable op ON op.OrderID = o.OrderID
    INNER JOIN PerishableProduct pp ON pp.ProductID = op.ProductID
    INNER JOIN Company c ON c.CompanyID = pp.CompanyID
    WHERE o.OrderID = @OrderID AND c.Deleted = 0
END;
GO

DROP PROC FindOrderedProductsRelatedToACompany
CREATE PROCEDURE FindOrderedProductsRelatedToACompany
	@OrderID int,
	@CompanyID int
AS
BEGIN
	SELECT onp.ProductName, npp.Category, c.CompanyName, onp.ProductPrice, onp.Quantity, npp.ImageURL, c.CompanyID
    FROM Orders o
    INNER JOIN OrderedNonPerishable onp ON onp.OrderID = o.OrderID
    INNER JOIN NonPerishableProduct npp ON npp.ProductID = onp.ProductID
    INNER JOIN Company c ON c.CompanyID = npp.CompanyID
	WHERE o.OrderID = @OrderID AND npp.CompanyID = @CompanyID AND c.Deleted = 0

	UNION

	SELECT onp.ProductName, npp.Category, c.CompanyName, onp.ProductPrice, onp.Quantity, npp.ImageURL, c.CompanyID
    FROM Orders o
    INNER JOIN OrderedPerishable onp ON onp.OrderID = o.OrderID
    INNER JOIN PerishableProduct npp ON npp.ProductID = onp.ProductID
    INNER JOIN Company c ON c.CompanyID = npp.CompanyID
	WHERE o.OrderID = @OrderID AND npp.CompanyID = @CompanyID AND c.Deleted = 0
END;
GO

DROP PROC GetOrdersByFilters
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
                    WHERE op.OrderID = o.OrderID AND co.Deleted = 0

                    UNION ALL

                    SELECT onp.OrderID, co.CompanyName
                    FROM OrderedNonPerishable onp
                    INNER JOIN NonPerishableProduct npp ON npp.ProductID = onp.ProductID
                    INNER JOIN Company co ON co.CompanyID = npp.CompanyID
                    WHERE onp.OrderID = o.OrderID AND co.Deleted = 0
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
GO

DROP PROC ClientGetOrders
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
    @CompanyName NVARCHAR(20) = NULL,
    @AllCompanies NVARCHAR(255) = NULL
AS
BEGIN
    SELECT 
        o.OrderID, 
        -- Concatenate the company names
        STUFF((
            SELECT ', ' + c.CompanyName
            FROM (
                SELECT DISTINCT c.CompanyName
                FROM (
                    SELECT op.OrderID, co.CompanyName
                    FROM OrderedPerishable op
                    INNER JOIN PerishableProduct pp ON pp.ProductID = op.ProductID
                    INNER JOIN Company co ON co.CompanyID = pp.CompanyID
                    WHERE op.OrderID = o.OrderID AND co.Deleted = 0
                    UNION ALL
                    SELECT onp.OrderID, co.CompanyName
                    FROM OrderedNonPerishable onp
                    INNER JOIN NonPerishableProduct npp ON npp.ProductID = onp.ProductID
                    INNER JOIN Company co ON co.CompanyID = npp.CompanyID
                    WHERE onp.OrderID = o.OrderID AND co.Deleted = 0
                ) c
                GROUP BY c.CompanyName
            ) c
            FOR XML PATH('') 
        ), 1, 2, '') AS AllCompanies, 

        -- Calculate total quantity for the order
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
        ), 0) AS Quantity,

        -- Other order details
        o.CreationDate, o.SentDate, o.DeliveredDate, o.CancellationDate,
        o.CancelledBy, o.OrderStatus, o.ProductCost + o.Tax AS ProductCost, 
        o.ShippingCost,
        o.ProductCost + o.Tax + o.ShippingCost AS Total
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
                                WHERE op.OrderID = o.OrderID AND co.Deleted = 0

                                UNION ALL

                                SELECT onp.OrderID, co.CompanyName
                                FROM OrderedNonPerishable onp
                                INNER JOIN NonPerishableProduct npp ON npp.ProductID = onp.ProductID
                                INNER JOIN Company co ON co.CompanyID = npp.CompanyID
                                WHERE onp.OrderID = o.OrderID AND co.Deleted = 0
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
        AND (o.OrderStatus = @OrderStatus 
            OR (@OrderStatus = 2 AND o.OrderStatus IN (1, 2, 4))
        )
        AND (@CompanyName IS NULL OR 
            EXISTS (
                -- Filter for CompanyName
                SELECT 1
                FROM OrderedPerishable op
                INNER JOIN PerishableProduct pp ON pp.ProductID = op.ProductID
                WHERE op.OrderID = o.OrderID AND pp.CompanyName = @CompanyName
                UNION
                SELECT 1
                FROM OrderedNonPerishable onp
                INNER JOIN NonPerishableProduct npp ON npp.ProductID = onp.ProductID
                WHERE onp.OrderID = o.OrderID AND npp.CompanyName = @CompanyName
            )
        )
    GROUP BY
        o.OrderID, o.CreationDate, o.SentDate, o.DeliveredDate, 
        o.ProductCost, o.ShippingCost, o.Tax, o.CancellationDate,
        o.CancelledBy, o.OrderStatus;
END;
GO

DROP PROC GetCancelledOrdersByFilters
CREATE PROCEDURE GetCancelledOrdersByFilters
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
						WHERE op.OrderID = o.OrderID AND co.Deleted = 0

						UNION ALL

						SELECT onp.OrderID, co.CompanyName
						FROM OrderedNonPerishable onp
						INNER JOIN NonPerishableProduct npp ON npp.ProductID = onp.ProductID
						INNER JOIN Company co ON co.CompanyID = npp.CompanyID
						WHERE onp.OrderID = o.OrderID AND co.Deleted = 0
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

			o.CreationDate, o.CancellationDate, o.CancelledBy, 
			(o.ProductCost + o.Tax) AS ProductCost, o.ShippingCost,
			(o.ProductCost + o.Tax + o.ShippingCost) AS Total
	FROM Orders o
	WHERE 
		o.OrderStatus = 3
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
		o.OrderID, o.CreationDate, o.CancellationDate, o.CancelledBy, 
		o.ProductCost, o.ShippingCost, o.Tax;
END
GO

drop proc GetPendingOrdersOfCompany
create procedure GetPendingOrdersOfCompany
	@companyId int
as begin
	declare @orderCursor as cursor 
	declare @orderId int

    if (select Deleted from Company where CompanyID = @companyId) = 0
    begin
	    set @orderCursor = cursor for 
	    select ONP.OrderID from OrderedNonPerishable as ONP where exists (
		    select NP.ProductID from NonPerishableProduct as NP where NP.CompanyID = @companyId and NP.ProductID = ONP.ProductID
	    )
	    union
	    select OP.OrderID from OrderedPerishable as OP where exists (
		    select P.ProductID from PerishableProduct as P where P.CompanyID = @companyId and P.ProductID = OP.ProductID
	    )

	    open @orderCursor
	    fetch next from @orderCursor into @orderId
	    while @@FETCH_STATUS = 0
	    begin
		    select OrderId from Orders as O where O.OrderStatus = 2 and OrderID = @orderId

		    fetch next from @orderCursor into @orderId
	    end
	    close @orderCursor
	    deallocate @orderCursor
    end
end;

Go
drop proc GetOrdersOfCompany
create procedure GetOrdersOfCompany
	@companyId int
as begin
	declare @orderCursor as cursor 
	declare @orderId int

    if (select Deleted from Company where CompanyID = @companyId) = 0
    begin
	    set @orderCursor = cursor for 
	    select ONP.OrderID from OrderedNonPerishable as ONP where exists (
		    select NP.ProductID from NonPerishableProduct as NP where NP.CompanyID = @companyId and NP.ProductID = ONP.ProductID
	    )
	    union
	    select OP.OrderID from OrderedPerishable as OP where exists (
		    select P.ProductID from PerishableProduct as P where P.CompanyID = @companyId and P.ProductID = OP.ProductID
	    )

	    open @orderCursor
	    fetch next from @orderCursor into @orderId
	    while @@FETCH_STATUS = 0
	    begin
		    select OrderId from Orders as O where OrderID = @orderId

		    fetch next from @orderCursor into @orderId
	    end
	    close @orderCursor
	    deallocate @orderCursor
    end
end;

Go
drop proc GetPendingOrderReport
create procedure GetPendingOrderReport
    @companyId int
as begin
	declare @orderCursor as cursor 
	declare @orderId int

    if (select Deleted from Company where CompanyID = @companyId) = 0
    begin
	    set @orderCursor = cursor for 
	    select ONP.OrderID from OrderedNonPerishable as ONP where exists (
		    select NP.ProductID from NonPerishableProduct as NP where NP.CompanyID = @companyId and NP.ProductID = ONP.ProductID
	    )
	    union
	    select OP.OrderID from OrderedPerishable as OP where exists (
		    select P.ProductID from PerishableProduct as P where P.CompanyID = @companyId and P.ProductID = OP.ProductID
	    )

	    open @orderCursor
	    fetch next from @orderCursor into @orderId
	    while @@FETCH_STATUS = 0
	    begin
		    select OrderId, OrderStatus, CreationDate, DeliveryDate, ProductCost, ShippingCost, Tax from Orders as O where O.OrderStatus = 2 and OrderID = @orderId

		    fetch next from @orderCursor into @orderId
	    end
	    close @orderCursor
	    deallocate @orderCursor
    end
end;

Go
drop proc GetCompaniesNamesOfOrder
create procedure GetCompaniesNamesOfOrder
    @orderId int
as begin
    declare @companyCursor as cursor
    declare @companyId int

    set @companyCursor = cursor for
    select NP.CompanyID from NonPerishableProduct as NP where exists (
        select ONP.ProductID from OrderedNonPerishable as ONP where ONP.OrderID = @orderId and NP.ProductID = ONP.ProductID
    )
    union
    select P.CompanyID from PerishableProduct as P where exists (
        select OP.ProductID from OrderedPerishable as OP where OP.OrderID = @orderId and P.ProductID = OP.ProductID
    )

    open @companyCursor
    fetch next from @companyCursor into @companyId
    while @@FETCH_STATUS = 0
    begin
        select CompanyName from Company where CompanyId = @companyId AND Deleted = 0

        fetch next from @companyCursor into @companyId
    end
    close @companyCursor
    deallocate @companyCursor
end;

Go
drop proc GetPendingReportOfAllUserCompanies
create procedure GetPendingReportOfAllUserCompanies
    @userId int
as begin
    declare @companyCursor as cursor
    set @companyCursor = cursor for
    select cop.CompanyID from CompanyProfiles as cop
    inner join Company as co on cop.CompanyID = co.CompanyID
    where cop.UserID = @userId and co.Deleted = 0

    open @companyCursor
    declare @companyId int
    fetch next from @companyCursor into @companyId

    while @@FETCH_STATUS = 0
    begin
        exec GetPendingOrderReport @companyId = @companyId
        fetch next from @companyCursor into @companyId
    end
    close @companyCursor
    deallocate @companyCursor
end;
