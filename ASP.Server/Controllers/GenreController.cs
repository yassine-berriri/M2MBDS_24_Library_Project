using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASP.Server.Database;
using ASP.Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ASP.Server.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;

namespace ASP.Server.Controllers
{
    public class GenreController(LibraryDbContext libraryDbContext, IMapper mapper) : Controller
    {
        private readonly LibraryDbContext libraryDbContext = libraryDbContext;
        private readonly IMapper mapper = mapper;


        public object Name { get; private set; }

        public ActionResult<IEnumerable<Genre>> List(int? page)
        {
     
            IEnumerable<Genre> ListGenres = libraryDbContext.Genre;
            int pageNumber = page ?? 1;
            int pageSize = 10;
            var pagedList = ListGenres.ToPagedList(pageNumber, pageSize);
            ViewBag.TotalItemCount = ListGenres.Count();
            ViewBag.PageCount = (int)Math.Ceiling((double)ViewBag.TotalItemCount / pageSize);

            return View(pagedList);
        }
        public ActionResult<CreateGenreViewModel> Create(CreateGenreViewModel genre)
        {
            // Le IsValid est True uniquement si tous les champs de CreateBookModel marqués Required sont remplis
            if (ModelState.IsValid)
            {
             
                libraryDbContext.Add(new Genre() { Name = genre.Name });
                libraryDbContext.SaveChanges();

                // Redériger vers la liste des genres
                return RedirectToAction("List");
            }

            return View(new CreateGenreViewModel() { AllGenres = libraryDbContext.Genre });
        }

        [HttpPost]
        public ActionResult Delete(List<int> selectedGenres)
        {
            if (selectedGenres != null && selectedGenres.Any())
            {
                foreach (var genreId in selectedGenres)
                {
                    Genre genreToDelete = libraryDbContext.Genre.Find(genreId);
                    if (genreToDelete != null)
                    {
                        libraryDbContext.Genre.Remove(genreToDelete);
                    }
                }
                libraryDbContext.SaveChanges();
            }

            return RedirectToAction("List");
        }
    }


}



