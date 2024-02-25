using ASP.Server.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASP.Server.ViewModels
{
    public class UpdateBookViewModel
    {
        [Required(ErrorMessage = "The name is required.")]
        public string Name { get; set; }

        public string Content { get; set; }

        [Required(ErrorMessage = "The price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "The price must be a positive number.")]
        public double Price { get; set; }
        public int Id { get; set; }

        // Add other necessary fields here

        // List of genres selected by the user
        [Required(ErrorMessage = "You need to select at least 1 genre.")]
        public List<int> SelectedGenres { get; set; }
        public List<int> SelectedAuthors { get; set; }

        public IEnumerable<int> Authors { get; set; }

        // List of all genres available to display to the user
        public IEnumerable<Genre> AllGenres { get; set; }
        public Book Book { get; internal set; }

        public IEnumerable<Author> AllAuthors { get; init; }
    }
}
