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

                // Tabla, nombre del campo ID, nombre del campo de contraseña
                var tablas = new[]
                {
            new { Tabla = "director", IdCampo = "IDdirector", CampoContrasena = "contrasena" },
            new { Tabla = "Administrativo", IdCampo = "IDAdministrativo", CampoContrasena = "contrasenia" },
            new { Tabla = "Maestro", IdCampo = "IDMaestro", CampoContrasena = "contrasenia" },
            new { Tabla = "Alumno", IdCampo = "Matricula", CampoContrasena = "contrasenia" }
        };

                foreach (var t in tablas)
                {
                    string query = $@"
                SELECT 
                    u.{t.IdCampo} AS ID,
                    e.descrípcion AS StatusDescripcion,
                    r.nombreRol AS NombreRol
                FROM {t.Tabla} u
                INNER JOIN estatus e ON u.IDStatus = e.IDStatus
                INNER JOIN rol r ON u.IDROL = r.IDROL
                WHERE u.usuario = @usuario AND u.{t.CampoContrasena} = @contrasena";

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
                                string tipoUsuario = t.Tabla; // director, Administrativo, etc.

                                return (idUsuario, descripcionStatus, nombreRol, tipoUsuario);
                            }
                        }
                    }
                }
            }

            // No se encontró el usuario en ninguna tabla
            return null;
        }

    }
}