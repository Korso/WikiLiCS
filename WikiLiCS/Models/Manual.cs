using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
namespace WikiLiCS.Models
{
    public class Manual
    {
        public int ManualID { get; set; }
        public int ModuleID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string linkURL { get; set; }
        public string imageURL { get; set; }
        public Module Module { get; set; }
    }
}