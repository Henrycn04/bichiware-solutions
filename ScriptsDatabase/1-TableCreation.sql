-- User Table
CREATE TABLE Usuario(
	NombreUsuario nvarchar(20) NOT NULL,
	Tipo int NOT NULL, 
	IDUsuario int NOT NULL PRIMARY KEY, 
	CorreoElectronico nvarchar(50) NOT NULL,
	Cedula int NOT NULL
);
GO

-- Profile Table
CREATE TABLE Perfil(
	IDUsuario int NOT NULL PRIMARY KEY,
	NombrePerfil nvarchar(20) NOT NULL,
	CorreoElectronico nvarchar(50) NOT NULL,
	Contasena nvarchar(20) NOT NULL,
	CuentaBancaria varchar(50),
	NumeroTelefono int NOT NULL
);
GO

-- Company table
CREATE TABLE Empresa(
	IDEmpresa int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	NombreEmpresa nvarchar(20) NOT NULL,
	CedulaJuridica int NOT NULL
);
GO

-- CompanyMembers table
CREATE TABLE MiembrosEmpresa (
	IDUsuario int,
	IDEmpresa int,
	CONSTRAINT MEPrimaries PRIMARY KEY (IDUsuario, IDEmpresa),
	CONSTRAINT MEUsuario FOREIGN KEY (IDUsuario)
	 REFERENCES Usuario(IDUsuario)
	 ON DELETE CASCADE,
	CONSTRAINT MEEmpresa FOREIGN KEY (IDEmpresa)
	 REFERENCES Empresa(IDEmpresa)
	 ON DELETE CASCADE
);
GO

-- CompanyProfiles table
CREATE TABLE PerfilesEmpresa (
	IDUsuario int,
	IDEmpresa int,
	CONSTRAINT PEPrimaries PRIMARY KEY (IDUsuario, IDEmpresa),
	CONSTRAINT PEUsuario FOREIGN KEY (IDUsuario)
	 REFERENCES Perfil(IDUsuario)
	 ON DELETE CASCADE,
	CONSTRAINT PEEmpresa FOREIGN KEY (IDEmpresa)
	 REFERENCES Empresa(IDEmpresa)
	 ON DELETE CASCADE
);
GO

-- Add foregin key restriction for User table
ALTER TABLE Usuario 
ADD CONSTRAINT PerfilUser 
FOREIGN KEY (IDUsuario) REFERENCES Perfil(IDUsuario);
GO

-- Address table
CREATE TABLE Direccion(
IDDireccion int IDENTITY(1,1) NOT NULL PRIMARY KEY,
Provincia nvarchar(50) NOT NULL,
Canton nvarchar(50) NOT NULL,
Distrito nvarchar(50) NOT NULL,
Direccion_exacta nvarchar(300) NOT NULL
);
GO

-- non-perishable product table
CREATE TABLE ProductoNoPerecedero(
IDProducto int PRIMARY KEY,
NombreProducto nvarchar(50) NOT NULL,
IDEmpresa int,
ImagenURL nvarchar(500),
Categoria nvarchar (50) CHECK (Categoria IN ('Alimentos', 'Electronicos', 'DecoracionCasa', 'Automoviles', 'DecoracionExteriores', 'Ropa', 'Joyeria', 'Limpieza')),
Precio int NOT NULL,
Descripcion nvarchar (300),
Existencias int NOT NULL,
CONSTRAINT ProdN_Empresa FOREIGN KEY (IDEmpresa)
 REFERENCES Empresa (IDEmpresa)
 ON DELETE CASCADE
);
GO

-- perishable product table
CREATE TABLE ProductoPerecedero(
IDProducto int PRIMARY KEY,
NombreProducto nvarchar (50) NOT NULL,
IDEmpresa int,
ImagenURL nvarchar(500),
Categoria nvarchar (50) CHECK (Categoria IN 
('Alimentos', 'Electronicos', 'DecoracionCasa', 'Automoviles', 'DecoracionExteriores', 'Ropa', 'Joyeria', 'Limpieza')),
Precio INT NOT NULL,
Descripcion nvarchar (300),
DiasEntrega nvarchar (50) NOT NULL CHECK (DiasEntrega IN 
('Lunes', 'Martes', 'Miercoles','Jueves','Viernes', 'Sabado', 'Domingo')),
LimiteProduccion int NOT NULL,
CONSTRAINT ProdP_Empresa FOREIGN KEY (IDEmpresa)
 REFERENCES Empresa (IDEmpresa)
 ON DELETE CASCADE
)
GO

-- Delivery table
CREATE TABLE Entrega(
IDProducto int,
NumeroLote int,
UnidadesApartadas int NOT NULL,
FechaExpiracion date NOT NULL,
CONSTRAINT EntregaPrimaries PRIMARY KEY (IDProducto, NumeroLote),
CONSTRAINT EntregaIDProducto FOREIGN KEY (IDProducto)
 REFERENCES ProductoPerecedero(IDProducto)
 ON DELETE CASCADE
);
GO

-- User address table
CREATE TABLE UserDirecc (
	IDUsuario int,
	IDDirecc int,
	CONSTRAINT UDPrimaries PRIMARY KEY (IDUsuario, IDDirecc),
	CONSTRAINT UDUsuario FOREIGN KEY (IDUsuario)
	 REFERENCES Usuario(IDUsuario)
	 ON DELETE CASCADE,
	CONSTRAINT UDDirecc FOREIGN KEY (IDDirecc)
	 REFERENCES Direccion(IDDireccion)
	 ON DELETE CASCADE
);
GO

-- Company address table
CREATE TABLE EmpresaDirecc (
	IDEmpresa int,
	IDDirecc int,
	CONSTRAINT EDPrimaries PRIMARY KEY (IDDirecc, IDEmpresa),
	CONSTRAINT EDEmpresa FOREIGN KEY (IDEmpresa)
	 REFERENCES Empresa(IDEmpresa)
	 ON DELETE CASCADE,
	CONSTRAINT EDDirecc FOREIGN KEY (IDDirecc)
	 REFERENCES Direccion(IDDireccion)
	 ON DELETE CASCADE
);
GO
