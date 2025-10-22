create database escuelaBD

use escuelaBD

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
    Promedio Decimal,
    FOREIGN KEY (IDAlumnoMateria) REFERENCES AlumnoMateria(IDAlumnoMateria)
);


  SELECT 
        m.Nombre, 
        c.Parcial1,
		c.Parcial2,
		c.Parcial3,
		c.Parcial4
    FROM alumno al
    INNER JOIN AlumnoMateria am ON al.Matricula = am.Matricula
    INNER JOIN Calificacion c ON c.IDAlumnoMateria = am.IDAlumnoMateria
	inner join materia m on m.IDMateria = am.IDMateria
	where al.IDUsuario = 14


-- Insertar roles: Alumno, Maestro, Administrativo, Director
INSERT INTO rol (nombreRol) VALUES ('Alumno'), ('Maestro'), ('Administrativo'), ('Director');

-- Insertar grupos
INSERT INTO grupo (grado, Grupo, anio) VALUES
(1, 'A', '2023'),
(2, 'B', '2023'),
(3, 'C', '2023');

-- Estatus como "Activo", "Inactivo", "Egresado", etc.
INSERT INTO estatus (descripcion) VALUES ('Activo'), ('Inactivo'), ('Egresado');

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
    'AD2',
    '00d0c504f03e06a06d03e0540fe08304001c0c306f0e006b0730ca05c0730320390e308b02b08901008b006051019048', -- Idealmente esta contraseña debería estar hasheada
    '2025-10-01',
    '2026-10-01', -- PeriodoFin NULL indica que sigue activo
    1,    -- Suponiendo que 1 es "Activo" en la tabla estatus
    3     -- IDROL para "Administrativo"
);

INSERT INTO Administrativo (IDUsuario)
VALUES (2);

select*from Director
select*from materia
select*from Usuario
select*from Estatus

SELECT 
    u.IdUsuario,
    e.descripcion AS StatusDescripcion,
    r.nombreRol AS NombreRol
FROM Usuario u
INNER JOIN Estatus e ON u.IDStatus = e.IDStatus
INNER JOIN Rol r ON u.IDROL = r.IDROL
WHERE u.usuario = 'AD1' 
  AND u.contrasena = '0d50be08600f0270ec0f109604f0910ef09e0400d104402d0870510ce0a905d0d80f805100d07300e08a0b80fe0170c3';

  select*from Alumno

  SELECT 
        u.Nombre, 
        u.ApellidoPaterno, 
        u.ApellidoMaterno, 
        (CAST(g.grado AS VARCHAR) + '-' + g.grupo + '-' + g.anio) AS Grupo,
        m.cedulaprofesional,
        e.descripcion as Estatus, 
        u.PeriodoIngreso, 
        u.PeriodoFin
    FROM Maestro m
    INNER JOIN Usuario u ON m.IDUsuario = u.IDUsuario
    INNER JOIN Grupo g ON m.IDGrupo = g.IDGrupo
    INNER JOIN Estatus e ON u.IDStatus = e.IDStatus

	select*from Grupo


	DELETE FROM Grupo WHERE IDGrupo = 16;