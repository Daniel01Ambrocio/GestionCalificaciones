using gestionescolar.DLL;
using gestionescolar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gestionescolar.BLL
{
    public class AdministrativoBLL
    {
        AdministrativoDLL administrativoDLL = new AdministrativoDLL();
        public string RegistrarAdministrativo(Entadministrativo entadministrativo)
        {

            return administrativoDLL.RegistrarAdministrativo(entadministrativo);
        }
    }
}