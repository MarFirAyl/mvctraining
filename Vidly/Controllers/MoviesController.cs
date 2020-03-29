using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext _context;

        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        
        private IEnumerable<Movie> GetMovies()
        {
            return new List<Movie>
            {
                new Movie {Id = 1, Name = "Shrek!"},
                new Movie {Id = 2, Name = "Pulp Fiction"},
                new Movie {Id = 3, Name = "JoJo Rabbit"}
            };
        }

        public ViewResult Index()
        {

            var movies = _context.Movies.Include(m => m.Genre);
            return View(movies);

        }

        public ActionResult New()
        {
            var genres = _context.Genres.ToList();
            var viewModel = new MovieFormViewModel
            {
                //Movie = new Movie(),
                Genres = genres
            };
            return View("MovieForm", viewModel);

        }

        public ActionResult Edit(int id)
        {
            var movie = _context.Movies.SingleOrDefault(m => m.Id == id);
            if (movie == null)
            {
                return HttpNotFound();
            }

            var viewModel = new MovieFormViewModel(movie)
            {
                Genres = _context.Genres.ToList()
            };


            return View("MovieForm", viewModel);
        }

        public ActionResult Details(int id)
        {
            var movie = _context.Movies.Include(m => m.Genre).SingleOrDefault(m => m.Id == id);

            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }


        // GET: Movies/Random
        public ActionResult Random()
        {
            var movie = new Movie() { Name = "Shrek!" };
            var customers = new List<Customer>
            {
                new Customer { Name = "Customer 1"},
                new Customer { Name = "Customer 2"}
            };

            var viewModel = new RandomMovieViewModel
            {
                Movie = movie,
                Customers = customers

            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new MovieFormViewModel(movie)
                {
                    Genres = _context.Genres.ToList()
                };
                return View("MovieForm", viewModel);
            }
            if (movie.Id == 0)
            {
                _context.Movies.Add(movie);
            }
            else
            {
                var existingMovieInDb = _context.Movies.Single(c => c.Id == movie.Id);
                // updaten van de record in de database kan op twee manieren.
                // TryUpdateModel(existingMovieInDb, "", new string[] { "Name", "Genre"}); 
                // De manier die door Microsoft wordt gebruikt voor het updaten van een customer. Deze levert wat problemen op. Kwaadwillende gebruikers kunnen op deze manier alle properties veranderen. Met het derde argumetn kunen welliswaar argumenten ge-whitelist worden. Maar dit levert een probleem op als de naam van een property wordt aangepast later. Het alternatief is de properites handmatig vullen.
                existingMovieInDb.Name = movie.Name;
                existingMovieInDb.ReleaseDate = movie.ReleaseDate;
                existingMovieInDb.Genre = movie.Genre;
                existingMovieInDb.NumberInStock = movie.NumberInStock;
                // ER kan een lib zoals "Automapper" worden gebruikt om de properties te mappen. 
            }
            try
            {
                _context.SaveChanges();
            }
            // Troubleshooting Entityvalidation errors
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }

            return RedirectToAction("Index", "Movies");
        }

        //public ActionResult Edit(int id)
        //{
        //    var movie = new Movie(); // _context.Movies.SingleOrDefault(c => c.Id == id);
        //    //if (movie == null)
        //    //{
        //    //    return HttpNotFound();
        //    //}

        //    var viewModel = new MovieFormViewModel
        //    {
        //        Movie = movie,
        //        Genres = _context.Genres.ToList()
        //    };


        //    return View("MovieForm", viewModel);
        //}

        //public ActionResult ByReleaseDate(int year, int month)
        //{
        //    return Content(year +"/"+month);
        //}
    }
}