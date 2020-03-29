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

        public ActionResult New()
        {
            var memberShipTypes = _context.MembershipTypes.ToList();
            var viewModel = new CustomerFormViewModel
            {
                // Een nieuw customer initieren om validation errors (op id, hidden field) op te lossen.
                Customer = new Customer(),
                MembershipTypes = memberShipTypes
            };
            return View("CustomerForm", viewModel);
       
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new CustomerFormViewModel
                {
                    Customer = customer,
                    MembershipTypes = _context.MembershipTypes.ToList()
                };

                return View("CustomerForm", viewModel);
            }

            if (customer.Id == 0)
            {
                _context.Customers.Add(customer);
            }
            else
            {
                var existingCustomerInDb = _context.Customers.Single(c => c.Id == customer.Id);
                // updaten van de record in de database kan op twee manieren.
                // TryUpdateModel(existingCustomerInDb, "", new string[] { "Name", "Email"}); 
                // De manier die door Microsoft wordt gebruikt voor het updaten van een customer. Deze levert wat problemen op. Kwaadwillende gebruikers kunnen op deze manier alle properties veranderen. Met het derde argumetn kunen welliswaar argumenten ge-whitelist worden. Maar dit levert een probleem op als de naam van een property wordt aangepast later. Het alternatief is de properites handmatig vullen.
                existingCustomerInDb.Name = customer.Name;
                existingCustomerInDb.Birthdate = customer.Birthdate;
                existingCustomerInDb.MembershipType = customer.MembershipType;
                existingCustomerInDb.IsSubscribedToNewsletter = customer.IsSubscribedToNewsletter;
                // ER kan een lib zoals "Automapper" worden gebruikt om de properties te mappen. 
            }
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