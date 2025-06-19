-- Script de inicialización de la base de datos
-- Este script se ejecutará automáticamente cuando se inicie el contenedor de SQL Server

USE master;
GO

-- Crear la base de datos si no existe
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'AlbergueDb')
BEGIN
    CREATE DATABASE AlbergueDb;
END
GO

USE CrudApiDB;
GO

-- Crear tabla Persons
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Persons')
BEGIN
    CREATE TABLE Persons (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        FirstName NVARCHAR(100) NOT NULL,
        LastName NVARCHAR(100) NOT NULL,
        Email NVARCHAR(150) UNIQUE NOT NULL,
        BirthDate DATE,
        Height DECIMAL(5,2),
        CreatedAt DATETIME DEFAULT GETDATE()
    );
    
    -- Crear índice en Email para mejor rendimiento
    CREATE INDEX IX_Persons_Email ON Persons(Email);
    CREATE INDEX IX_Persons_FullName ON Persons(FirstName, LastName);
END
GO

-- Crear tabla Pets
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Pets')
BEGIN
    CREATE TABLE Pets (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Name NVARCHAR(100) NOT NULL,
        Species NVARCHAR(50) NOT NULL,
        Breed NVARCHAR(100),
        Color NVARCHAR(50),
        Age INT
    );
    
    -- Crear índices para mejor rendimiento
    CREATE INDEX IX_Pets_Species ON Pets(Species);
    CREATE INDEX IX_Pets_Name ON Pets(Name);
    CREATE INDEX IX_Pets_Age ON Pets(Age);
END
GO

-- Insertar datos de ejemplo
IF NOT EXISTS (SELECT * FROM Persons)
BEGIN
    INSERT INTO Persons (FirstName, LastName, Email, BirthDate, Height) VALUES
    ('Juan', 'Pérez', 'juan.perez@email.com', '1990-05-15', 175.5),
    ('María', 'González', 'maria.gonzalez@email.com', '1985-08-20', 168.0),
    ('Carlos', 'López', 'carlos.lopez@email.com', '1992-12-10', 180.2),
    ('Ana', 'Martínez', 'ana.martinez@email.com', '1988-03-25', 165.7);
END
GO

IF NOT EXISTS (SELECT * FROM Pets)
BEGIN
    INSERT INTO Pets (Name, Species, Breed, Color, Age) VALUES
    ('Max', 'Perro', 'Golden Retriever', 'Dorado', 3),
    ('Luna', 'Gato', 'Siamés', 'Blanco y Negro', 2),
    ('Rocky', 'Perro', 'Pastor Alemán', 'Marrón y Negro', 5),
    ('Mimi', 'Gato', 'Persa', 'Gris', 4),
    ('Buddy', 'Perro', 'Labrador', 'Negro', 1),
    ('Whiskers', 'Gato', 'Común Europeo', 'Atigrado', 6);
END
GO

PRINT 'Base de datos CrudApiDB inicializada correctamente con datos de ejemplo.';
