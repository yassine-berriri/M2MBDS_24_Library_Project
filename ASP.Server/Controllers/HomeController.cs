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
    public class HomeController(ILogger<HomeController> logger, LibraryDbContext libraryDbContext, IMapper mapper) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly IMapper mapper = mapper;

        public IActionResult Index()
        {
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
