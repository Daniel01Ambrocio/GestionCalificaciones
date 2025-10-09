using gestionescolar.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace gestionescolar.DLL
{
    public class RolDLL
    {
        string connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

        public DataTable ObtenerRoles()
        {
            DataTable dtRoles = new DataTable();

            string query = "SELECT IDROL, nombreRol FROM Rol";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dtRoles);
            }

            return dtRoles;
        }
    }
}