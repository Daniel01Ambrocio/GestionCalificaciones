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
go
ALTER TABLE AlumnoMateria
ADD CONSTRAINT UQ_AlumnoMateria UNIQUE (Matricula, IDMateria);
go

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

--triguer para cuando se inserta una nueva materia
 CREATE TRIGGER TR_Materia_Insert
ON Materia
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO AlumnoMateria (Matricula, IDMateria)
    SELECT 
        a.Matricula,
        i.IDMateria
    FROM inserted i
    INNER JOIN Grupo g ON g.grado = i.GradoEscolar
    INNER JOIN Alumno a ON a.IDGrupo = g.IDGrupo
    WHERE g.anio = YEAR(GETDATE())
    AND NOT EXISTS (
        SELECT 1
        FROM AlumnoMateria am
        WHERE am.Matricula = a.Matricula
        AND am.IDMateria = i.IDMateria
    );
END;

go

--triguer para cuando se inserta un nuevo alumno
CREATE TRIGGER TR_Alumno_InsertUpdate
ON Alumno
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO AlumnoMateria (Matricula, IDMateria)
    SELECT 
        i.Matricula,
        m.IDMateria
    FROM inserted i
    INNER JOIN Grupo g ON i.IDGrupo = g.IDGrupo
    INNER JOIN Materia m ON m.GradoEscolar = g.grado
    WHERE g.anio = YEAR(GETDATE())
    AND NOT EXISTS (
        SELECT 1
        FROM AlumnoMateria am
        WHERE am.Matricula = i.Matricula
        AND am.IDMateria = m.IDMateria
    );
END;

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
    '2029-10-01', 
    1, 
    3  
);
 
 go

INSERT INTO Administrativo (IDUsuario)
VALUES (1);
 
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
    'Escuela Primaria Lucila',
    'A123',
    'Calle 1',
    '555-1234',
    'logoEscuela',
    '2025-2026'
);
  
go 

 --insertamos grupos

INSERT INTO Grupo(grado, grupo, anio) VALUES (1, 'A', 2025);
INSERT INTO Grupo(grado, grupo, anio) VALUES (1, 'A', 2026);
INSERT INTO Grupo(grado, grupo, anio) VALUES (1, 'B', 2026);
INSERT INTO Grupo(grado, grupo, anio) VALUES (1, 'C', 2026);
go
--INSERTAMOS MATERIAS
INSERT INTO Materia(Nombre, GradoEscolar) VALUES ('Español', 1);
INSERT INTO Materia(Nombre, GradoEscolar) VALUES ('Español', 2);
INSERT INTO Materia(Nombre, GradoEscolar) VALUES ('Español', 3);
INSERT INTO Materia(Nombre, GradoEscolar) VALUES ('Historia', 1);

 go
-- usuario de Alumnos
INSERT INTO Usuario (Nombre, ApellidoPaterno, ApellidoMaterno, usuario, contrasena, PeriodoIngreso, PeriodoFin, IDStatus, IDROL)
VALUES  
('Juan', 'Perez', 'Lopez', 'AL1', 'AL1', '2024-01-01', '2030-01-01', 1, 1),
('Maria', 'Gomez', 'Hernandez', 'AL2', 'AL2', '2024-01-01', '2030-01-01', 1, 1),
('Luis', 'Ramirez', 'Torres', 'AL3', 'AL3', '2024-01-01', '2030-01-01', 1, 1),
('Ana', 'Martinez', 'Diaz', 'AL4', 'AL4', '2024-01-01', '2030-01-01', 1, 1);

--alumnos
INSERT INTO Alumno(IDGrupo, IDUsuario)
VALUES 
(1,2),
(2,3),
(3,4),
(3,5);
go

-- usuario de Maestros
INSERT INTO Usuario (Nombre, ApellidoPaterno, ApellidoMaterno, usuario, contrasena, PeriodoIngreso, PeriodoFin, IDStatus, IDROL)
VALUES
('Carlos', 'Sanchez', 'Ruiz', 'MA1', 'MA1', '2024-01-01', '2030-01-01', 1, 2),
('Laura', 'Fernandez', 'Castro', 'MA2', 'MA2', '2024-01-01', '2030-01-01', 1, 2);

go 
--maestro
INSERT INTO Maestro(IDGrupo,cedulaprofesional, IDUsuario)
VALUES 
(2,'dfbgdb',6),
(3,'dfgdfgdthgnb',7);


go
 -- usuario de directores
INSERT INTO Usuario (Nombre, ApellidoPaterno, ApellidoMaterno, usuario, contrasena, PeriodoIngreso, PeriodoFin, IDStatus, IDROL)
VALUES 
('Daniel', 'Ambrocio', 'Reyes', 'DI1', 'DI1', '2024-01-01', '2030-01-01', 1, 1);
go
--directores
INSERT INTO Director(IDUsuario)
VALUES (8);

go

--Aasdfg@1
UPDATE Usuario
SET Contrasena = '0a801f0dd0190550ac0c90710f10c80120c60a605c08a0250e10f500e0f108f0d50330ea09302c00f00f09602b0310ce';
go

select*from Usuario