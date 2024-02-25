using ASP.Server.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASP.Server.ViewModels
{
    public class CreateGenreViewModel
    {
        [Required]
        public string Name { get; set; }
        public IEnumerable<Genre> Genres { get; set; }

        // Liste des genres a afficher à l'utilisateur
        public IEnumerable<Genre> AllGenres { get; init; }
    }
}

