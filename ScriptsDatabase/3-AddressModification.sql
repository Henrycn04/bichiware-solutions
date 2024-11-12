ALTER TABLE dbo.Address
	ADD Latitude DECIMAL(13, 10) NOT NULL DEFAULT (0) CHECK (Latitude > -90 AND Latitude < 90)
	
ALTER TABLE dbo.Address
	ADD Longitude DECIMAL (13, 10) NOT NULL DEFAULT (0) CHECK (Longitude > -180 AND Longitude < 180)
