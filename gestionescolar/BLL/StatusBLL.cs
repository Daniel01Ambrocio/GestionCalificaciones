using gestionescolar.DLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace gestionescolar.BLL
{
    public class StatusBLL
    {

        StatusDLL statusDLL = new StatusDLL();
        public DataTable ObtenerStatus()
        {
            return statusDLL.ObtenerStatus();
        }
    }
}