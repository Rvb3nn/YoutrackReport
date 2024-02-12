using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutrackReport.DTOs
{
    public class FieldsDTO
    {
        public string Project { get; set; }
        public string idReadable { get; set; }
        public string UrlYT { get; set; }

        //public string links { get; set; }
        //public string Issues { get; set; }

        public List<ProjectIssues> Issues { get; set; }  // Cambiada a lista de Issues
        public List<ProjectLinks> links { get; set; }
        public string ProjectId { get; set; }

        public string summary { get; set; }
        public string Subsystem { get; set; }
        public string Type { get; set; }
        public string Priority { get; set; }
        public string State { get; set; }
        public string RechazoHDI { get; set; }
        public string EncargadoHDI { get; set; }
        public string JefeDeProyecto { get; set; }
        public string Assignee { get; set; }
        public string DueDate { get; set; }
        public string Estimacion { get; set; }
        public string FechaInicio { get; set; }
        public string? FechaTerminoDesa { get; set; } = null;
        public string FechaTerminoQA { get; set; }
        public string FechaTerminoReal { get; set; }
        public string URLJira { get; set; }
        public string URLBitbucket { get; set; }
        public string URLSonarQube { get; set; }
        public string Dificultad { get; set; }
        public string IDMh { get; set; }
        public string IDAgil { get; set; }
        public string SprintsSeparadosPorComa { get; set; }
        public string Completado { get; set; }
        public string FixedInBuild { get; set; }

        public bool TieneQA()
        {
            return (Issues != null && Issues.Any()) || (links != null && links.Any(link => link.issues != null && link.issues.Any()));
        }


        public IEnumerable<object> ToArray()
        {
            return new object[] {
        Project,
        Subsystem,
        Type,
        Priority,
        State,
        RechazoHDI,
        EncargadoHDI,
        JefeDeProyecto,
        Assignee,
        DueDate,
        Estimacion,
        FechaInicio,
        FechaTerminoDesa,
        FechaTerminoQA,
        FechaTerminoReal,
        URLJira,
        URLBitbucket,
        URLSonarQube,
        Dificultad,
        IDMh,
        IDAgil,
        SprintsSeparadosPorComa,
        Completado,
        FixedInBuild
        };
        }



    }

}
