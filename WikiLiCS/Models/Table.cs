﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
namespace WikiLiCS.Models
{
    public class Table
    {
        public int TableId { get; set; }
        public int ModuleID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [DataType(DataType.MultilineText)]
        public string Info { get; set; }
        public Module Module { get; set; }
    }
}

        