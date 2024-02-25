using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ASP.Server.Models
{
    public class Book
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Content { get; set; }

        [Required]
        public double Price { get; set; }

        public ICollection<Genre> Genres { get; set; }

        public ICollection<Author> Authors { get; set; }

    }
}