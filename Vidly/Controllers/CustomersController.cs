using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext _context;
        // Om verbinding mnet de database te maken is een DbContext nodig
        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }

        // De context is disposable daarom moet volgende methode worden ingeregeld
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult CustomerForm()
        {
            var memberShipTypes = _context.MembershipTypes.ToList();
            var viewModel = new CustomerFormViewModel
            {
                MembershipTypes = memberShipTypes
            };
            return View(viewModel);
       
        }

        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
            
            return RedirectToAction("Index", "Customers");
        }

        // GET: Customers
        public ViewResult Index()
        {
            //var customers = GetCustomers();
            var customers = _context.Customers.Include(c => c.MembershipType);  // Add "c => c.MembershipType" om eager loading te gebruiken, dwz: ook afhankelijke objecten laden.
            // De query die volgt uit deze statement wordt niet meteen uitgevoerd.
            // Dit gebeurd pas als er over de lijst met customers wordt geitereerd. 
            // Het toevoegen van de ".ToList()" zorgt er voor dat de query meteen uitgevoerd wordt.
            return View(customers);
        }

        public ActionResult Details(int id)
        {
            //var customer = GetCustomers().SingleOrDefault(c => c.Id == id);
            var customer = _context.Customers.Include(c => c.MembershipType).SingleOrDefault(c => c.Id == id); // data ophalen uit de database
            // ^^ Eerst de Inculde dan de SingleOrDefault omdat dit de eigenlijke select is.
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        public ActionResult Edit(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            var viewModel = new CustomerFormViewModel
            {
                Customer = customer,
                MembershipTypes = _context.MembershipTypes.ToList()
            };


            return View("CustomerForm", viewModel);
        }

        //private IEnumerable<Customer> GetCustomers()
        //{
        //    return new List<Customer>
        //    {
        //        new Customer {Id = 1, Name = "Marcel"},
        //        new Customer {Id = 2, Name = "Firuzan"}
        //    };
        //}
    }
}