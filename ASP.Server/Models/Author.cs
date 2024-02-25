using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.NetworkInformation;

namespace ASP.Server.Models
{
    public class Author
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Key]
        public int Id { get; set; }

        // Mettez ici les propriété de votre livre: Nom et Livres associés
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }


        [Required]
        public DateOnly Birthday { get; set; }

        public String Bio { get; set; }
 
        public ICollection<Book> Books { get; set; }
    }

}

