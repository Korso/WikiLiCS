using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WikiLiCS.Models
{
    public partial class Module
    {
        public int ModuleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Transaction> Transactions { get; set; }
        public List<Table> Tables { get; set; } 
    }
}