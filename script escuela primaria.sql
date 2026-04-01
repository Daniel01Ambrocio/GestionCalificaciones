create database escuelaBD

use escuelaBD
 

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
	anio varchar(5)
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

select*from ROL;

