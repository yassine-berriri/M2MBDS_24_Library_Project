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
using X.PagedList;

namespace ASP.Server.Controllers
{
    public class AuthorController(LibraryDbContext libraryDbContext, IMapper mapper) : Controller
    {
        private readonly LibraryDbContext libraryDbContext = libraryDbContext;


        public ActionResult<IEnumerable<Author>> List(int? page)
        {
            // récupérer les autheurs dans la base de donées pour qu'elle puisse être affiché
            IEnumerable<Author> ListAuthors = libraryDbContext.Author
             .Include(b => b.Books)
             .ToList();
            int pageNumber = page ?? 1;
            int pageSize = 10;
            var pagedList = ListAuthors.ToPagedList(pageNumber, pageSize);
            ViewBag.TotalItemCount = ListAuthors.Count();
            ViewBag.PageCount = (int)Math.Ceiling((double)ViewBag.TotalItemCount / pageSize);

            return View(pagedList);
        }
        public ActionResult<CreateAuthorViewModel> Create(CreateAuthorViewModel author)
        {
            // Le IsValid est True uniquement si tous les champs de CreateBookModel marqués Required sont remplis
            if (ModelState.IsValid)
            {

                Author newAuthor = new Author() { FullName = author.FullName, Email = author.Email, Birthday = author.Birthday, Bio = author.Bio, Books = [] };
                    foreach (var bookId in author.Books)
                        {
                            var book = libraryDbContext.Books.Find(bookId);
                            if (book != null)
                            {
                                newAuthor.Books.Add(book);
                            }
                        }

                libraryDbContext.Add(newAuthor);
                libraryDbContext.SaveChanges();
                return RedirectToAction("List");
            }

            return View(new CreateAuthorViewModel() { AllAuthors = libraryDbContext.Author, AllBooks = libraryDbContext.Books });
        }

        [HttpPost]
        public ActionResult Delete(List<int> selectedAuthors)
        {
            if (selectedAuthors != null && selectedAuthors.Any())
            {
                foreach (var authorId in selectedAuthors)
                {
                    Author authorToDelete = libraryDbContext.Author.Find(authorId);
                    if (authorToDelete != null)
                    {
                        libraryDbContext.Author.Remove(authorToDelete);
                    }
                }
                libraryDbContext.SaveChanges();
            }

            return RedirectToAction("List");
        }




        public ActionResult<UpdateAuthorViewModel> Update(int id)
        {
            //get current book
            var author = libraryDbContext.Author.Include(b => b.Books).FirstOrDefault(b => b.Id == id);

            if (author == null)
            {
                return NotFound();
            }

            //get genere
            var allBooks = libraryDbContext.Books.ToList();

            //add to ciew model
            var viewModel = new UpdateAuthorViewModel
            {
                Id = author.Id,
                FullName = author.FullName,
                Birthday = author.Birthday,
                Email = author.Email,
                Bio = author.Bio,
                AllBooks = allBooks,
            };
            List<int> Books = new List<int>();

            foreach (var book in author.Books)
            {
                Books.Add(book.Id);

            }
            ViewBag.myBooks = Books;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Update(UpdateAuthorViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                //get current book
                var authorToUpdate = libraryDbContext.Author.Include(b => b.Books).FirstOrDefault(b => b.Id == viewModel.Id);

                if (authorToUpdate == null)
                {
                    return NotFound();
                }

                //current author
                authorToUpdate.FullName = viewModel.FullName;
                authorToUpdate.Email = viewModel.Email;
                authorToUpdate.Bio = viewModel.Bio;
                authorToUpdate.Birthday = viewModel.Birthday;

                if (viewModel.SelectedBooks != null) 
                foreach (var bookId in viewModel.SelectedBooks)
                {
                    var book = libraryDbContext.Books.Find(bookId);
                    if (book != null)
                    {
                        authorToUpdate.Books.Add(book);
                    }
                }
                else
                {
                    authorToUpdate.Books.Clear();
                }
                //save
                libraryDbContext.SaveChanges();

                return RedirectToAction("List");
            }
            var allAuthors = libraryDbContext.Author.ToList();

            //    viewModel.AllAuthors = libraryDbContext.Author.ToList();
            return View(viewModel);
        }
    }


}

   
