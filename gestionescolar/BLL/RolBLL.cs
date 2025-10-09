using gestionescolar.DLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace gestionescolar.BLL
{
    public class RolBLL
    {
        RolDLL rolDLL = new RolDLL();
        public DataTable ObtenerRoles()
        {
            return rolDLL.ObtenerRoles();
        }
    }
}