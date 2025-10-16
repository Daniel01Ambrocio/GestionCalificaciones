using gestionescolar.DLL;
using gestionescolar.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace gestionescolar.BLL
{
    public class MateriaBLL
    {
        MateriaDLL MateriaDLL = new MateriaDLL();
        public string RegistrarMateria(Entmateria Materia)
        {
            return MateriaDLL.RegistrarMateria(Materia);
        }
        public string EliminarMateria(Entmateria Materia)
        {
            return MateriaDLL.EliminarMateria(Materia);
        }
        public DataTable ObtenerMaterias()
        {
            return MateriaDLL.ObtenerMaterias();
        }
        public DataTable ObtenerMateriasConID()
        {
            return MateriaDLL.ObtenerMateriasConID();
        }
        public List<int> ObtenerMateriasPorGrado(Entgrupo entgrupo)
        {
            return MateriaDLL.ObtenerMateriasPorGrado(entgrupo);
        }
    }
}