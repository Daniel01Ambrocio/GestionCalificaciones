create database escuelaBD

use escuelaBD

CREATE TABLE rol (
    IDROL INT IDENTITY(1,1) PRIMARY KEY,
    nombreRol VARCHAR(15)
);

CREATE TABLE grupo (
	IDGrupo int identity(1,1) primary key,
	grado int,
	Grupo varchar(3),
	anio varchar(5)
)

CREATE TABLE estatus (
    IDStatus INT IDENTITY(1,1) PRIMARY KEY,
    descrípcion VARCHAR(15)
);

CREATE TABLE director (
    Iddirector INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(50),
    ApellidoPaterno VARCHAR(50),
    ApellidoMaterno VARCHAR(50),
    usuario VARCHAR(50),
    contrasena VARCHAR(100),
    PeriodoIngreso DATE,
    PeriodoFin DATE,
    IDStatus int,
    IDROL INT,
    FOREIGN KEY (IDROL) REFERENCES rol(IDROL),
	FOREIGN KEY (IDStatus) REFERENCES estatus(IDStatus)
);

CREATE TABLE Administrativo (
    IDAdministrativo INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(50),
    ApellidoPaterno VARCHAR(50),
    ApellidoMaterno VARCHAR(50),
    usuario VARCHAR(50),
    contrasenia VARCHAR(100),
    PeriodoIngreso DATE,
    PeriodoFin DATE,
    IDStatus int,
    IDROL INT,
    FOREIGN KEY (IDROL) REFERENCES rol(IDROL),
	FOREIGN KEY (IDStatus) REFERENCES estatus(IDStatus)
);


CREATE TABLE Maestro (
    IDMaestro INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(50),
    ApellidoPaterno VARCHAR(50),
    ApellidoMaterno VARCHAR(50),
    grupo int,
    cedulaprofesional VARCHAR(30),
    usuario VARCHAR(50),
    contrasenia VARCHAR(100),
    PeriodoIngreso DATE,
    PeriodoFin DATE,
    IDStatus int,
    IDROL INT,
    FOREIGN KEY (IDROL) REFERENCES rol(IDROL),
	FOREIGN KEY (IDStatus) REFERENCES estatus(IDStatus),
	FOREIGN KEY (grupo) REFERENCES grupo(IDGrupo)
);


CREATE TABLE Alumno (
    Matricula INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(50),
    ApellidoPaterno VARCHAR(50),
    ApellidoMaterno VARCHAR(50),
    grupo int,
    usuario VARCHAR(50),
    contrasenia VARCHAR(100),
    PeriodoIngreso DATE,
    PeriodoFin DATE,
   IDStatus int,
    IDROL INT,
    FOREIGN KEY (IDROL) REFERENCES rol(IDROL),
	FOREIGN KEY (IDStatus) REFERENCES estatus(IDStatus),
	FOREIGN KEY (grupo) REFERENCES grupo(IDGrupo)
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
    Promedio AS ((Parcial1 + Parcial2 + Parcial3 + Parcial4) / 4.0) PERSISTED,
    FOREIGN KEY (IDAlumnoMateria) REFERENCES AlumnoMateria(IDAlumnoMateria)
);

-- Insertar roles: Alumno, Maestro, Administrativo, Director
INSERT INTO rol (nombreRol) VALUES ('Alumno'), ('Maestro'), ('Administrativo'), ('Director');

-- Insertar grupos
INSERT INTO grupo (grado, Grupo, anio) VALUES
(1, 'A', '2023'),
(2, 'B', '2023'),
(3, 'C', '2023');

-- Estatus como "Activo", "Inactivo", "Egresado", etc.
INSERT INTO estatus (descrípcion) VALUES ('Activo'), ('Inactivo'), ('Egresado');

INSERT INTO director (Nombre, ApellidoPaterno, ApellidoMaterno, usuario, contrasena, PeriodoIngreso, PeriodoFin, IDStatus, IDRol)
VALUES ('Luis', 'Martínez', 'Gómez', 'lmartinez', '1234pass', '2023-08-01', NULL, 1, 4);

INSERT INTO Administrativo (Nombre, ApellidoPaterno, ApellidoMaterno, usuario, contrasenia, PeriodoIngreso, PeriodoFin, IDStatus, IDRol)
VALUES ('Ana', 'Lopez', 'Santos', 'alopez', 'admin123', '2023-08-01', NULL, 1, 3);

INSERT INTO Maestro (Nombre, ApellidoPaterno, ApellidoMaterno, grupo, cedulaprofesional, usuario, contrasenia, PeriodoIngreso, PeriodoFin, IDStatus, IDRol)
VALUES ('Carlos', 'Ramírez', 'López', 1, '12345678', 'cramirez', 'maestro123', '2023-08-01', NULL, 1, 2);

INSERT INTO Alumno (Nombre, ApellidoPaterno, ApellidoMaterno, grupo, usuario, contrasenia, PeriodoIngreso, PeriodoFin, IDStatus, IDRol)
VALUES ('María', 'Hernández', 'Díaz', 1, 'mhernandez', 'alumno123', '2023-08-01', NULL, 1, 1);  -- status = 1 (Activo)

INSERT INTO Materia (Nombre, GradoEscolar) VALUES
('Matemáticas', 1),
('Historia', 1),
('Español', 1),
('Ciencias', 2);

-- Primero buscamos los IDs de las materias del grado 1 (puedes automatizar esto en una app)
-- Luego los asignamos:
INSERT INTO AlumnoMateria (Matricula, IDMateria)
VALUES
(1, 1),  -- Matemáticas
(1, 2);  -- Historia


-- Para Matemáticas (IDAlumnoMateria = 1)
INSERT INTO Calificacion (IDAlumnoMateria, Parcial1, Parcial2, Parcial3, Parcial4)
VALUES (1, 8.5, 9.0, 8.0, 9.5);

-- Para Historia (IDAlumnoMateria = 2)
INSERT INTO Calificacion (IDAlumnoMateria, Parcial1, Parcial2, Parcial3, Parcial4)
VALUES (2, 7.5, 8.0, 9.0, 8.5);



--Consulta: Promedio de cada materia de un alumno por Matricula
SELECT 
    a.Matricula,
    a.Nombre AS NombreAlumno,
    a.ApellidoPaterno,
    a.ApellidoMaterno,
    m.Nombre AS NombreMateria,
    c.Promedio AS PromedioMateria
FROM Alumno a
JOIN AlumnoMateria am ON a.Matricula = am.Matricula
JOIN Materia m ON am.IDMateria = m.IDMateria
JOIN Calificacion c ON am.IDAlumnoMateria = c.IDAlumnoMateria
WHERE a.Matricula = 1  -- Cambia este número por la matrícula del alumno deseado
ORDER BY m.Nombre;


use escuelaBD

select *from Administrativo 