BEGIN TRANSACTION

-- Uncomment if you want to delete everything
--DELETE FROM PerfilesEmpresa;
--DELETE FROM MiembrosEmpresa;
--DELETE FROM Usuario;
--DELETE FROM Empresa;
--DELETE FROM Perfil;

-- Inserts test data into Perfil table
INSERT INTO Perfil
VALUES(10,'Andre21','andre.salas.cr.201@gmail.com','Hola_1234','CRC012341',86430421);
GO

-- Inserts test data into Usuario, linking with the data in perfil
INSERT INTO Usuario
VALUES('Andre21',2,10,'andre.salas.cr.201@gmail.com',134324221);
GO

-- Inserts test data into Empresa
INSERT INTO Empresa
VALUES('Bichiware Solutions', 123456789, 193);
GO

-- Inserts test data into MiembrosEmpresa, linking with Usuario and Empresa
INSERT INTO MiembrosEmpresa
VALUES (10,193);
GO

-- Inserts test data into PerfilesEmpresa, linking with Perfil and Empresa
INSERT INTO PerfilesEmpresa
VALUES(10,193);
GO

-- Commits all changes
COMMIT;

-- Uncomment to check the tables
--SELECT * FROM Perfil;
--SELECT * FROM Usuario;
--SELECT * FROM Empresa;
--SELECT * FROM MiembrosEmpresa;
--SELECT * FROM PerfilesEmpresa;
