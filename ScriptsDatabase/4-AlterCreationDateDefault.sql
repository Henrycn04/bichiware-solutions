ALTER TABLE Profile
ADD CONSTRAINT DF_Profile_CreationDateTime DEFAULT GETDATE() FOR CreationDateTime;
GO