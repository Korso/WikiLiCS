using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WikiLiCS.Models;

namespace WikiLiCS.Models
{
    public class PaginaDeManualesViewModel
    {
        public int NumeroDeManuales { get; set; }
        public IEnumerable<Manual> Manuals { get; set; }
        public int ManualesPorPagina { get; set; }
        public int PaginaActual { get; set; }
        public string Sort { get; set; }
        public string sortDir { get; set; }
        public string filtro { get; set; }
    }

}