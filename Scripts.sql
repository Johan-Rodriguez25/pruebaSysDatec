CREATE DATABASE Prueba_SysDatec
USE Prueba_SysDatec

CREATE TABLE Estudiantes (
    Estudiante_id INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Nombre VARCHAR(100)
)

CREATE TABLE Profesores (
    Profesor_id INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Nombre_profesor VARCHAR(100)
)

CREATE TABLE Clases (
    Clase_id INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Nombre_clase VARCHAR(100),
    Profesor_id INT FOREIGN KEY REFERENCES Profesores(Profesor_id)
)

CREATE TABLE RegistroClases (
    Registro_id INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Estudiante_id INT FOREIGN KEY REFERENCES Estudiantes(Estudiante_id),
    Clase_id INT FOREIGN KEY REFERENCES Clases(Clase_id)
)

// Stored procedure de estudiantes

CREATE PROCEDURE ObtenerEstudiantes
    AS BEGIN
    SELECT * FROM Estudiantes
    END

CREATE PROCEDURE ObtenerEstudiantePorId
    @Estudiante_id INT
    AS BEGIN
    SET NOCOUNT ON
    
    SELECT Estudiante_id, Nombre
    FROM Estudiantes
    WHERE Estudiante_id = @Estudiante_id
    END

CREATE PROCEDURE InsertarEstudiante
    @Nombre VARCHAR(100)
    AS BEGIN
    INSERT INTO Estudiantes VALUES (@Nombre)
    END

CREATE PROCEDURE ActualizarEstudiante
    @Estudiante_id INT,
    @Nombre VARCHAR(100)
    AS BEGIN
    UPDATE Estudiantes SET Nombre = @Nombre WHERE Estudiante_id = @Estudiante_id
    END

CREATE PROCEDURE EliminarEstudiante
    @Estudiante_id INT
    AS BEGIN
    DELETE FROM Estudiantes WHERE Estudiante_id = @Estudiante_id
    END

// Stored procedure de profesores

CREATE PROCEDURE ObtenerProfesores
    AS BEGIN
    SELECT * FROM Profesores
    END

CREATE PROCEDURE ObtenerProfesorPorId
    @Profesor_id INT
    AS BEGIN
    SET NOCOUNT ON
    
    SELECT Profesor_id, Nombre_profesor
    FROM Profesores
    WHERE Profesor_id = @Profesor_id
    END

CREATE PROCEDURE InsertarProfesor
    @Nombre_profesor VARCHAR(100)
    AS BEGIN
    INSERT INTO Profesores VALUES (@Nombre_profesor)
    END

CREATE PROCEDURE ActualizarProfesor
    @Profesor_id INT,
    @Nombre_profesor VARCHAR(100)
    AS BEGIN
    UPDATE Profesores SET Nombre_profesor = @Nombre_profesor WHERE Profesor_id = @Profesor_id
    END

CREATE PROCEDURE EliminarProfesor
    @Profesor_id INT
    AS BEGIN
    DELETE FROM Profesores WHERE Profesor_id = @Profesor_id
    END

// Stored procedure de clases

CREATE PROCEDURE ObtenerClases
    AS BEGIN
    SELECT * FROM Clases
    END

CREATE PROCEDURE ObtenerClasePorId
    @Clase_id INT
    AS BEGIN
    SET NOCOUNT ON
    
    SELECT Clase_id, Nombre_clase, Profesor_id
    FROM Clases
    WHERE Clase_id = @Clase_id
    END

CREATE PROCEDURE InsertarClase
    @Nombre_clase VARCHAR(100),
	@Profesor_id INT
    AS BEGIN
    INSERT INTO Clases VALUES (@Nombre_clase, @Profesor_id)
    END

CREATE PROCEDURE ActualizarClase
    @Clase_id INT,
    @Nombre_clase VARCHAR(100),
	@Profesor_id INT
    AS BEGIN
    UPDATE Clases SET Nombre_clase = @Nombre_clase WHERE Clase_id = @Clase_id
    END

CREATE PROCEDURE EliminarClase
    @Clase_id INT
    AS BEGIN
    DELETE FROM Clases WHERE Clase_id = @Clase_id
    END

// Stored procedure de asignar clase a estudiante

CREATE PROCEDURE ObtenerRegistroClases
    AS BEGIN
    SELECT * FROM RegistroClases
    END

CREATE PROCEDURE AsignarClaseAEstudiante 
    @Estudiante_id INT,
    @Clase_id INT
	AS BEGIN
    INSERT INTO RegistroClases VALUES (@Estudiante_id, @Clase_id)
	END

CREATE PROCEDURE ActualizarRegistroClase
    @Registro_id INT,
    @Estudiante_id INT,
	@Clase_id INT
    AS BEGIN
    UPDATE RegistroClases SET Estudiante_id = @Estudiante_id, Clase_id = @Clase_id
	WHERE Registro_id = @Registro_id
    END

CREATE PROCEDURE EliminarRegistroClase
    @Registro_id INT
    AS BEGIN
    DELETE FROM RegistroClases WHERE Registro_id = @Registro_id
    END

CREATE PROCEDURE ObtenerCantidadEstudiantesEnClase
    @Clase_id INT
	AS BEGIN
    SELECT COUNT(*) FROM RegistroClaseS WHERE Clase_id = @Clase_id
	END

CREATE PROCEDURE VerificarEstudianteEnClase
    @Estudiante_id INT,
    @Clase_id INT
	AS BEGIN
    IF EXISTS (SELECT 1 FROM RegistroClases WHERE Estudiante_id = @Estudiante_id AND Clase_id = @Clase_id)
        SELECT 1
    ELSE
        SELECT 0
	END