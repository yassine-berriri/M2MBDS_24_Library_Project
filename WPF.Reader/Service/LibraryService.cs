using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media.Media3D;
using Windows.Web.Http;
using WPF.Reader.Model;
using WPF.Reader.OpenApi;

namespace WPF.Reader.Service
{
    public class LibraryService
    {
        // A remplacer avec vos propre données !!!!!!!!!!!!!!
        // Pensé qu'il ne faut mieux ne pas réaffecter la variable Books, mais juste lui ajouter et / ou enlever des éléments
        // Donc pas de LibraryService.Instance.Books = ...
        // mais plutot LibraryService.Instance.Books.Add(...)
        // ou LibraryService.Instance.Books.Clear()
        /*public ObservableCollection<Book> Books { get; set; } = new ObservableCollection<Book>() {
            new Book(),
            new Book(),
            new Book()
        };
        */

        public ObservableCollection<Book> Books { get; set; } = new ObservableCollection<Book>();

        public ObservableCollection<Genre> Genres { get; set; } = new ObservableCollection<Genre>();
        public Book Book { get; set; } = new Book();


        private HttpClient _httpClient = new HttpClient();

        private Uri _urlGlobal = new Uri("http://localhost:5001/api/Book/");


        // C'est aussi ici que vous ajouterez les requête réseau pour récupérer les livres depuis le web service que vous avez fait
        // Vous pourrez alors ajouter les livres obtenu a la variable Books !
        // Faite bien attention a ce que votre requête réseau ne bloque pas l'interface 
        public LibraryService()
        {

            //  _httpClient.BaseAddress = new Uri("http://localhost:5001/api/Book/GetBooksP?genreIds=2&limit=10&offset=0");
            ///GetBooks?genres=2&limit=10&offset=0
            FetchBookByIdAndPaginate(null, 10, 0);
            FetchGenres();

        }

        public LibraryService(Collection<int> id, int? limit, int? offset)
        {

            //  _httpClient.BaseAddress = new Uri("http://localhost:5001/api/Book/GetBooksP?genreIds=2&limit=10&offset=0");
            ///GetBooks?genres=2&limit=10&offset=0
            FetchBookByIdAndPaginate(id, limit, offset);
        }

        public async Task FetchBooksAsync()
        {
            try
            {
                var endpoint = new Uri(_urlGlobal, "GetBooks");

                // Effectuer la requête GET de manière asynchrone
                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();

                // Lire la réponse comme une chaîne de caractères
                var content = await response.Content.ReadAsStringAsync();

                // Désérialiser le JSON en une liste de livres
                var booksFromService = JsonConvert.DeserializeObject<List<Book>>(content);

                // Assurez-vous d'exécuter les modifications de la collection sur le thread UI
                App.Current.Dispatcher.Invoke(() =>
                {
                    // Ajouter les livres récupérés à la collection
                    foreach (var book in booksFromService)
                    {
                        Books.Add(book);
                    }
                });
            }
            catch (Exception ex)
            {
                // Gérer les erreurs (ex: log, afficher un message à l'utilisateur)
                Console.WriteLine($"Une erreur est survenue lors de la récupération des livres: {ex.Message}");
            }
        }


        public async Task<Book> FetchBookById(int id)
        {
            try
            {
                var endpoint = new Uri(_urlGlobal, $"GetBook?id={id}");

                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var bookFromService = JsonConvert.DeserializeObject<Book>(content);

                return bookFromService; // Retourner le livre directement
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur est survenue lors de la récupération du livre: {ex.Message}");
                return null; // Retourner null ou gérer autrement en cas d'erreur
            }
        }


        public async Task FetchBookByIdAndPaginate(Collection<int> genres, int? limit, int? offset)
        {
            try
            {
                StringBuilder endpointBuilder = new StringBuilder(_urlGlobal.AbsoluteUri + "GetBooks");

                if (genres != null )
                {
                    foreach (var genre in genres)
                    {
                        endpointBuilder.Append(endpointBuilder.ToString().Contains("?") ? "&" : "?");
                        endpointBuilder.Append($"genres={genre}");
                    }
                }

                if (limit != null)
                {
                    endpointBuilder.Append(endpointBuilder.ToString().Contains("?") ? "&" : "?");
                    endpointBuilder.Append($"limit={limit}");
                }

                if (offset != null)
                {
                    endpointBuilder.Append(endpointBuilder.ToString().Contains("?") ? "&" : "?");
                    endpointBuilder.Append($"offset={offset}");
                }

                Uri endpoint = new Uri(endpointBuilder.ToString());
                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var booksFromService = JsonConvert.DeserializeObject<List<Book>>(content);

                App.Current.Dispatcher.Invoke(() =>
                {
                    Books.Clear();
                    foreach (var book in booksFromService)
                    {
                        Books.Add(book);
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur est survenue lors de la récupération du livre: {ex.Message}");
            }
        }


        public async Task FetchGenres()
        {
            try
            {
                var response = await _httpClient.GetAsync(new Uri("http://localhost:5001/api/Book/GetGenres"));
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var genresFromService = JsonConvert.DeserializeObject<List<Genre>>(content);

                App.Current.Dispatcher.Invoke(() =>
                {
                    Genres.Clear();
                    foreach (var genre in genresFromService)
                    {
                        Genres.Add(genre);
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur est survenue lors de la récupération du genre: {ex.Message}");
            }
        }


    }
}