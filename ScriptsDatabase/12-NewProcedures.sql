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
		select OrderId from Orders as O where O.OrderStatus = 2

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
		select OrderId from Orders as O

		fetch next from @orderCursor into @orderId
	end
	close @orderCursor
	deallocate @orderCursor
end;

Go
create procedure GetCompanyProducts
	@companyId int
as begin
	select NP.ProductID from NonPerishableProduct as NP where NP.CompanyID = @companyId
	union
	select P.ProductID from PerishableProduct as P where P.CompanyID = @companyId
end;

