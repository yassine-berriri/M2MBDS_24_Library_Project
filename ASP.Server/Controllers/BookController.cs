using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASP.Server.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ASP.Server.Models;
using ASP.Server.ViewModels;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Cryptography.Xml;
using System.Security.Policy;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

using X.PagedList;



namespace ASP.Server.Controllers
{
    public class BookController(LibraryDbContext libraryDbContext, IMapper mapper) : Controller
    {
        private readonly LibraryDbContext libraryDbContext = libraryDbContext;
        private readonly IMapper _mapper;

        public ActionResult List(int? authorId, int? genreId, int? page)
        {
           
            var books = libraryDbContext.Books
                .Include(b => b.Genres)
                .Include(a => a.Authors)
                .ToList();
            ViewBag.Authors = libraryDbContext.Author.ToList();
            ViewBag.Genres = libraryDbContext.Genre.ToList();

         
            if (authorId != null)
            {
                books = books.Where(b => b.Authors.Any(a => a.Id == authorId)).ToList();
            }
            if (genreId != null)
            {
                books = books.Where(b => b.Genres.Any(g => g.Id == genreId)).ToList();
            }

            // Pagination
            int pageNumber = page ?? 1; 
            int pageSize = 10; 
         
            var pagedList = books.ToPagedList(pageNumber, pageSize);

           
            ViewBag.TotalItemCount = books.Count();
            ViewBag.PageCount = (int)Math.Ceiling((double)ViewBag.TotalItemCount / pageSize);

            
            return View(pagedList);
        }



        public ActionResult<CreateBookViewModel> Create(CreateBookViewModel book)
        {
            // Le IsValid est True uniquement si tous les champs de CreateBookModel marqués Required sont remplis
            if (ModelState.IsValid)
            {
               
                Book myBook = new Book() { Name = book.Name, Price = book.Price, Content = book.Content ,Genres = [], Authors = [] };
              

                foreach (var genreId in book.Genres)
                {
                    var genre = libraryDbContext.Genre.Find(genreId);
                    if (genre != null)
                    {
                        myBook.Genres.Add(genre);
                    }
                }
                foreach (var authorId in book.Authors)
                {
                    var author = libraryDbContext.Author.Find(authorId);
                    if (author != null)
                    {
                        myBook.Authors.Add(author);
                    }
                }
                libraryDbContext.Add(myBook);
                libraryDbContext.SaveChanges();
                return RedirectToAction("List");

            }

            return View(new CreateBookViewModel() { AllGenres = libraryDbContext.Genre, AllAuthors = libraryDbContext.Author });
        }

        public ActionResult<UpdateBookViewModel> Update(int id)
        {
            //get current book
            var book = libraryDbContext.Books.Include(b => b.Genres).Include(a => a.Authors).FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return NotFound(); 
            }
            List<int> Genres = new List<int>();

            foreach (var genre in book.Genres)
            {
                    Genres.Add(genre.Id);
                
            }
            ViewBag.myGenres = Genres;

            List<int> Authors = new List<int>();

            if (book.Authors != null)
            foreach (var author in book.Authors)
            {
                Authors.Add(author.Id);

            }
            ViewBag.myAuthors = Authors;
            //get genere
            var allGenres = libraryDbContext.Genre.ToList();
            var allAuthors = libraryDbContext.Author.ToList();

            //add to ciew model
            var viewModel = new UpdateBookViewModel
            {
                Id = book.Id,
                Name = book.Name,
                Content = book.Content,
                Price = book.Price,
                AllGenres = allGenres,
                AllAuthors = allAuthors
            };
            viewModel.AllGenres = libraryDbContext.Genre.ToList();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Update(UpdateBookViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.Name == null)
                {
                    return View(List);
                }
                //get current book
                var bookToUpdate = libraryDbContext.Books.Include(b => b.Genres).Include(a => a.Authors).FirstOrDefault(b => b.Id == viewModel.Id);

                if (bookToUpdate == null)
                {
                    return View(List);
                }

                //update book
                bookToUpdate.Name = viewModel.Name;
                bookToUpdate.Price = viewModel.Price;
                bookToUpdate.Content = viewModel.Content;

                //update genre
                bookToUpdate.Genres.Clear();
                if (bookToUpdate.Authors != null) { 
                bookToUpdate.Authors.Clear();
                }
                foreach (var genreId in viewModel.SelectedGenres)
                {
                    var genre = libraryDbContext.Genre.Find(genreId);
                    if (genre != null)
                    {
                        bookToUpdate.Genres.Add(genre);
                    }
                }
                if (viewModel.SelectedAuthors != null)
                    foreach (var authorId in viewModel.SelectedAuthors)
                {
                    var author = libraryDbContext.Author.Find(authorId);
                    if (author != null)
                    {
                        bookToUpdate.Authors.Add(author);
                    }
                }
                //save
                libraryDbContext.SaveChanges();

                return RedirectToAction("List");
            }
            var allAuthors = libraryDbContext.Author.ToList();

            viewModel.AllGenres = libraryDbContext.Genre.ToList();
        //    viewModel.AllAuthors = libraryDbContext.Author.ToList();
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var book = libraryDbContext.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            libraryDbContext.Books.Remove(book);
            libraryDbContext.SaveChanges();

            return RedirectToAction("List");
        }

        public async Task<ActionResult<CreateBookApiViewModel>> CreateFromApiAsync(CreateBookApiViewModel book)
        {
            if (ModelState.IsValid)
            {
       
                var isbn = book.ISBN;
                var apiUrl = $"https://openlibrary.org/api/books?bibkeys=ISBN:{isbn}&jscmd=data&format=json";

                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        var response = await httpClient.GetAsync(apiUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            var testExist = libraryDbContext.Books.Include(b => b.Genres).Include(a => a.Authors).FirstOrDefault(b => b.Id.ToString()== isbn);

                            if (testExist != null)
                            {
                                ViewBag["ErrorMessage"] = "Your error message here";
                                return RedirectToAction("CreateFromApi");

                            }
                            var responseContent = await response.Content.ReadAsStringAsync();
                            JObject bookInfo = JObject.Parse(responseContent);
                            if (bookInfo.Count == 0)
                            {
                                ModelState.AddModelError(string.Empty, "ISBN invalide ");

                                return View();

                            }


                            // Get title
                            string title = (string)bookInfo[$"ISBN:{isbn}"]["title"];

                            // Get content
                            string content = (string)bookInfo[$"ISBN:{isbn}"]["excerpts"]?.FirstOrDefault()?["text"];



                            // Get authors
                            var authorsArray = bookInfo[$"ISBN:{isbn}"]["authors"];
                            List<string> authors = authorsArray.Select(a => (string)a["name"]).ToList();

                            var subjectsArray = bookInfo[$"ISBN:{isbn}"]["subjects"];
                            List<string> subjects = subjectsArray.Select(s => (string)s["name"]).ToList();

                            Console.WriteLine("Title: " + title);
                            Console.WriteLine("Content: " + content);
                            Console.WriteLine("Authors: " + string.Join(", ", authors));
                            Console.WriteLine("Subjects: " + string.Join(", ", subjects));

                            Console.WriteLine(responseContent);
                            IEnumerable<Genre> ListGenre = libraryDbContext.Genre.ToList();
                            List<int> Genres = new List<int>();
                            List<int> Authors = new List<int>();

                            if (subjects.Count > 0)
                            {
                                foreach (var subject in subjects)
                                {

                                    Genre isExist = libraryDbContext.Genre.FirstOrDefault(g => g.Name == subject);

                                    if (isExist == null)
                                    {
                                        libraryDbContext.Add(new Genre() { Name = subject });
                                        libraryDbContext.SaveChanges();
                                    }
                                }


                                foreach (var subject in subjects)
                                {
                                    Genre isExist = libraryDbContext.Genre.FirstOrDefault(g => g.Name == subject);

                                    if (isExist != null)
                                    {
                                        Genres.Add(isExist.Id);
                                    }
                                }
                            }

                            /*
                            if (authors.Count > 0)
                            {
                                foreach (var author in authors)
                                {

                                    Author isExist = libraryDbContext.Author.FirstOrDefault(g => g.FullName == author );

                                    if (isExist == null)
                                    {
                                        libraryDbContext.Add(new Author() { FullName = author ,Email = "", Birthday = new DateOnly(1990, 1, 1) , Bio = "", Books = [] });
                                        libraryDbContext.SaveChanges();
                                    }
                                }
                            }
                            */
                                //author
                                
                                if (authors.Count > 0)
                                {
                                    foreach (var author in authors)
                                    {

                                        Author isExist = libraryDbContext.Author.FirstOrDefault(g => g.FullName == author);

                                        if (isExist == null)
                                        {
                                        libraryDbContext.Add(new Author() {FullName = author, Email = "", Birthday = new DateOnly(1990, 1, 1), Bio = "", Books = [] });
                                        libraryDbContext.SaveChanges();
                                        }
                                    }


                                    foreach (var author in authors)
                                    {
                                        Author isExist = libraryDbContext.Author.FirstOrDefault(g => g.FullName == author);

                                        if (isExist != null)
                                        {
                                            Authors.Add(isExist.Id);
                                        }
                                    }
                                }
                                
                                Book myBook = new Book() { Name = title, Price = 0, Content = book.Content, Genres = [], Authors = [] };


                            foreach (var genreId in Genres)
                            {
                                var genre = libraryDbContext.Genre.Find(genreId);
                                if (genre != null)
                                {
                                    myBook.Genres.Add(genre);
                                }
                            }


                            if (Authors.Count > 0) 
                            { 
                                foreach (var authorId in Authors)
                                {
                                    var author = libraryDbContext.Author.Find(authorId);
                                    if (author != null)
                                    {
                                        myBook.Authors.Add(author);
                                    }
                                } 
                            }
                            libraryDbContext.Add(myBook);
                            libraryDbContext.SaveChanges();
                            return RedirectToAction("List");


                        }
                        else
                        {
                            Console.WriteLine($"Failed to fetch data. Status code: {response.StatusCode}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                }

                }

            return View(new CreateBookApiViewModel() );
        }


    }
}
