using gestionescolar.DLL;
using gestionescolar.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace gestionescolar.BLL
{
    public class GrupoBLL
    {
        GrupoDLL grupoDLL = new GrupoDLL();
        AlumnoBLL alumnoBLL = new AlumnoBLL();
        public string RegistrarGrupo(Entgrupo grupo)
        {
            return grupoDLL.RegistrarGrupo(grupo);
        }
        public int RegistrarGrupoObtenerIDGrupo(Entgrupo grupo)
        {
            return grupoDLL.RegistrarGrupoObtenerIDGrupo(grupo);
        }

        public string EliminarGrupo(Entgrupo grupo)
        {
            return grupoDLL.EliminarGrupo(grupo);
        }
        public bool ExisteAlumnoEnGrupo(Entgrupo entgrupo)
        {
            return grupoDLL.ExisteAlumnoEnGrupo(entgrupo);
        }
        public DataTable ObtenerGrupos()
        {
            return grupoDLL.ObtenerGrupos();
        }
        public DataTable ObtenerGruposDelPeriodo(int periodoActual)
        {
            return grupoDLL.ObtenerGruposDelPeriodo(periodoActual);
        }
        public DataTable ObtenerGruposConID()
        {
            return grupoDLL.ObtenerGruposConID();
        }
        public DataTable CargarGruposPorMaestro(EntUsuario entUsuario)
        {
            return grupoDLL.CargarGruposPorMaestro(entUsuario);
        }
        public int ObtenerGradoPorIdGrupo(Entgrupo entgrupo)
        {
            return grupoDLL.ObtenerGradoPorIdGrupo(entgrupo);
        }
        public string GenerarGruposNuevoPeriodo(int periodoActual, int periodoNuevo)
        {
            Entgrupo grupoent = new Entgrupo();
            string mensaje = "";
            DataTable dtgrupos = new DataTable();
            dtgrupos = ObtenerGruposDelPeriodo(periodoActual);
            int idGrupoNuevo = 0;
            //obtenemos la lista de grupos del periodo actual
            if(dtgrupos != null)
            {
                //recorrer cada grupo obtenido(grupoObtenido) 
                for (int grupoObtenido = 0; grupoObtenido < dtgrupos.Rows.Count; grupoObtenido++)
                {
                    //orden 0:IDGrupo, 1:grado,2: grupo,3: anio
                    //insertamos un nuevo grupo con la informacion de grupoObtenido pero en el periodoActual
                    grupoent.grado = Convert.ToInt16(dtgrupos.Rows[grupoObtenido][1]);//grado
                    grupoent.grupo = Convert.ToString(dtgrupos.Rows[grupoObtenido][2]);//grupo
                    grupoent.anio = periodoNuevo;//anio
                    //obtenemos su IDGrupoNuevo
                    idGrupoNuevo = RegistrarGrupoObtenerIDGrupo(grupoent);
                    if (idGrupoNuevo > 0)
                    {
                        //obtenemos la lista de matriculas que pertenezcan al idgrupoAnterior

                        //cambiamos el idgrupoAnterior por el IDGrupoNuevo a todos loa alumnos obtenidos
                        return "Exito.";
                    }
                    else
                    {
                        mensaje = "Error al generar nuevos grupos. Intentelo más tarde.";
                        return mensaje;
                    }

                }
            }
            else
            {
                mensaje= "Error. Itentelo más tarde.";
                return mensaje;
            }
        }
    }
}