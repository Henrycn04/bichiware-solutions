-- Fix: Profile table was missing important attributes
ALTER TABLE Perfil
	ADD Estado nvarchar(50)
		NOT NULL
		DEFAULT 'Inactivo'
		CHECK (Estado in ('Activo', 'Inactivo', 'Bloqueado'))

ALTER TABLE	Perfil
	-- Stores the hashed confirmation code using SHA-512
	ADD CodigoConfirmacion NVARCHAR(128)

ALTER TABLE Perfil
	ADD FechaHoraDeUltimoCodigo DATETIME2(2)
		NOT NULL
		DEFAULT SYSDATETIME()
GO
