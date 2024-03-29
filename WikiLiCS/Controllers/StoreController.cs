﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WikiLiCS.Models;
using System.Linq.Expressions;
using System.Web.Security;


namespace WikiLiCS.Controllers
{
    public class StoreController : Controller
    {
        WikiLiCSEntities storeDB= new WikiLiCSEntities();
        
        public ActionResult Index()//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            var modules = storeDB.Modules.Include("Transactions").Include("Tables").ToList();
            
            return View(modules);
        }

        public ActionResult Browse(string module,string Donde)///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            if ( Donde.ToString()== "TR")
            {
                return RedirectToAction("ListTransactions2", new { filtro = module });
            }
            else
            {
                return RedirectToAction("ListTables", new { filtro = module });
            }
        }

        public ActionResult Details(int id)//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            var transaction = storeDB.Transactions.Find(id);
            return View(transaction);
        }
        public ActionResult DetailsTables(int id)//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            var table = storeDB.Tables.Find(id);
            return View(table);
        }
        [Authorize(Roles="Administrator")]
        public ActionResult EditTransaction(int id)//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {

            if (id != null)
            {
                ViewBag.Modules = storeDB.Modules.OrderBy(g => g.Name).ToList();
                Transaction transaction = storeDB.Transactions.Single(a => a.TransactionId == id);
                return View(transaction);
            }
            else { return HttpNotFound(); }
        }
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult EditTransaction(int id, FormCollection collection)//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            var transaction = storeDB.Transactions.Find(id);
            if (ModelState.IsValid)
            {
                try
                {
                    if (TryUpdateModel(transaction))
                    {
                        storeDB.SaveChanges();
                        return RedirectToAction("ListTransactions", storeDB.Transactions);
                    }
                    else
                    {
                        ViewBag.Modules = storeDB.Modules.OrderBy(g => g.Name).ToList();
                        return View(transaction);
                    }
                }
                catch
                {
                    return View();
                }
            }
            ViewBag.Modules = storeDB.Modules.OrderBy(g => g.Name).ToList();
            return View(transaction);
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult EditTable(int id)//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {

            if (id != null)
            {
                ViewBag.Modules = storeDB.Modules.OrderBy(g => g.Name).ToList();
                Table table = storeDB.Tables.Single(a => a.TableId == id);
                return View(table);
            }
            else { return HttpNotFound(); }
        }
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult EditTable(int id, FormCollection collection)//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            var table = storeDB.Tables.Find(id);
            if (ModelState.IsValid)
            {
                try
                {
                    if (TryUpdateModel(table))
                    {
                        storeDB.SaveChanges();
                        return RedirectToAction("ListTables", storeDB.Tables);
                    }
                    else
                    {
                        ViewBag.Modules = storeDB.Modules.OrderBy(g => g.Name).ToList();
                        return View(table);
                    }
                }
                catch
                {
                    return View();
                }
            }
            ViewBag.Modules = storeDB.Modules.OrderBy(g => g.Name).ToList();
            return View(table);
        }
        public ActionResult BrowseSearch(string SearchString)//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {

            //string cadena = SearchString.ToUpper();
            //var partial1 = storeDB.Transactions.Where(s => s.Code.ToUpper().Contains(cadena));
            //var partial2 = storeDB.Transactions.Where(s => s.Description.ToUpper().Contains(cadena));
            //var transactions = partial1.Union(partial2);
            //return View("ListTransactions", transactions);
            return RedirectToAction("ListTransactions2", new { filtro = SearchString });
            
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult CreateTransaction()//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            ViewBag.Modules = storeDB.Modules.OrderBy(g => g.Name).ToList();
            Transaction transaction = new Transaction();
            return View("EditTransaction",transaction);
        }
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult CreateTransaction(Transaction transaction)//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            if (ModelState.IsValid)
            {
                storeDB.Transactions.Add(transaction);
                storeDB.SaveChanges();
                var transactions = storeDB.Transactions.ToList();
                return View("ListTransactions", transactions);
            }
            ViewBag.Modules = storeDB.Modules.OrderBy(g => g.Name).ToList();
            return View(transaction);
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult CreateTable()//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            ViewBag.Modules = storeDB.Modules.OrderBy(g => g.Name).ToList();
            Table table= new Table();
            return View("EditTable", table);
        }
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult CreateTable(Table table)//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            if (ModelState.IsValid)
            {
                storeDB.Tables.Add(table);
                storeDB.SaveChanges();
                var tables = storeDB.Tables.ToList();
                return View("ListTables", table);
            }
            ViewBag.Modules = storeDB.Modules.OrderBy(g => g.Name).ToList();
            return View(table);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteTransaction(int id)//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            var transaction = storeDB.Transactions.Find(id);
            return View(transaction);
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult DeleteTransaction(int ID, FormCollection collection)//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            var transaction = storeDB.Transactions.Find(ID);
            storeDB.Transactions.Remove(transaction) ;
            storeDB.SaveChanges();
            var transactions = storeDB.Transactions.ToList();
            return View("ListTransactions", transactions);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteTable(int id)//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            var table = storeDB.Tables.Find(id);
            return View(table);
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult DeleteTable(int ID, FormCollection collection)//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            var table = storeDB.Tables.Find(ID);
            storeDB.Tables.Remove(table);
            storeDB.SaveChanges();
            var tables = storeDB.Tables.ToList();
            return View("ListTables", table);
        }

        public ActionResult ListManuals(int page = 1, string sort = "Code", string sortDir = "ASC", string filtro = "")
        {
            const int manualsPorPagina = 40;
            var numManuales = Count(filtro, "Manual");

            IEnumerable<Manual> manuals;
            Direction dir = sortDir.Equals("ASC", StringComparison.CurrentCultureIgnoreCase) ? Direction.Ascendente : Direction.Descendente;
            switch (sort.ToLower())
            {
                case "name":
                    manuals = GetPagesManuals(page, manualsPorPagina, p => p.Name, dir, filtro);
                    break;
                case "description":
                    manuals = GetPagesManuals(page, manualsPorPagina, p => p.Description, dir, filtro);
                    break;
                default:
                    manuals = GetPagesManuals(page, manualsPorPagina, p => p.Name, dir, filtro);
                    break;
            }
            var datos = new PaginaDeManualesViewModel()
            {
                NumeroDeManuales = numManuales,
                ManualesPorPagina = manualsPorPagina,
                Manuals = manuals,
                PaginaActual = page,
                Sort = sort,
                sortDir = sortDir,
                filtro = filtro
            };
            if (sortDir == "ASC")
            { datos.sortDir = "DESC"; }
            else { datos.sortDir = "ASC"; }
            return View("ListManuals", datos);
        }
        public ActionResult DeleteManual(int id)
        {
            var manual=storeDB.Manuals.Single(p=>p.ManualID==id);
            return View("DeleteManual"); 
        }
        [Authorize(Roles="Administrator")]
        [HttpPost]
        public ActionResult DeleteManual(int id,FormCollection collection)
        { 
            var manual=storeDB.Manuals.Single(p=>p.ManualID==id);
            storeDB.Manuals.Remove(manual);
            storeDB.SaveChanges();
            return RedirectToAction("ListManual");
        }

        public ActionResult ListTables(int page = 1, string sort = "Code", string sortDir = "ASC", string filtro = "")
        {
            const int tablasPorPagina = 20;
            var numTablas= Count(filtro,"Table");

            IEnumerable<Table> tablas;
            Direction dir = sortDir.Equals("ASC", StringComparison.CurrentCultureIgnoreCase) ? Direction.Ascendente : Direction.Descendente;
            switch (sort.ToLower())
            {
                case "code":
                    tablas = GetPagesTables(page, tablasPorPagina, p => p.Name, dir, filtro);
                    break;
                case "description":
                    tablas = GetPagesTables(page, tablasPorPagina, p => p.Description, dir, filtro);
                    break;
                default:
                    tablas = GetPagesTables(page, tablasPorPagina, p => p.Name, dir, filtro);
                    break;
            }
            var datos = new PaginaDeTablasViewModel()
            {
                NumeroDeTablas = numTablas,
                TablasPorPagina = tablasPorPagina,
                Tablas = tablas,
                PaginaActual = page,
                Sort = sort,
                sortDir = sortDir,
                filtro = filtro
            };
            if (sortDir == "ASC")
            { datos.sortDir = "DESC"; }
            else { datos.sortDir = "ASC"; }
            return View("ListTables", datos);
        }

        public ActionResult ListTransactions2(int page = 1, string sort = "Code", string sortDir = "ASC",string filtro="")////////////////////////////////////////////////////////////////
        {
            const int transaccionesPorPagina = 20;
            var numTransacciones = Count(filtro,"Transaction");
            
            IEnumerable<Transaction> transactions;
            Direction dir = sortDir.Equals("ASC", StringComparison.CurrentCultureIgnoreCase) ? Direction.Ascendente : Direction.Descendente;
            switch (sort.ToLower())
            {
                case "code":
                    transactions = GetPagesTransactions(page, transaccionesPorPagina, p => p.Code, dir, filtro);
                    break;
                case "description":
                    transactions = GetPagesTransactions(page, transaccionesPorPagina, p => p.Description, dir, filtro);
                    break;
                case "methods":
                    transactions = GetPagesTransactions(page, transaccionesPorPagina, p => p.Methods, dir, filtro);
                    break;
                default:
                    transactions = GetPagesTransactions(page, transaccionesPorPagina, p => p.Code, dir, filtro);
                    break;
            }
            var datos = new PaginaDeTransaccionesViewModel()
            {
                NumeroDeTransacciones = numTransacciones,
                TransaccionesPorPagina = transaccionesPorPagina,
                Transacciones = transactions,
                PaginaActual=page,
                Sort=sort,
                sortDir=sortDir,
                filtro=filtro
            };
            
                if(sortDir=="ASC")
                    { datos.sortDir = "DESC"; }
                 else { datos.sortDir = "ASC"; }
            

            return View("ListTransactions2", datos);
        }


        public IQueryable<Transaction> getQueryTransaction(string filtro)
        {
            IQueryable<Transaction> query;
            if (filtro != "")
            {
                string[] split = filtro.Split(new Char[] { ':' });
                switch (split[0].ToUpper())
                {
                    case "MODULE":
                        var M = split[1].ToUpper();
                        query = storeDB.Transactions.Where(p => p.Module.Name.ToUpper().Contains(M));
                        break;
                    case "DESCRIPTION":
                        var D = split[1].ToUpper();
                        query = storeDB.Transactions.Where(p => p.Description.ToUpper().Contains(D));
                        break;
                    default:
                        string cadena = filtro.ToUpper();
                        query = from e in storeDB.Transactions
                                where e.Code.ToUpper().Contains(cadena) || e.Description.ToUpper().Contains(cadena)
                                select e;
                        break;
                }
            }
            else
            { query = storeDB.Transactions; }
            return query;
        }
        public IQueryable<Manual> getQueryManual(string filtro)
        {
            IQueryable<Manual> query;
            if (filtro != "")
            {
                string[] split = filtro.Split(new Char[] { ':' });
                switch (split[0].ToUpper())
                {
                    case "MODULE":
                        var M = split[1].ToUpper();
                        query = storeDB.Manuals.Where(p => p.Module.Name.ToUpper().Contains(M));
                        break;
                    case "DESCRIPTION":
                        var D = split[1].ToUpper();
                        query = storeDB.Manuals.Where(p => p.Description.ToUpper().Contains(D));
                        break;
                    default:
                        string cadena = filtro.ToUpper();
                        query = from e in storeDB.Manuals
                                where e.Name.ToUpper().Contains(cadena) || e.Description.ToUpper().Contains(cadena)
                                select e;
                        break;
                }
            }
            else
            { query = storeDB.Manuals; }
            return query;
        }

        public IQueryable<Table> getQueryTable(string filtro)
        {
            IQueryable<Table> query;
            if (filtro != "")
            {
                string[] split = filtro.Split(new Char[] { ':' });
                switch (split[0].ToUpper())
                {
                    case "MODULE":
                        var F = split[1].ToUpper();
                        query = storeDB.Tables.Where(p => p.Module.Name.ToUpper().Contains(F));
                        break;
                    case "NAME":
                        var M = split[1].ToUpper();
                        query = storeDB.Tables.Where(p => p.Name.ToUpper().Contains(M));
                        break;
                    case "DESCRIPTION":
                        var D = split[1].ToUpper();
                        query = storeDB.Tables.Where(p => p.Description.ToUpper().Contains(D));
                        break;
                    default:
                        string cadena = filtro.ToUpper();
                        query = from e in storeDB.Tables
                                where e.Name.ToUpper().Contains(cadena) || e.Description.ToUpper().Contains(cadena)
                                select e;
                        break;
                }
            }
            else
            { query = storeDB.Tables; }
            return query;
        }
        public IEnumerable<Transaction> GetPagesTransactions<T>(int actualPage, int transactionsForPage, Expression<Func<Transaction, T>> ordenacion, Direction dir,string filtro)
        {

            if (actualPage < 1) actualPage = 1;
            IQueryable<Transaction> query;
            query = getQueryTransaction(filtro);
            //if (filtro!="")
            //    { 
                
            //    string [] split = filtro.Split(new Char [] {':'});
            //    switch (split[0].ToUpper()){
            //        case "MODULE":
            //            var M=split[1].ToUpper();
            //            //query = storeDB.Transactions.Where(p => p.Module.Name.ToUpper() == M);
            //            query = storeDB.Transactions.Where(p=>p.Module.Name.ToUpper().Contains(M));
            //            break;
            //        case "DESCRIPTION":
            //            var D = split[1].ToUpper();
            //            query = storeDB.Transactions.Where(p => p.Description.ToUpper().Contains(D));
            //            break;
            //        default:
            //            string cadena = filtro.ToUpper();
            //            //query = storeDB.Transactions.Where(s => s.Code.ToUpper().Contains(cadena)).Where(s => s.Description.ToUpper().Contains(cadena));
            //            query = from e in storeDB.Transactions
            //                     where e.Code.ToUpper().Contains(cadena) || e.Description.ToUpper().Contains(cadena)
            //                     select e;
            //            //query = storeDB.Transactions.Where(s => s.Code.ToUpper().Contains(cadena));
            //            //query2 = storeDB.Transactions.Where(s => s.Description.ToUpper().Contains(cadena));
            //            //query = query.Union(query2);
            //            //return View("ListTransactions", transactions);
            //            break;
            //        }
            //    }
            //else
            //    {query = storeDB.Transactions;}

            if (dir == Direction.Ascendente)
                query = query.OrderBy(ordenacion);
            else
                query = query.OrderByDescending(ordenacion);
            //if (module==null)
            return query
                        .Skip((actualPage - 1) * transactionsForPage)
                        .Take(transactionsForPage)
                        .ToList();
            //else
            //return query.Where(s => s.Module.Name == module)
            //            .Skip((actualPage - 1) * transactionsForPage)
            //            .Take(transactionsForPage)
            //            .ToList();
        }
        public IEnumerable<Table> GetPagesTables<T>(int actualPage, int tablesForPage, Expression<Func<Table, T>> ordenacion, Direction dir, string filtro)
        {
            if (actualPage < 1) actualPage = 1;
            IQueryable<Table> query;
            query = getQueryTable(filtro);
            if (dir == Direction.Ascendente)
                query = query.OrderBy(ordenacion);
            else
                query = query.OrderByDescending(ordenacion);
            return query
                        .Skip((actualPage - 1) * tablesForPage)
                        .Take(tablesForPage)
                        .ToList();
        }
        public IEnumerable<Manual> GetPagesManuals<T>(int actualPage, int ManualForPage, Expression<Func<Manual, T>> ordenacion, Direction dir, string filtro)
        {
            if (actualPage < 1) actualPage = 1;
            IQueryable<Manual> query;
            query = getQueryManual(filtro);
            if (dir == Direction.Ascendente)
                query = query.OrderBy(ordenacion);
            else
                query = query.OrderByDescending(ordenacion);
            return query
                        .Skip((actualPage - 1) * ManualForPage)
                        .Take(ManualForPage)
                        .ToList();
        }
        
        public int Count(string filtro,string tipo)//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            //if (tipo == ("Transaction"))
            //{
            //    IQueryable<Transaction> query;
            //    query = getQueryTransaction(filtro);
            //    return query.Count();
            //}
            //else
            //{
            //    IQueryable<Table> query;
            //    query = getQueryTable(filtro);
            //    return query.Count();
            //}
            var cant=0;
            switch (tipo)
            {
                case "Table":
                    IQueryable<Table> query2;
                    query2 = getQueryTable(filtro);
                    cant = query2.Count();    
                    break;
                case "Manual":
                    IQueryable<Manual> query3;
                    query3 = getQueryManual(filtro);
                    cant= query3.Count();
                    break;
                default:
                    IQueryable<Transaction> query;
                    query = getQueryTransaction(filtro);
                    cant= query.Count();
                    break;
            }
            return cant;
            //if (filtro != "")
            //{ return storeDB.Transactions.Where(p => p.Module.Name == filtro).Count(); }
            //else
            //{ return storeDB.Transactions.Count(); }
        }
        //public ActionResult ListTransactionsOrder(string orderby, IEnumerable<Transaction> tr)//////////////////////////////////////////////////////////////////
        //{
        //    ViewBag.CodeSortParm = String.IsNullOrEmpty(orderby) ? "Code desc" : "";
        //    ViewBag.DescriptionSortParm = String.IsNullOrEmpty(orderby) ? "Description desc" : "";
        //    /*var transactions = storeDB.Transactions;*/
        //    var transactions = from s in storeDB.Transactions
        //                       select s;
        //    switch (orderby)
        //    {
        //        case "Code desc":
        //            transactions = transactions.OrderByDescending(s => s.Code);
        //            break;
        //        case "Description":
        //            transactions = transactions.OrderBy(s => s.Description);
        //            break;
        //        case "Description desc":
        //            transactions = transactions.OrderByDescending(s => s.Description);
        //            break;
        //        default:
        //            transactions = transactions.OrderBy(s => s.Code);
        //            break;
        //    }
        //    return View("ListTransactions",transactions);
        //} 
        //public ActionResult ListTransactions()//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //{

        //    if (User.IsInRole("Administrator"))
        //        return View(storeDB.Transactions.ToList());
        //    else
        //        return View(storeDB.Transactions.Where(s => s.SecurityLevel == 0).ToList());
        //}
    }
}
