using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vidly.Dtos;
using Vidly.Models;

namespace Vidly.Controllers.Api
{
    public class CustomersController : ApiController
    {
        private ApplicationDbContext _context;

        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }

        // GET /api/customers - get a list of customers (default MVC)
        public IEnumerable<CustomerDto> GetCustomers()
        {
            return _context.Customers.ToList().Select(Mapper.Map<Customer, CustomerDto>);
        }

        // GET /api/customers/1 (id) - get 1 customer
        public IHttpActionResult GetCustomer(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);
            
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<Customer, CustomerDto>(customer));
        }

        // POST /api/customers - toevoegen van een customer
        // Wanneer de methode "PostCustomer" genoemd wordt is de [HttpPost] annotation niet nodig (best practice Microsoft). Deze annatoatie wordt gebruikt zodat de methode alleen vanuit een post request geroepen kan worden. Dit is aan te raden bij elke geval waarin een resource wordt gecreëerd.
        [HttpPost]
        public IHttpActionResult CreateCustomer(CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var customer = Mapper.Map<CustomerDto, Customer>(customerDto);
            _context.Customers.Add(customer);
            _context.SaveChanges();

            customerDto.Id = customer.Id;

            return Created(new Uri(Request.RequestUri + "/" + customer.Id), customerDto);
        }

        // PUT /api/customers
        [HttpPut]
        public void UpdateCustomer(int id, CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customerInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            // omdat er een bestaand objext gebruikt wordt, kan er een tweede argument meegegeven worden aan de Mapper.Map methode (het target objet) in plaats van het creëren van een niwuw object
            //Mapper.Map<CustomerDto, Customer>(customerDto, customerInDb); // same as :
            Mapper.Map(customerDto, customerInDb);
            // updating customer -- verwijderen omdat automapper wordt gebruikt
            //customerInDb.Name = customerDto.Name;
            //customerInDb.Birthdate = customerDto.Birthdate;
            //customerInDb.MembershipTypeId = customerDto.MembershipTypeId;
            //customerInDb.IsSubscribedToNewsletter = customerDto.IsSubscribedToNewsletter;

            _context.SaveChanges();
        }

        // DELETE /api/customers/1
        [HttpDelete]
        public void DeleteCustomer(int id)
        {
            var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customerInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            _context.Customers.Remove(customerInDb);
            _context.SaveChanges();
        }


    }
}
