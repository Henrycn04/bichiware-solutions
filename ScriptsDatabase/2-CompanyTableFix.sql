-- Fix: Company table was missing phone number and email address
ALTER TABLE Empresa
	ADD phoneNumber int NOT NULL

ALTER TABLE Empresa
	ADD emailAddress varchar(50) NOT NULL
GO