using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace WikiLiCS.Models
{
    public class WikiLiCSEntities : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Table> Tables { get; set; }
    }
}