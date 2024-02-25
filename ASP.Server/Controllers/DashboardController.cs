using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ASP.Server.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ASP.Server.ViewModels;
using AutoMapper;
using ASP.Server.Database;

namespace ASP.Server.Controllers
{
    public class DashboardController(ILogger<DashboardController> logger, LibraryDbContext libraryDbContext, IMapper mapper) : Controller
    {
        private readonly ILogger<DashboardController> _logger = logger;
        private readonly IMapper mapper = mapper;

        public IActionResult Dashboard()
        {
            //********** Total books **********
            int totalBooks = libraryDbContext.Books.Count();
            ViewData["TotalBooks"] = totalBooks;


            //********** Number of books per author **********
            var booksPerAuthor = libraryDbContext.Author
            .Select(author => new
            {
                AuthorName = author.FullName,
                BookCount = author.Books.Count()
            })
            .ToList();
            ViewData["BooksPerAuthor"] = booksPerAuthor;

            //********** Number of books per genre **********
            var booksPerGenre = libraryDbContext.Genre
            .Select(genre => new
            {
                GenreName = genre.Name,
                BookCount = genre.Books.Count()
            })
            .ToList();
            ViewData["booksPerGenre"] = booksPerGenre;

            //********** The maximum, minimum, median and average number of words in a book **********
            var allBooks = libraryDbContext.Books.ToList();

            // Calculate statistics
            int maxWords = allBooks.Max(b => b.Content.Split(' ').Length);
            int minWords = allBooks.Min(b => b.Content.Split(' ').Length);
            double averageWords = allBooks.Average(b => b.Content.Split(' ').Length);
            int medianWords = CalculateMedian(allBooks.Select(b => b.Content.Split(' ').Length));

            // Pass statistics to the view
            ViewData["MaxWords"] = maxWords;
            ViewData["MinWords"] = minWords;
            ViewData["AverageWords"] = averageWords;
            ViewData["MedianWords"] = medianWords;

            return View();
        }
        private int CalculateMedian(IEnumerable<int> values)
        {
            var sortedValues = values.OrderBy(v => v).ToList();
            int count = sortedValues.Count;
            if (count % 2 == 0)
            {
                return (sortedValues[count / 2 - 1] + sortedValues[count / 2]) / 2;
            }
            else
            {
                return sortedValues[count / 2];
            }
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
