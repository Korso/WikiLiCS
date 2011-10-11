using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WikiLiCS.Models;

namespace WikiLiCS.Models
{
    public class PaginaDeTablasViewModel
    {
        public int NumeroDeTablas { get; set; }
        public IEnumerable<Table> Tablas { get; set; }
        public int TablasPorPagina { get; set; }
        public int PaginaActual { get; set; }
        public string Sort { get; set; }
        public string sortDir { get; set; }
        public string filtro { get; set; }
    }
}