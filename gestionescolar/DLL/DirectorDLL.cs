using gestionescolar.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace gestionescolar.DLL
{
    public class DirectorDLL
    {

        string connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;
        public DataTable ObtenerDirectores()
        {
            DataTable dtRoles = new DataTable();

            string query = @"SELECT 
                    u.Nombre, 
                    u.ApellidoPaterno, 
                    u.ApellidoMaterno, 
                    e.descripcion as Estatus, 
                    FORMAT(u.PeriodoIngreso, 'dd-MM-yyyy') AS PeriodoIngreso, 
                    FORMAT(u.PeriodoFin, 'dd-MM-yyyy') AS PeriodoFin
                FROM Director d
                INNER JOIN Usuario u ON d.IDUsuario = u.IDUsuario 
                INNER JOIN Estatus e ON u.IDStatus = e.IDStatus";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dtRoles);
            }

            return dtRoles;
        }
        public string RegistrarDirector(EntUsuario entUsuario)
        {
            try
            {
                int iddic = 0;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                INSERT INTO Director 
                (IDUsuario) 
                VALUES 
                (@IDUsuario);
                SELECT SCOPE_IDENTITY();";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@IDUsuario", entUsuario.IdUsuario);

                    conn.Open();
                    object result = cmd.ExecuteScalar(); // Devuelve el ID insertado

                    if (result != null)
                    {
                        int.TryParse(result.ToString(), out iddic);
                        return "DI" + iddic;
                    }
                    else
                    {
                        return "Error de registro";
                    }
                }

            }
            catch (SqlException ex)
            {
                return "Error de registro";
            }
        }
    }
}