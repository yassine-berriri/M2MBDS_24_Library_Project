using ASP.Server.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace ASP.Server.Dtos
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public double Price { get; set; }
        public string Content { get; set; }

        public ICollection<GenreDto> Genres { get; set; }

        public ICollection<AuthorDto> Authors { get; set; }  


    }
}
