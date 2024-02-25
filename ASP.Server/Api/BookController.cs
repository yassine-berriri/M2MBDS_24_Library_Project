using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.Server.Database;
using ASP.Server.Models;
using AutoMapper;
using ASP.Server.Dtos;
using AutoMapper.QueryableExtensions;
using System.ComponentModel;

namespace ASP.Server.Api
{

    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class BookController(LibraryDbContext libraryDbContext, IMapper mapper) : ControllerBase
    {
        private readonly LibraryDbContext libraryDbContext = libraryDbContext;
        private readonly IMapper mapper = mapper;

        // Methode a ajouter : 
        // - GetBooks
        //   - Entrée: Optionel -> Liste d'Id de genre, limit -> defaut à 10, offset -> défaut à 0
        //     Le but de limit et offset est dé créer un pagination pour ne pas retourner la BDD en entier a chaque appel
        //   - Sortie: Liste d'object contenant uniquement: Auteur, Genres, Titre, Id, Prix
        //     la liste restourner doit être compsé des élément entre <offset> et <offset + limit>-
        //     Dans [1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20] si offset=8 et limit=5, les élément retourner seront : 8, 9, 10, 11, 12


        [HttpGet]
        [Produces("application/json")]
        [Description("recuper juste des  Livres en pasant les id des genres et limit et l'offset en parametre")]
     //   [EndpointDescription("Retrieves a paginated list of books,Pagination parameters can be used to control the number of results returned , An array of genre IDs to filter the books. Optional ,The number of books to skip. Defaults to 0 ,>A list of books, each containing details such as title, author, and genres ,Returns the paginated list of books .")]
        public ActionResult<IEnumerable<Book2Dto>> GetBooks([FromQuery] int[] genres, int limit = 100, int offset = 0)
        {

            IQueryable<Book> query = libraryDbContext.Books.Include(book => book.Genres).Include(book => book.Authors); 


            if (genres != null && genres.Length>0)
            {
                query = query.Where(book => book.Genres.Any(genre => genres.Contains(genre.Id)));
            }

            var paginatedBooks=  query.Skip(offset).Take(limit).ToList();
            HttpContext.Response.Headers.Append("X-Total-Count", (paginatedBooks.Count()).ToString());
            HttpContext.Response.Headers.Append("X-Start-Index", offset.ToString());
            HttpContext.Response.Headers.Append("X-End-Index", (offset + paginatedBooks.Count - 1).ToString());

            return mapper.Map<List<Book2Dto>>(paginatedBooks);   

        }

        /*
        [HttpGet]
        [Produces("application/json")]
        public ActionResult<IEnumerable<BookDto>> GetBooks()
        {

            return mapper.Map<List<BookDto>>(libraryDbContext.Books);

        }
        */

       
        [HttpGet]   
        [Produces("application/json")]
        [Description("recuper juste un seul book en pasant l'id en parametre")]
        public ActionResult<BookDto> GetBook(int id)
        {
            Book book = libraryDbContext.Books.Include(book => book.Genres).Include(book => book.Authors).First(book => book.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return mapper.Map<BookDto>(book);
        }


        // Je vous montre comment faire la 1er, a vous de la compléter et de faire les autres !

        // - GetBook
        //   - Entrée: Id du livre
        //   - Sortie: Object livre entier

        // - GetGenres
        //   - Entrée: Rien
        //   - Sortie: Liste des genres

        [HttpGet]
        [Produces("application/json")]
        [Description("recuper les genres")]
        public ActionResult<IEnumerable<GenreDto>> GetGenres()
        {
            var genres = libraryDbContext.Genre
                .Select(genre => new GenreDto
                {
                    Id = genre.Id,
                    Name = genre.Name
                })
                .ToList();

            return Ok(genres);
        }




        // Aide:
        // Pour récupéré un objet d'une table :
        //   - libraryDbContext.MyObjectCollection.<Selecteurs>.First()
        // Pour récupéré des objets d'une table :
        //   - libraryDbContext.MyObjectCollection.<Selecteurs>.ToList()
        // Pour faire une requète avec filtre:
        //   - libraryDbContext.MyObjectCollection.<Selecteurs>.Skip().<Selecteurs>
        //   - libraryDbContext.MyObjectCollection.<Selecteurs>.Take().<Selecteurs>
        //   - libraryDbContext.MyObjectCollection.<Selecteurs>.Where(x => x == y).<Selecteurs>
        // Pour récupérer une 2nd table depuis la base:
        //   - .Include(x => x.yyyyy)
        //     ou yyyyy est la propriété liant a une autre table a récupéré
        //
        // Exemple:
        //   - Ex: libraryDbContext.MyObjectCollection.Include(x => x.yyyyy).Where(x => x.yyyyyy.Contains(z)).Skip(i).Take(j).ToList()

        // DTOs
        // transformation "à la main":
        //      my_array.Select(item => new ItemDto() { prop1 = item.prop1, prop2 = item.prop2, ... })
        // transformation avec AutoMapper
        //      Rajouter le mapping dans MappingProfile.cs
        //      this.mapper.Map<List<ItemDto>>(my_array);

        // Je vous montre comment faire la 1er, a vous de la compléter et de faire les autres !

    }
}

