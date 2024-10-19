ALTER TABLE PerishableProduct
ADD Weight dec(38,2) NOT NULL DEFAULT 0;
GO

ALTER TABLE NonPerishableProduct
ADD Weight dec(38,2) NOT NULL DEFAULT 0;
GO