using gestionescolar.DLL;
using gestionescolar.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace gestionescolar.BLL
{
    public class SolicitudBajaBLL
    {
        SolicitudBajaDLL solicitudbajadll = new SolicitudBajaDLL();

        public string RegistrarSolicitud(EntSolicitudBajas entsolicitudBajas)
        {
            return solicitudbajadll.RegistrarSolicitud(entsolicitudBajas);
        }
        public DataTable MostrarSolicitudes(int IdAdministrativo)
        {
            return solicitudbajadll.MostrarSolicitudes(IdAdministrativo);
        }
    }
}