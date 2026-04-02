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
    public class SolicitudBajaDLL
    {
        string connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;
        public string RegistrarSolicitud(EntSolicitudBajas entSolicitudBajas)
        {
            try
            {
                int idGenerado = 0;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
            INSERT INTO SolicitudBajas 
            (IDAdministrativo, IDDirectivo, IDUsuarioBaja, Descripcion, FechaSolicitud, Estado) 
            VALUES 
            (@IDAdministrativo, @IDDirectivo, @IDUsuarioBaja, @Descripcion, @FechaSolicitud, @Estado);
            SELECT SCOPE_IDENTITY();";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IDAdministrativo", entSolicitudBajas.IDAdministrativo);
                        cmd.Parameters.AddWithValue("@IDDirectivo", entSolicitudBajas.IDDirectivo);
                        cmd.Parameters.AddWithValue("@IDUsuarioBaja", entSolicitudBajas.IDUsuarioBaja);
                        cmd.Parameters.AddWithValue("@Descripcion", entSolicitudBajas.Descripcion);
                        cmd.Parameters.AddWithValue("@FechaSolicitud", entSolicitudBajas.FechaSolicitud);
                        cmd.Parameters.AddWithValue("@Estado", entSolicitudBajas.Estado);

                        conn.Open();
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            int.TryParse(result.ToString(), out idGenerado);
                        }
                    }
                }

                if (idGenerado > 0)
                {
                    return "Registro correcto.";
                }
                else
                {
                    return "No se pudo registrar la solicitud. Intentelo más tarde.";
                }
            }
            catch (SqlException ex)
            {
                return "Error en el sistema. Intentelo más tarde.";
            }
            catch (Exception ex)
            {
                return "Error general: " + ex.Message;
            }
        }
        public DataTable MostrarSolicitudes(int IdAdministrativo)
        {
            DataTable dtsolicitudes = new DataTable();

            string query = "SELECT sb.IDSolicitudBajas, " +
                   "CONCAT(au.Nombre, ' ', au.ApellidoPaterno, ' ', au.ApellidoMaterno) AS NombAdministrativo, " +
                   "CONCAT(u.Nombre, ' ', u.ApellidoPaterno, ' ', u.ApellidoMaterno) AS NombUsuarioBaja, " +
                   "sb.Descripcion, " +
                   "CONCAT(du.Nombre, ' ', du.ApellidoPaterno, ' ', du.ApellidoMaterno) AS NombDirectivo, " +
                   "CONVERT(VARCHAR(10), sb.FechaSolicitud, 103) AS FechaSolicitud, " +  // dd/MM/yyyy
                   "CONVERT(VARCHAR(10), sb.FechaAprobacion, 103) AS FechaAprobacion, " + // dd/MM/yyyy
                   "sb.Estado " +
                   "FROM SolicitudBajas sb " +
                   "INNER JOIN Administrativo a ON sb.IDAdministrativo = a.IDAdministrativo " +
                   "INNER JOIN Usuario au ON a.IDUsuario = au.IdUsuario " +
                   "INNER JOIN Usuario u ON sb.IDUsuarioBaja = u.IdUsuario " +
                   "INNER JOIN Director d ON sb.IDDirectivo = d.IdDirector " +
                   "INNER JOIN Usuario du ON d.IDUsuario = du.IdUsuario " +
                   "WHERE sb.IDAdministrativo = @IdAdministrativo";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {

                cmd.Parameters.Add("@IdAdministrativo", SqlDbType.Int).Value = IdAdministrativo;

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dtsolicitudes);
                }
            }

            return dtsolicitudes;
        }
        public DataTable MostrarSolicitudesPendientes()
        {
            DataTable dtsolicitudes = new DataTable();

            string query = "SELECT sb.IDSolicitudBajas, " +
                   "CONCAT(au.Nombre, ' ', au.ApellidoPaterno, ' ', au.ApellidoMaterno) AS NombAdministrativo, " +
                   "CONCAT(u.Nombre, ' ', u.ApellidoPaterno, ' ', u.ApellidoMaterno) AS NombUsuarioBaja, " +
                   "sb.Descripcion, " +
                   "CONCAT(du.Nombre, ' ', du.ApellidoPaterno, ' ', du.ApellidoMaterno) AS NombDirectivo, " +
                   "CONVERT(VARCHAR(10), sb.FechaSolicitud, 103) AS FechaSolicitud, " +  // dd/MM/yyyy
                   "CONVERT(VARCHAR(10), sb.FechaAprobacion, 103) AS FechaAprobacion, " + // dd/MM/yyyy
                   "sb.Estado " +
                   "FROM SolicitudBajas sb " +
                   "INNER JOIN Administrativo a ON sb.IDAdministrativo = a.IDAdministrativo " +
                   "INNER JOIN Usuario au ON a.IDUsuario = au.IdUsuario " +
                   "INNER JOIN Usuario u ON sb.IDUsuarioBaja = u.IdUsuario " +
                   "INNER JOIN Director d ON sb.IDDirectivo = d.IdDirector " +
                   "INNER JOIN Usuario du ON d.IDUsuario = du.IdUsuario " +
                   "WHERE sb.Estado = 'Pendiente'";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dtsolicitudes);
                }
            }

            return dtsolicitudes;
        }
        public DataTable MostrarSolicitudesAprobadas()
        {
            DataTable dtsolicitudes = new DataTable();

            string query = "SELECT sb.IDSolicitudBajas, " +
                   "CONCAT(au.Nombre, ' ', au.ApellidoPaterno, ' ', au.ApellidoMaterno) AS NombAdministrativo, " +
                   "CONCAT(u.Nombre, ' ', u.ApellidoPaterno, ' ', u.ApellidoMaterno) AS NombUsuarioBaja, " +
                   "sb.Descripcion, " +
                   "CONCAT(du.Nombre, ' ', du.ApellidoPaterno, ' ', du.ApellidoMaterno) AS NombDirectivo, " +
                   "CONVERT(VARCHAR(10), sb.FechaSolicitud, 103) AS FechaSolicitud, " +  // dd/MM/yyyy
                   "CONVERT(VARCHAR(10), sb.FechaAprobacion, 103) AS FechaAprobacion, " + // dd/MM/yyyy
                   "sb.Estado " +
                   "FROM SolicitudBajas sb " +
                   "INNER JOIN Administrativo a ON sb.IDAdministrativo = a.IDAdministrativo " +
                   "INNER JOIN Usuario au ON a.IDUsuario = au.IdUsuario " +
                   "INNER JOIN Usuario u ON sb.IDUsuarioBaja = u.IdUsuario " +
                   "INNER JOIN Director d ON sb.IDDirectivo = d.IdDirector " +
                   "INNER JOIN Usuario du ON d.IDUsuario = du.IdUsuario " +
                   "WHERE sb.Estado = 'Aprobado'";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dtsolicitudes);
                }
            }

            return dtsolicitudes;
        }
        public string AprobarSolicitud(int IDSolicitudBajas)
        {
            try
            {
                int idGenerado = 0;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        UPDATE SolicitudBajas
                        SET Estado = 'Aprobado',
                            FechaAprobacion = GETDATE()
                        WHERE IDSolicitudBajas = @IDSolicitudBajas;

                        SELECT @@ROWCOUNT AS FilasAfectadas;";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IDSolicitudBajas", IDSolicitudBajas);
                        
                        conn.Open();
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            int.TryParse(result.ToString(), out idGenerado);
                        }
                    }
                }

                if (idGenerado > 0)
                {
                    return "Actualización correcta.";
                }
                else
                {
                    return "No se pudo aprobar la solicitud. Intentelo más tarde.";
                }
            }
            catch (SqlException ex)
            {
                return "Error en el sistema. Intentelo más tarde.";
            }
            catch (Exception ex)
            {
                return "Error general: " + ex.Message;
            }
        }
    }
}