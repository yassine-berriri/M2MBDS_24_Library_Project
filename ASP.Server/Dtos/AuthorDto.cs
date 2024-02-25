using System;

namespace ASP.Server.Dtos
{
    public class AuthorDto
    {
        public int Id { get; set; }

        public string FullName { get; set; }


        public string Email { get; set; }


        public DateOnly Birthday { get; set; }

        public String Bio { get; set; }

    }
}
