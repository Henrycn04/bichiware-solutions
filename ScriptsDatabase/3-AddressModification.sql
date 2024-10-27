ALTER TABLE dbo.Address
	ADD Latitude DECIMAL(13, 10) NOT NULL
	
ALTER TABLE dbo.Address
	ADD Longitude DECIMAL (13, 10) NOT NULL

GO

ALTER TABLE dbo.Address
	ADD CONSTRAINT LatitudeDefaultValue DEFAULT(0) FOR Latitude

ALTER TABLE dbo.Address
	ADD CONSTRAINT LongitudeDefaultValue DEFAULT(0) FOR Longitude

ALTER TABLE dbo.Address
	ADD CONSTRAINT LatitudeLimits CHECK (Latitude > -90 AND Latitude < 90)

ALTER TABLE dbo.Address
	ADD CONSTRAINT LongitudeLimits CHECK (Longitude > -180 AND Longitude < 180)
