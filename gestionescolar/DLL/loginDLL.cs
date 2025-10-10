using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace gestionescolar.DLL
{
    public class loginDLL
    {
        public (int idUsuario, string descripcionStatus, string nombreRol, string tipoUsuario)? InicioSesion(string user, string pass)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Paso 1: Buscar en la tabla Usuario
                string query = @"
                    SELECT 
                        u.IdUsuario,
                        e.descripcion AS StatusDescripcion,
                        r.nombreRol AS NombreRol
                    FROM Usuario u
                    INNER JOIN Estatus e ON u.IDStatus = e.IDStatus
                    INNER JOIN Rol r ON u.IDROL = r.IDROL
                    WHERE u.usuario = @usuario AND u.contrasena = @contrasena";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@usuario", user);
                    cmd.Parameters.AddWithValue("@contrasena", pass);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int idUsuario = reader.GetInt32(0);
                            string descripcionStatus = reader.GetString(1);
                            string nombreRol = reader.GetString(2);

                            reader.Close(); // Cerrar antes de la siguiente consulta

                            // Paso 2: Determinar el tipo de usuario
                            string tipoQuery = @"
                SELECT TOP 1 TipoUsuario FROM (
                    SELECT 'Director' AS TipoUsuario WHERE EXISTS (SELECT 1 FROM Director WHERE IDUsuario = @idUsuario)
                    UNION
                    SELECT 'Administrativo' WHERE EXISTS (SELECT 1 FROM Administrativo WHERE IDUsuario = @idUsuario)
                    UNION
                    SELECT 'Maestro' WHERE EXISTS (SELECT 1 FROM Maestro WHERE IDUsuario = @idUsuario)
                    UNION
                    SELECT 'Alumno' WHERE EXISTS (SELECT 1 FROM Alumno WHERE IDUsuario = @idUsuario)
                ) AS Tipos";

                            using (SqlCommand tipoCmd = new SqlCommand(tipoQuery, conn))
                            {
                                tipoCmd.Parameters.AddWithValue("@idUsuario", idUsuario);

                                using (SqlDataReader tipoReader = tipoCmd.ExecuteReader())
                                {
                                    if (tipoReader.Read())
                                    {
                                        string tipoUsuario = tipoReader.GetString(0);
                                        return (idUsuario, descripcionStatus, nombreRol, tipoUsuario);
                                    }
                                    else
                                    {
                                        // No pertenece a ninguna tabla de tipo de usuario
                                        return (idUsuario, descripcionStatus, nombreRol, "Desconocido");
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Si no se encontró el usuario
            return null;

        }

    }
}