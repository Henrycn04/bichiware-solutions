-- Index on order creation, since it is used to know in which date
-- of a graph or report a certain order should be counted in, 
-- this allows a faster creation of reports and graphs, which are
-- some of the most expensive queries
-- The index for status, status is one of the most used
-- columns in the table, but since (in theory) it should be populated
-- by mostly status 5 (completed) it doesnt make sense to make it a normal index
-- so it is made into a composite index with creation date
CREATE INDEX index_Orders_CreationDate
ON Orders (CreationDate, OrderStatus);
GO



-- Index in the email of profiles so the log-in process
-- can be made as quickly as possible without having to look
-- into all profiles before the correct one, this makes the log in
-- very scalable in the matter of a higher number of profiles
CREATE INDEX index_Profile_Email
ON Profile (Email);
GO

-- Both of these indexes exist to facilitate the search of products
-- associated to a specific company, this is mostly used for the sake of
-- reports and graphs, helping to speed up some of the heaviest queries
-- in the system
CREATE INDEX index_NonPerishableProduct_CompanyID
ON NonPerishableProduct (CompanyID);
GO
CREATE INDEX index_PerishableProduct_CompanyID
ON PerishableProduct (CompanyID);
GO

-- The category is added because of scalability, as the number of
-- products grow, it will become harder and harder to find products by their 
-- category, this index helps in that and thus speeds up the product
-- search for the users.
CREATE NONCLUSTERED INDEX NC_index_NonPerishableProduct_Category
ON NonPerishableProduct (Category);
GO
CREATE NONCLUSTERED INDEX NC_index_PerishableProduct_Category
ON PerishableProduct (Category);
GO