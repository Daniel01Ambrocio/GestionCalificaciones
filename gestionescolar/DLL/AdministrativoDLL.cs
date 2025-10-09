using gestionescolar.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace gestionescolar.DLL
{
    public class AdministrativoDLL
    {
        string connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;
        public string RegistrarAdministrativo(Entadministrativo entadministrativo)
        {
            try
            {
                int idadmi = 0;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                INSERT INTO Administrativo 
                (Nombre, ApellidoPaterno, ApellidoMaterno, contrasenia, PeriodoIngreso, PeriodoFin, IDStatus, IDRol) 
                VALUES 
                (@Nombre, @ApellidoPaterno, @ApellidoMaterno, @contrasenia, @PeriodoIngreso, @PeriodoFin, @status, @rol);
                SELECT SCOPE_IDENTITY();";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Nombre", entadministrativo.Nombre);
                    cmd.Parameters.AddWithValue("@ApellidoPaterno", entadministrativo.ApellidoPaterno);
                    cmd.Parameters.AddWithValue("@ApellidoMaterno", entadministrativo.ApellidoMaterno);
                    cmd.Parameters.AddWithValue("@contrasenia", entadministrativo.contrasena);
                    cmd.Parameters.AddWithValue("@PeriodoIngreso", entadministrativo.PeriodoIngreso);
                    cmd.Parameters.AddWithValue("@PeriodoFin", entadministrativo.PeriodoFin);
                    cmd.Parameters.AddWithValue("@status", entadministrativo.IDStatus);
                    cmd.Parameters.AddWithValue("@rol", entadministrativo.IDRol);

                    conn.Open();
                    object result = cmd.ExecuteScalar(); // Devuelve el ID insertado

                    if (result != null)
                    {
                        int.TryParse(result.ToString(), out idadmi);
                    }
                }

                if (idadmi > 0)
                {
                    ActualizaNombreAdmi(idadmi);
                }

                return "Registro exitoso.";
            }
            catch (SqlException ex)
            {
                return $"Error al registrar administrativo (SQL): {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"Error inesperado: {ex.Message}";
            }
        }

        private void ActualizaNombreAdmi(int idAdministrativo)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string nuevoUsuario = "AD" + idAdministrativo;

                string query = "UPDATE Administrativo SET usuario = @usuario WHERE IDAdministrativo = @id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@usuario", nuevoUsuario);
                cmd.Parameters.AddWithValue("@id", idAdministrativo);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}