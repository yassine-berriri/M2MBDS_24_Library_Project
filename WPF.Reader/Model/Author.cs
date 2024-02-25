using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WPF.Reader.Model
{
    public class Author
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public DateOnly Birthday { get; set; }

        public String Bio { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
