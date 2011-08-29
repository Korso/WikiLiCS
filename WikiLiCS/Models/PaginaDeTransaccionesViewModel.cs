using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WikiLiCS.Models;

namespace WikiLiCS.Models
{
    public class PaginaDeTransaccionesViewModel
    {
        public int NumeroDeTransacciones { get; set; }
        public IEnumerable<Transaction> Transacciones { get; set; }
        public int TransaccionesPorPagina { get; set; }
    }
}
