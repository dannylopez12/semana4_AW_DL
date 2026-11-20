IF DB_ID('clubes_deportivos') IS NULL
    CREATE DATABASE clubes_deportivos;
GO

USE clubes_deportivos;
GO

IF OBJECT_ID('Clubes', 'U') IS NULL
    CREATE TABLE Clubes (
        club_id INT IDENTITY(1,1) PRIMARY KEY,
        nombre NVARCHAR(100) NOT NULL,
        deporte NVARCHAR(50) NOT NULL,
        ubicacion NVARCHAR(150) NOT NULL,
        fecha_fundacion DATE NOT NULL
    );
GO

IF OBJECT_ID('Miembros', 'U') IS NULL
    CREATE TABLE Miembros (
        miembro_id INT IDENTITY(1,1) PRIMARY KEY,
        nombre NVARCHAR(60) NOT NULL,
        apellido NVARCHAR(60) NOT NULL,
        email NVARCHAR(100) NOT NULL UNIQUE,
        telefono VARCHAR(10) NOT NULL
    );
GO

-- Registros: inscripción de un miembro en un club
IF OBJECT_ID('Registros', 'U') IS NULL
    CREATE TABLE Registros (
        registro_id INT IDENTITY(1,1) PRIMARY KEY,
        club_id INT NOT NULL REFERENCES Clubes(club_id) ON DELETE CASCADE,
        miembro_id INT NOT NULL REFERENCES Miembros(miembro_id) ON DELETE CASCADE,
        fecha_registro DATE NOT NULL DEFAULT GETDATE(),
        UNIQUE (club_id, miembro_id)
    );
GO

IF NOT EXISTS (SELECT 1 FROM Clubes)
    INSERT INTO Clubes (nombre, deporte, ubicacion, fecha_fundacion) VALUES
    (N'Club Deportivo Los Halcones', N'Fútbol', N'Ambato, Tungurahua', '2005-03-12'),
    (N'Club de Natación Delfines', N'Natación', N'Quito, Pichincha', '2010-08-25'),
    (N'Club de Básquet Cóndores', N'Básquetbol', N'Guayaquil, Guayas', '2015-01-30');
GO

IF NOT EXISTS (SELECT 1 FROM Miembros)
    INSERT INTO Miembros (nombre, apellido, email, telefono) VALUES
    (N'María', N'Pérez', N'maria.perez@example.com', '0991111111'),
    (N'José', N'Núñez', N'jose.nunez@example.com', '0992222222'),
    (N'Ana', N'Torres', N'ana.torres@example.com', '0993333333'),
    (N'Luis', N'Gómez', N'luis.gomez@example.com', '0994444444');
GO

IF NOT EXISTS (SELECT 1 FROM Registros)
    INSERT INTO Registros (club_id, miembro_id, fecha_registro)
    SELECT c.club_id, m.miembro_id, v.fecha
    FROM (VALUES
        (N'Club Deportivo Los Halcones', N'maria.perez@example.com', '2026-01-10'),
        (N'Club Deportivo Los Halcones', N'jose.nunez@example.com', '2026-02-05'),
        (N'Club de Natación Delfines', N'ana.torres@example.com', '2026-03-18'),
        (N'Club de Básquet Cóndores', N'luis.gomez@example.com', '2026-04-02')
    ) AS v (club, email, fecha)
    JOIN Clubes c ON c.nombre = v.club
    JOIN Miembros m ON m.email = v.email;
GO
