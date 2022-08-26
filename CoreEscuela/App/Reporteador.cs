using CoreEscuela.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEscuela.App
{
    public  class Reporteador
    {
        /*Aportando a lo inicial del curso referente al por qué los constructores de las 
         * clases estáticas NO llevan parámetros es porque estas no pueden ser instanciadas, es decir, no podemos 
         * crear objetos a partir de ellas, además están selladas y no pueden heredarse.
         * Siguiendo, una clase NO estática puede contener un constructor estático sí y sólo sí tiene 
         * miembros estáticos (métodos, propiedades, eventos) pero este constructor, como todo constructor estático, se 
         * ejecuta una sóla vez y será el primero en ejecutarse antes que cualquier otro constructor no estático.
         */

        //Por sugerencia del cuando hagamos un objeto privado es mejor iniciar con un '_'

        Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> _diccionario;
        public Reporteador(Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> dicObsEsc)
        {
            if(dicObsEsc == null)
            {
                throw new ArgumentNullException(nameof(dicObsEsc));
            }
            _diccionario = dicObsEsc;
        }

        public IEnumerable<Evaluación> GetListaEvaluaciones()
        {
            IEnumerable<Evaluación> rta;
            if(_diccionario.TryGetValue(LlaveDiccionario.Evaluación, out IEnumerable<ObjetoEscuelaBase> lista))
            {
                return lista.Cast<Evaluación>();
            }
            {
                return new List<Evaluación>();
            }
        }

        //Sobrecarga de métodos

        public IEnumerable<string> GetListaAsignaturas()
        {
            return GetListaAsignaturas(
                    out var dummy);
        }

        public IEnumerable<string> GetListaAsignaturas(
            out IEnumerable<Evaluación> listaEvaluaciones)
        {
            listaEvaluaciones = GetListaEvaluaciones();

            return (from Evaluación ev in listaEvaluaciones
                    select ev.Asignatura.Nombre).Distinct();
        }

        public Dictionary<string, IEnumerable<Evaluación>> GetDicEvaluaXAsig()
        {
            var dictaRta = new Dictionary<string, IEnumerable<Evaluación>>();

            var listaAsig = GetListaAsignaturas(out var listaEval);

            foreach (var asig in listaAsig)
            {
                var evalsAsig = from eval in listaEval
                                where eval.Asignatura.Nombre == asig
                                select eval;

                dictaRta.Add(asig, evalsAsig);
            }

            return dictaRta;
        }

        public Dictionary<string, IEnumerable<object>> GetPromeAlumnPorAsignatura()
        {
            var rta = new Dictionary<string, IEnumerable<object>>();
            var dicEvalXAsig = GetDicEvaluaXAsig();

            foreach (var asigConEval in dicEvalXAsig)
            {
                var promsAlumn = from eval in asigConEval.Value
                                 group eval by new
                                 {
                                     eval.Alumno.UniqueId,
                                     eval.Alumno.Nombre
                                 }
                            into grupoEvalsAlumno
                                 select new AlumnoPromedio
                                 {
                                     alumnoid = grupoEvalsAlumno.Key.UniqueId,
                                     alumnoNombre = grupoEvalsAlumno.Key.Nombre,
                                     promedio = grupoEvalsAlumno.Average(evaluacion => evaluacion.Nota)
                                 };

                rta.Add(asigConEval.Key, promsAlumn);
            }

            return rta;
        }
    }
}
