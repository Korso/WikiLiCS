using System;
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
        //
        // GET: /Store/
        //public ActionResult ListTransactions(int page = 1, string sort = "Code", string sortDir = "ASC")////////////////////////////////////////////////////////////////
        //{
        //    const int transaccionesPorPagina = 20;
        //    var numTransacciones = CountTransactions();
            
        //    IEnumerable<Transaction> transactions;
        //    Direction dir = sortDir.Equals("ASC", StringComparison.CurrentCultureIgnoreCase) ? Direction.Ascendente : Direction.Descendente;
        //    switch (sort.ToLower())
        //    {
        //        case "code":
        //            transactions = GetPagesTransactions( page, transaccionesPorPagina, p => p.Code, dir);
        //            break;
        //        case "description":
        //            transactions = GetPagesTransactions(page, transaccionesPorPagina, p => p.Description, dir);
        //            break;
        //        case "methods":
        //            transactions = GetPagesTransactions(page, transaccionesPorPagina, p => p.Methods, dir);
        //            break;
        //        default:
        //            transactions = GetPagesTransactions(page, transaccionesPorPagina, p => p.Code, dir);
        //            break;
        //    }
        //    var datos=new PaginaDeTransaccionesViewModel()
        //    {
        //        NumeroDeTransacciones=numTransacciones,
        //        TransaccionesPorPagina=transaccionesPorPagina,
        //        Transacciones=transactions
        //    };
        //    return View("ListTransactions", datos);
        //}
        public ActionResult Index()//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            var modules = storeDB.Modules.Include("Transactions").ToList();
            return View(modules);
        }
        public ActionResult Browse(string module)///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            //const int transaccionesPorPagina = 20;
            //var numTransacciones = storeDB.Transactions.Where(s=>s.Module.Name==module).Count();

            //IEnumerable<Transaction> transactions;
            //Direction dir = sortDir.Equals("ASC", StringComparison.CurrentCultureIgnoreCase) ? Direction.Ascendente : Direction.Descendente;
            //switch (sort.ToLower())
            //{
            //    case "code":
            //        transactions = GetPagesTransactions(page, transaccionesPorPagina, p => p.Code, p => p.Code, dir, module);
            //        break;
            //    case "description":
            //        transactions = GetPagesTransactions(page, transaccionesPorPagina, p => p.Description, dir, module);
            //        break;
            //    case "methods":
            //        transactions = GetPagesTransactions(page, transaccionesPorPagina, p => p.Methods, dir, module);
            //        break;
            //    default:
            //        transactions = GetPagesTransactions(page, transaccionesPorPagina, p => p.Code, dir, module);
            //        break;
            //}
            //var datos = new PaginaDeTransaccionesViewModel()
            //{
            //    NumeroDeTransacciones = numTransacciones,
            //    TransaccionesPorPagina = transaccionesPorPagina,
            //    Transacciones = transactions
            //};
            //return View("ListTransactions", datos);
            //IList<Transaction> tr=storeDB.Transactions.Where(s=>s.Module.Name==module).ToList();
            //return RedirectToAction("ListTransactions", new { transaction = tr });
            //return RedirectToAction("ListTransactions", new {transaction = tr, module = module, page = 1 });
            //return RedirectToAction("ListTransactions", new {module=module});
            //int transaccionesPorPagina = 20;
            //IEnumerable<Transaction> transactions = storeDB.Transactions.Where(s => s.Module.Name == module).ToList();
            //var numTransacciones = transactions.Count();
            var omodule=storeDB.Modules.Where(s=> s.Name==module).Single();
            ViewBag.Module = omodule.Name + " - " + omodule.Description;
            //var datos = new PaginaDeTransaccionesViewModel()
            //{
            //    NumeroDeTransacciones = numTransacciones,
            //    TransaccionesPorPagina = transaccionesPorPagina,
            //    Transacciones = transactions
            //};
            ////return RedirectToAction("ListTransactionsForModul", transactions);
            var datos = storeDB.Transactions.Where(s => s.Module.Name == module);
            return View("ListTransactions", datos);
        }


        public ActionResult ListTransactions()//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {

            if (User.IsInRole("Administrator"))
                return View(storeDB.Transactions.ToList());
            else
                return View(storeDB.Transactions.Where(s => s.SecurityLevel == 0).ToList());
            
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
        public ActionResult Details(int id)//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            var transaction = storeDB.Transactions.Find(id);
            return View(transaction);
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

        public ActionResult BrowseSearch(string SearchString)//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            string cadena = SearchString.ToUpper();
            var partial1 = storeDB.Transactions.Where(s => s.Code.ToUpper().Contains(cadena));
            var partial2 = storeDB.Transactions.Where(s => s.Description.ToUpper().Contains(cadena));
            var transactions = partial1.Union(partial2);
            return View("ListTransactions", transactions);
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
        /*
        public int CountTransactions()//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            return storeDB.Transactions.Count();
        }
        
        public IEnumerable<Transaction> GetPagesTransactions(int actualPage, int transactionsForPage, string orderC)//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            var o = orderC;
            if (actualPage < 1) actualPage = 1;
            var tr = storeDB.Transactions.OrderBy(


            return storeDB.Transactions
              .OrderBy(p=>p.Description)
              .Skip((actualPage - 1) * transactionsForPage)
              .Take(transactionsForPage)
              .ToList();
        }
        public IEnumerable<Transaction> GetPagesTransactions<T>(int actualPage, int transactionsForPage, Expression<Func<Transaction, T>> ordenacion,  Direction dir)
        {
            
            if (actualPage < 1) actualPage = 1;
            IQueryable<Transaction> query;
            query = storeDB.Transactions;

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
        */



    }
}
