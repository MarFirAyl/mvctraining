using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vidly.Models;

namespace Vidly.ViewModels
{
    public class MovieFormViewModel
    {
        public IEnumerable<Genre> Genres { get; set; }
        // verwijderen zodat de form netjes getoond wordt zonder bijv standaar datum van : 1-1-1900
        //public Movie Movie { get; set; } 


        //ipv de movie als property te gebruiken worden hier de afzonderlijke properties van de Movie gebruiket om een "pure" viewmodel te creëren.
        public int? Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please select a Genre")]
        [Display(Name= "Genre")]
        public byte? GenreId { get; set; }

        [Required]
        [Display(Name = "Release Date")]
        public DateTime? ReleaseDate { get; set; }
        
        [Required]
        [Display(Name = "Number in Stock")]
        [Range(1, 100)]
        public int? NumberInStock { get; set; }

        // Ik had hier een adnere oplossing voor bedacht. Zie de comments in het MovieForm.cshtml script.

        public string Title
        {
            get
            {
                return Id != 0 ? "Edit Movie" : "New Movie";
                //if (Id != 0)
                //{
                //    return "Edit Movie";
                //}
                //return "New Movie";
            }
        }


        // Toevoegen constructors om de viewmodel te kunnen vullen met data uit een meegegeven Movie model.
        public MovieFormViewModel()
        {
            //  Id op nul zetten om validation errors p het hidden field te voorkomen
            Id = 0;
        }

        public MovieFormViewModel(Movie movie)
        {
            Id = movie.Id;
            Name = movie.Name;
            ReleaseDate = movie.ReleaseDate;
            NumberInStock = movie.NumberInStock;
            GenreId = movie.GenreId;
        }
    }
}