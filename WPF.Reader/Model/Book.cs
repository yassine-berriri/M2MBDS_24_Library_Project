using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPF.Reader.Model
{
    // A vous de completer ce qu'est un Livre !!
    // /!\ ATTENTION ! Si vous récupéré les livres depuis votre serveur, cette classe ne sert plus a rien !
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Content { get; set; }

        public double Price { get; set; }

        public ICollection<Genre> Genres { get; set; }

        public ICollection<Author> Authors { get; set; }
    }
}
