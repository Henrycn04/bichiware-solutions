BEGIN TRANSACTION
use BichiwareSolutions

-- Uncomment if you want to delete everything
--DELETE FROM PerfilesEmpresa;
--DELETE FROM MiembrosEmpresa;
--DELETE FROM Entrega;
--DELETE FROM ProductoNoPerecedero;
--DELETE FROM ProductoPerecedero;
--DELETE FROM UserDirecc;
--DELETE FROM EmpresaDirecc;
--DELETE FROM Usuario;
--DELETE FROM Empresa;
--DELETE FROM Perfil;
--DELETE FROM Direccion;

 --Inserts test data into Perfil table
INSERT INTO Perfil
VALUES(10,'Andre21','andre.salas.cr.201@gmail.com','Hola_1234','CRC012341',86430421,'Activo'
	,'318f3f005a97a26456bb9b77024eabbeba2ad71ca423636a16fd3430bc9e9f4b347d9b9856ebc238262b0bd688b1d6333653e0d6faf77abbc3622c523326c5c7'
	,'2024-11-09 12:30:49');
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

-- Inserts test data into Direccion
INSERT INTO Direccion (IDDireccion, Provincia, Canton, Distrito, Direccion_exacta)
VALUES (1, 'San José', 'Central', 'Merced', 'Avenida 2, Calle 5, Casa 7');
GO


-- Inserts test data into ProductoNoPerecedero, linking with data in Empresa
INSERT INTO ProductoNoPerecedero (IDProducto, NombreProducto, IDEmpresa, ImagenURL, Categoria, Precio, Descripcion, Existencias)
VALUES (1, 'Computadora', 193, 'http://computadora.jpg', 'Electronicos', 800, 'Laptop HP con 16GB de RAM y 512GB SSD', 10);


-- Inserts test data into  ProductoPerecedero, linking with data in Empresa
INSERT INTO ProductoPerecedero (IDProducto, NombreProducto, IDEmpresa, ImagenURL, Categoria, Precio, Descripcion, DiasEntrega, LimiteProduccion)
VALUES (1, 'Pan Baguette', 193, 'http://example.com/images/pan_baguette.jpg', 'Alimentos', 2, 'Pan baguette fresco y crujiente', 'Lunes', 100);


-- Inserts test data into Entrega, linking with data in ProductoPerecedero
INSERT INTO Entrega (IDProducto, NumeroLote, UnidadesApartadas, FechaExpiracion)
VALUES (1, 1001, 50, '2024-10-01');


-- Inserts test data into UserDirecc, linking with data in Usuario and Direccion
INSERT INTO UserDirecc (IDUsuario, IDDirecc)
VALUES (10, 1); 


-- Inserts test data into EmpresaDirecc, linking with data in Empresa and Direccion
INSERT INTO EmpresaDirecc (IDEmpresa, IDDirecc)
VALUES (193, 1);

-- Commits all changes
COMMIT;

-- Uncomment to check the tables
--SELECT * FROM Perfil;
--SELECT * FROM Usuario;
--SELECT * FROM Empresa;
--SELECT * FROM MiembrosEmpresa;
--SELECT * FROM PerfilesEmpresa;
--SELECT * FROM Direccion;
--SELECT * FROM ProductoNoPerecedero;
--SELECT * FROM ProductoPerecedero;
--SELECT * FROM Entrega;
--SELECT * FROM UserDirecc;
--SELECT * FROM EmpresaDirecc;
