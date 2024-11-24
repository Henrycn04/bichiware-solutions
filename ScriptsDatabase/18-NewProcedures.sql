CREATE PROCEDURE GetActiveOrdersByUserID
    @UserID INT
AS
BEGIN
    SELECT TOP 10
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
        UserID = @UserID AND OrderStatus IN (1,2,4)
END;
GO
CREATE PROCEDURE GetActiveOrdersForEntrepreneurs
    @OrderID INT
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
        OrderID = @OrderID
END;
GO
CREATE PROCEDURE GetActiveOrdersForAdmins
AS
BEGIN
    SELECT TOP 10
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
        OrderStatus IN (1,2,4)
	ORDER BY 
		CreationDate DESC;
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
GO
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
create procedure GetPendingOrdersOfCompany
	@companyId int
as begin
	declare @orderCursor as cursor 
	declare @orderId int

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
end;

Go
create procedure GetOrdersOfCompany
	@companyId int
as begin
	declare @orderCursor as cursor 
	declare @orderId int

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
end;

Go
create procedure GetCompanyProducts
	@companyId int
as begin
	select NP.ProductID from NonPerishableProduct as NP where NP.CompanyID = @companyId AND NP.Deleted = 0
	union
	select P.ProductID from PerishableProduct as P where P.CompanyID = @companyId AND P.Deleted = 0
end;

Go
create procedure GetPendingOrderReport
    @companyId int
as begin
	declare @orderCursor as cursor 
	declare @orderId int

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
end;

Go
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
        select CompanyName from Company where CompanyId = @companyId

        fetch next from @companyCursor into @companyId
    end
    close @companyCursor
    deallocate @companyCursor
end;

Go
create procedure GetPendingReportOfAllUserCompanies
    @userId int
as begin
    declare @companyCursor as cursor
    set @companyCursor = cursor for
    select CompanyID from CompanyProfiles where UserID = @userId

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

CREATE PROCEDURE GetMonthlyShippingCost
    @StartDate DATE = NULL,
    @EndDate DATE = NULL
AS BEGIN
    SELECT SUM(o.ShippingCost) AS Cost, MONTH(o.DeliveredDate) AS Month, YEAR(o.DeliveredDate) AS Year
    FROM Orders o
    WHERE   ( @StartDate IS NULL OR o.DeliveredDate >= @StartDate) AND 
            (@EndDate IS NULL OR o.DeliveredDate <= @EndDate) AND 
            o.OrderStatus = 5
    GROUP BY  MONTH(o.DeliveredDate), YEAR(o.DeliveredDate)
END;