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
        public string RegistrarGrupo(Entgrupo entgrupo)
        {
            int idgrupo = BuscarExistenciaGrupo( entgrupo);
            if(idgrupo == 0)
            {
                return grupoDLL.RegistrarGrupo(entgrupo);
            }
            else
            {
                return "El grupo ya existe.";
            }
        }
        public int RegistrarGrupoObtenerIDGrupo(Entgrupo entgrupo)
        {
            return grupoDLL.RegistrarGrupoObtenerIDGrupo(entgrupo);
        }

        public string EliminarGrupo(Entgrupo entgrupo)
        {
            return grupoDLL.EliminarGrupo(entgrupo);
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
        public int BuscarExistenciaGrupo(Entgrupo entgrupo)
        {
            return grupoDLL.BuscarExistenciaGrupo(entgrupo);
        }
        public string GenerarGruposNuevoPeriodo(int periodoActual, int periodoNuevo)
        {
            Entgrupo grupoent = new Entgrupo();
            string mensaje = "";
            DataTable dtgrupos = new DataTable();
            dtgrupos = ObtenerGruposDelPeriodo(periodoActual);
            int idGrupoNuevo = 0;
            DataTable dtAlumnos = new DataTable();
            Entalumno alumnoent = new Entalumno();
            int cantidadGrupos = 0;
            int cantidadAlumnos = 0;
            int nuevoGrado = 0;
            //obtenemos la lista de grupos del periodo actual
            if (dtgrupos != null)
            {
                //recorrer cada grupo obtenido(grupoObtenido) 
                cantidadGrupos = dtgrupos.Rows.Count;
                for (int grupoObtenido = 0; grupoObtenido < cantidadGrupos; grupoObtenido++)
                {
                    //orden 0:IDGrupo, 1:grado,2: grupo,3: anio
                    //insertamos un nuevo grupo con la informacion de grupoObtenido pero en el periodoActual
                    grupoent.IDGrupo = Convert.ToInt16(dtgrupos.Rows[grupoObtenido][0]);//IDGrupo
                    nuevoGrado = Convert.ToInt16(dtgrupos.Rows[grupoObtenido][1]) + 1;
                    grupoent.grado = nuevoGrado;//grado
                    grupoent.grupo = Convert.ToString(dtgrupos.Rows[grupoObtenido][2]);//grupo
                    grupoent.anio = periodoNuevo;//anio
                    //validamos que no eista el grupo
                    int idgrupo = BuscarExistenciaGrupo(grupoent);
                    if (idgrupo == 0)
                    {
                        //No existe el grupo, se registra un nuevo grupo
                        //obtenemos su IDGrupoNuevo
                        idGrupoNuevo = RegistrarGrupoObtenerIDGrupo(grupoent);
                        
                    }
                    else
                    {
                        //si existe obtenemos su idgrupo sin registrar un grupo nuevo
                        idGrupoNuevo = idgrupo;
                    }
                        
                    if (idGrupoNuevo > 0)
                    {
                        //obtenemos la lista de matriculas que pertenezcan al idgrupoAnterior
                        //orden: 0: NombreCompleto 1: Matricula,
                        dtAlumnos = alumnoBLL.ObtenerAlumnosPorGrupo(grupoent);
                        cantidadAlumnos = dtAlumnos.Rows.Count;
                        if (cantidadAlumnos > 0)
                        {
                            //recorremos cada alumno
                            
                            for (int alumnosObtenido = 0; alumnosObtenido < cantidadAlumnos; alumnosObtenido++)
                            {
                                //Actualizamos el idgrupoAnterior por el IDGrupoNuevo a todos loa alumnos obtenidos
                                alumnoent.Matricula = Convert.ToInt16(dtAlumnos.Rows[alumnosObtenido][1]);//matricula
                                alumnoent.IDGrupo = idGrupoNuevo;
                                mensaje = alumnoBLL.ActualizarGrupoAlumno(alumnoent);
                                if (mensaje != "Actualización correcta.")
                                {
                                    return "Error al actualizar el grupo de los alumnos. Intentelo más tarde.";
                                }
                            }
                        }
                        else
                        {
                            return "Todos los alumnos ya han sido asignados a sus nuevos grupos.";
                        }

                    }
                    else
                    {
                        mensaje = "Error al generar nuevos grupos. Intentelo más tarde.";
                        return mensaje;
                    }
                }
                return "Algo salio mal. Intentelo más tarde.";
            }
            else
            {
                mensaje = "Error. Intentelo más tarde.";
                return mensaje;
            }
        }
    }
}