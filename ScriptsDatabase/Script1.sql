CREATE TABLE Usuario(
	NombreUsuario nvarchar(20) NOT NULL,
	Tipo int NOT NULL, 
	IDUsuario int NOT NULL PRIMARY KEY, 
	CorreoElectronico nvarchar(50) NOT NULL,
	Cedula int NOT NULL
);

CREATE TABLE Perfil(
	IDUsuario int NOT NULL PRIMARY KEY,
	NombrePerfil nvarchar(20) NOT NULL,
	CorreoElectronico nvarchar(50) NOT NULL,
	Contasena nvarchar(20) NOT NULL,
	CuentaBancaria varchar(50),
	NumeroTelefono int NOT NULL
);

CREATE TABLE Empresa(
	NombreEmpresa nvarchar(20) NOT NULL,
	CedulaJuridica int NOT NULL,
	IDEmpresa int NOT NULL PRIMARY KEY
);

CREATE TABLE MiembrosEmpresa (
	IDUsuario int,
	IDEmpresa int,
	CONSTRAINT MEPrimaries PRIMARY KEY (IDUsuario, IDEmpresa),
	CONSTRAINT MEUsuario FOREIGN KEY (IDUsuario)
	REFERENCES Usuario(IDUsuario),
	CONSTRAINT MEEmpresa FOREIGN KEY (IDEmpresa)
	REFERENCES Empresa(IDEmpresa)
);

CREATE TABLE PerfilesEmpresa (
	IDUsuario int,
	IDEmpresa int,
	CONSTRAINT PEPrimaries PRIMARY KEY (IDUsuario, IDEmpresa),
	CONSTRAINT PEUsuario FOREIGN KEY (IDUsuario)
	REFERENCES Perfil(IDUsuario),
	CONSTRAINT PEEmpresa FOREIGN KEY (IDEmpresa)
	REFERENCES Empresa(IDEmpresa)
);

ALTER TABLE Usuario 
ADD CONSTRAINT PerfilUser 
FOREIGN KEY (IDUsuario) REFERENCES Perfil(IDUsuario)