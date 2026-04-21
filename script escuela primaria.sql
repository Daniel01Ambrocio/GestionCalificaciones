create database escuelaBD
go  
use escuelaBD
go

CREATE TABLE Escuela (
    IDEscuela INT IDENTITY(1,1) PRIMARY KEY,
    NombreEscuela NVARCHAR(150) NOT NULL,
    ClaveInstitucion NVARCHAR(20) NOT NULL,
    Direccion NVARCHAR(250),
    Telefono NVARCHAR(20) NULL,
    Logotipo NVARCHAR(300) NULL,
    CicloEscolar NVARCHAR(20) NOT NULL
);

CREATE TABLE Rol (
    IDROL INT IDENTITY(1,1) PRIMARY KEY,
    nombreRol VARCHAR(15)
);

CREATE TABLE Grupo (
	IDGrupo int identity(1,1) primary key,
	grado int,
	grupo varchar(3),
	anio int
)
CREATE TABLE Estatus (
    IDStatus INT IDENTITY(1,1) PRIMARY KEY,
    descripcion VARCHAR(15)
);
select*from estatus

CREATE TABLE Usuario (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(50),
    ApellidoPaterno VARCHAR(50),
    ApellidoMaterno VARCHAR(50),
    usuario VARCHAR(50),
    contrasena VARCHAR(200),
    PeriodoIngreso DATE,
    PeriodoFin DATE,
    IDStatus int,
    IDROL INT,
    FOREIGN KEY (IDROL) REFERENCES rol(IDROL),
	FOREIGN KEY (IDStatus) REFERENCES estatus(IDStatus)
);
select*from Usuario

CREATE TABLE Director (
    Iddirector INT IDENTITY(1,1) PRIMARY KEY,
    IDUsuario INT,
	FOREIGN KEY (IDUsuario) REFERENCES usuario(IDUsuario)
);

CREATE TABLE Administrativo (
    IDAdministrativo INT IDENTITY(1,1) PRIMARY KEY,
	IDUsuario INT,
	FOREIGN KEY (IDUsuario) REFERENCES usuario(IDUsuario)
);


CREATE TABLE Maestro (
    IDMaestro INT IDENTITY(1,1) PRIMARY KEY,
    IDGrupo int,
    cedulaprofesional VARCHAR(30),
    IDUsuario INT,
	FOREIGN KEY (IDUsuario) REFERENCES usuario(IDUsuario),
	FOREIGN KEY (IDGrupo) REFERENCES grupo(IDGrupo)
);


CREATE TABLE Alumno (
    Matricula INT IDENTITY(1,1) PRIMARY KEY,
    IDGrupo int,
    IDUsuario INT,
	FOREIGN KEY (IDUsuario) REFERENCES usuario(IDUsuario),
	FOREIGN KEY (IDGrupo) REFERENCES grupo(IDGrupo)
);


CREATE TABLE Materia (
    IDMateria INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    GradoEscolar INT NOT NULL 
);

CREATE TABLE AlumnoMateria (
    IDAlumnoMateria INT IDENTITY(1,1) PRIMARY KEY,
    Matricula INT NOT NULL,  -- FK con Alumno
    IDMateria INT NOT NULL,  -- FK con Materia
    FOREIGN KEY (Matricula) REFERENCES Alumno(Matricula),
    FOREIGN KEY (IDMateria) REFERENCES Materia(IDMateria)
);


CREATE TABLE Calificacion (
    IDCalificacion INT IDENTITY(1,1) PRIMARY KEY,
    IDAlumnoMateria INT NOT NULL, -- Relación con AlumnoMateria
    Parcial1 DECIMAL(5,2) CHECK (Parcial1 BETWEEN 0 AND 10),
    Parcial2 DECIMAL(5,2) CHECK (Parcial2 BETWEEN 0 AND 10),
    Parcial3 DECIMAL(5,2) CHECK (Parcial3 BETWEEN 0 AND 10),
    Parcial4 DECIMAL(5,2) CHECK (Parcial4 BETWEEN 0 AND 10),
    Promedio DECIMAL(5,2),
    FOREIGN KEY (IDAlumnoMateria) REFERENCES AlumnoMateria(IDAlumnoMateria)
);

go
ALTER TABLE Calificacion
ADD CONSTRAINT UQ_Calificacion_IDAlumnoMateria UNIQUE (IDAlumnoMateria);

go
CREATE TABLE SolicitudBajas (
    IDSolicitudBajas INT IDENTITY(1,1) PRIMARY KEY,
    IDAdministrativo INT NOT NULL,
    IDDirectivo INT NOT NULL,
    IDUsuarioBaja INT NOT NULL,
    Descripcion VARCHAR(255),
    FechaSolicitud DATETIME,
    FechaAprobacion DATETIME,
    Estado VARCHAR(10),
    FOREIGN KEY (IDUsuarioBaja) REFERENCES Usuario(IDUsuario)
);
 
 
 go

-- Insertar roles: Alumno, Maestro, Administrativo, Director
INSERT INTO rol (nombreRol) VALUES ('Alumno'), ('Maestro'), ('Administrativo'), ('Director');
  
 go


-- Estatus como "Activo", "Inactivo", "Egresado", etc.
INSERT INTO estatus (descripcion) VALUES ('Activo'), ('Inactivo'), ('Egresado');
 
 go

INSERT INTO Usuario (
    Nombre,
    ApellidoPaterno,
    ApellidoMaterno,
    usuario,
    contrasena,
    PeriodoIngreso,
    PeriodoFin,
    IDStatus,
    IDROL
)
VALUES (
    'AdminNombre',
    'AdminApellidoP',
    'AdminApellidoM',
    'AD1',
    '0a801f0dd0190550ac0c90710f10c80120c60a605c08a0250e10f500e0f108f0d50330ea09302c00f00f09602b0310ce', -- Idealmente esta contraseña debería estar hasheada
    '2025-10-01',
    '2026-10-01', -- PeriodoFin NULL indica que sigue activo
    1,    -- Suponiendo que 1 es "Activo" en la tabla estatus
    3     -- IDROL para "Administrativo"
);
 
 go

INSERT INTO Administrativo (IDUsuario)
VALUES (1);
 
 go

--Aasdfg@1
UPDATE Usuario
SET Contrasena = '0a801f0dd0190550ac0c90710f10c80120c60a605c08a0250e10f500e0f108f0d50330ea09302c00f00f09602b0310ce';
 
 go

select*from Usuario
SELECT*FROM Alumno 
 go


INSERT INTO Escuela 
( 
    NombreEscuela,
    ClaveInstitucion,
    Direccion,
    Telefono,
    Logotipo,
    CicloEscolar
)
VALUES
( 
    'Escuela Primaria 1',
    'A123',
    'Calle 1',
    '555-1234',
    'logoEscuela',
    '2025-2026'
);
 
 go
select*from Estatus
select*from Usuario;
select*from SolicitudBajas
go


CREATE TRIGGER TR_InsertarAlumnoMateria
ON Materia
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @AnioActual INT;
    SET @AnioActual = YEAR(GETDATE());

    -- Tabla temporal para capturar IDs generados
    DECLARE @NuevasRelaciones TABLE (
        IDAlumnoMateria INT
    );

    -- Insert en AlumnoMateria capturando el ID generado
    INSERT INTO AlumnoMateria (Matricula, IDMateria)
    OUTPUT INSERTED.IDAlumnoMateria INTO @NuevasRelaciones
    SELECT 
        A.Matricula,
        I.IDMateria
    FROM inserted I
    INNER JOIN Grupo G 
        ON G.grado = I.GradoEscolar
       AND G.anio = @AnioActual
    INNER JOIN Alumno A 
        ON A.IDGrupo = G.IDGrupo
    WHERE NOT EXISTS (
        SELECT 1 
        FROM AlumnoMateria AM
        WHERE AM.Matricula = A.Matricula
          AND AM.IDMateria = I.IDMateria
    );

    -- Insert en Calificacion con valores en 0
    INSERT INTO Calificacion (IDAlumnoMateria, Parcial1, Parcial2, Parcial3, Parcial4, Promedio)
    SELECT 
        IDAlumnoMateria,
        0, 0, 0, 0, 0
    FROM @NuevasRelaciones;

END;
GO






CREATE TRIGGER TR_InsertarAlumnoMateriaDespuesDeAlumno
ON Alumno
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @AnioActual INT = YEAR(GETDATE());

    DECLARE @NuevasRelaciones TABLE (
        IDAlumnoMateria INT
    );

    INSERT INTO AlumnoMateria (Matricula, IDMateria)
    OUTPUT INSERTED.IDAlumnoMateria INTO @NuevasRelaciones
    SELECT 
        I.Matricula,
        M.IDMateria
    FROM inserted I
    INNER JOIN Grupo G 
        ON I.IDGrupo = G.IDGrupo
    INNER JOIN Materia M 
        ON M.GradoEscolar = G.Grado
    WHERE G.anio = @AnioActual
      AND NOT EXISTS (
          SELECT 1
          FROM AlumnoMateria AM
          WHERE AM.Matricula = I.Matricula
            AND AM.IDMateria = M.IDMateria
      );

    INSERT INTO Calificacion (IDAlumnoMateria, Parcial1, Parcial2, Parcial3, Parcial4, Promedio)
    SELECT IDAlumnoMateria, 0,0,0,0,0
    FROM @NuevasRelaciones;
END;
go



CREATE TRIGGER TR_InsertarAlumnoMateriaDespuesDeGrupo
ON Grupo
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NuevasRelaciones TABLE (
        IDAlumnoMateria INT
    );

    INSERT INTO AlumnoMateria (Matricula, IDMateria)
    OUTPUT INSERTED.IDAlumnoMateria INTO @NuevasRelaciones
    SELECT 
        A.Matricula,
        M.IDMateria
    FROM inserted I
    INNER JOIN Alumno A 
        ON A.IDGrupo = I.IDGrupo
    INNER JOIN Materia M 
        ON M.GradoEscolar = I.Grado
       AND M.GradoEscolar = I.Grado
    WHERE NOT EXISTS (
        SELECT 1
        FROM AlumnoMateria AM
        WHERE AM.Matricula = A.Matricula
          AND AM.IDMateria = M.IDMateria
    );

    INSERT INTO Calificacion (...)
    SELECT ...
END;

go 


 SELECT*FROM AlumnoMateria
 SELECT*FROM Maestro
 SELECT*FROM Calificacion