using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WikiLiCS.Models
{
    [Bind(Exclude = "TransactionId")]
    public class Transaction
    {
        [ScaffoldColumn(false)]
        public int TransactionId { get; set; }

        [DisplayName("Module")]
        public int ModuleId { get; set; }

        [Required(ErrorMessage = "Transaction code is required")]
        [StringLength(50)]
        public string Code { get; set; }

        [Required(ErrorMessage = "Transaction description is required")]
        [StringLength(150)]
        public string Description { get; set; }

        [DataType(DataType.MultilineText)]
        public string Methods { get; set; }
        [DataType(DataType.MultilineText)]
        public string Parameters { get; set; }
        public string TransactionURL { get; set; }
        public Module Module { get; set; }
        [Range(0, 100, ErrorMessage = "SecurityLevel must be betwenn 0 and 100")]
        public int SecurityLevel { get; set; }
    }

}