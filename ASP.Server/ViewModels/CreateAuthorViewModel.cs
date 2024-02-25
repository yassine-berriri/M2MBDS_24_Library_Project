using ASP.Server.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASP.Server.ViewModels
{
    // Un ViewModel représente le contenu envoyé à la vu mais aussi le contenu récupéré a travers une requète HTTP:
    // -> /api/1.0.0/xxx?prop1=123&prop2=azerty&prop3[]=0&prop3[]=1
    public class CreateAuthorViewModel
    {
        [Required]
        public String FullName { get; set; }

        [Required]
        public string Email { get; set; }
        
        [Required]
        public DateOnly Birthday { get; set; }

        public String Bio { get; set; }

        public IEnumerable<int> Books { get; set; }

        public IEnumerable<Author> AllAuthors { get; init; }
        public IEnumerable<Book> AllBooks { get; init; }

    }
}
