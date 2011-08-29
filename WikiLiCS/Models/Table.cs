using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WikiLiCS.Models
{
    public class Table
    {
        public int TableId { get; set; }
        public int ModuleId { get; set; }
        public string Nme { get; set; }
        public string Description { get; set; }
        public string Info { get; set; }
    }
}

        