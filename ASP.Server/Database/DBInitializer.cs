using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ASP.Server.Models;
using System.Xml.Linq;

namespace ASP.Server.Database
{
    public class DbInitializer
    {
        public static void Initialize(LibraryDbContext bookDbContext)
        {
        
            // Create Genres
            var genres = new List<Genre>
            {
                new Genre { Name = "Science Fiction" },
                new Genre { Name = "Classic" },
                new Genre { Name = "Romance" },
                new Genre { Name = "Thriller" }
            };

            bookDbContext.Genre.AddRange(genres);
            bookDbContext.SaveChanges();

            // Create Authors
            var authors = new List<Author>();
            for (int i = 0; i < 20; i++) // Increased the number of authors
            {
                authors.Add(new Author { FullName = $"Author {i}", Email = $"author{i}@example.com", Birthday = new DateOnly(1970 + i, 1, 1) });
            }

            bookDbContext.Author.AddRange(authors);
            bookDbContext.SaveChanges();

            // Create Books with multiple Authors
            var random = new Random();
            var books = new List<Book>();

            for (int i = 0; i < 100; i++)
            {
                var book = new Book
                {
                    Name = $"Book {i}",
                    Content = $"Content of Book {i}, There once lived a poor\r\ntailor, who had a son called\r\nAladdin, a careless, idle boy\r\nwho would do nothing but\r\nplay ball all day long in the\r\nstreets with little idle boys\r\nlike himself. This so grieved\r\nthe father that he died; yet,\r\nin spite of his mother’s tears\r\nand prayers, Aladdin did not mend his ways.\r\nOne day, " +
                    $"when he was playing in the streets as\r\nusual, a stranger asked him his age, and if he\r\nwas not the son of Mustapha the tailor. However, she prepared supper,\r\nand bade Aladdin seek his uncle, who came\r\nladen with wine and fruit. He presently fell\r\ndown and kissed the place where Mustapha\r\nused to sit," +
                    $" bidding Aladdin’s mother not to\r\nbe surprised at not having seen him before,\r\nas he had been forty years out of the country.\r\nHe then turned to Aladdin, and asked him\r\nhis trade, at which the boy hung his head,\r\nwhile his mother burst into " +
                    $"tears However, she prepared supper,\r\nand bade Aladdin seek his uncle, who came\r\nladen with wine and fruit. He presently fell\r\ndown and kissed the place where Mustapha\r\nused to sit, bidding Aladdin’s mother not to\r\nbe surprised at not having seen him before,\r\nas he had been forty years out of the country.\r\nHe then turned to Aladdin, and asked him\r\nhis trade, at which the boy hung his head,\r\nwhile his mother burst into tears",
                    Price = random.NextDouble() * 50,
                    Genres = new List<Genre> { genres[random.Next(genres.Count)] },
                    Authors = new List<Author>()
                    {
                        authors[random.Next(authors.Count)]
                    }

                };  

                bookDbContext.Books.AddRange(book);
                bookDbContext.SaveChanges();
            }

            

        }
        
    }
}