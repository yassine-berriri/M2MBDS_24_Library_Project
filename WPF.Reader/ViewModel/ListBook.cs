using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using WPF.Reader.Model;
using WPF.Reader.Service;
using System.Windows;
using System.Collections.Generic;
using Windows.Web.Http;
using System.Threading.Tasks;
using System;
using System.Windows.Input;
using System.Windows.Controls;

namespace WPF.Reader.ViewModel
{
    public partial class ListBook : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [RelayCommand]
        public void ItemSelected(Book book)
        {
            Ioc.Default.GetRequiredService<INavigationService>().Navigate<DetailsBook>(book);
        }

        

        private RelayCommand<SelectionChangedEventArgs> selectionChangedCommand;
        public ICommand SelectionChangedCommand => selectionChangedCommand ??= new RelayCommand<SelectionChangedEventArgs>(SelectionChanged);

        private void SelectionChanged(SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var selectedBook = e.AddedItems[0] as Book;
                if (selectedBook != null)
                {
                    // Faites quelque chose avec selectedBook
                    Ioc.Default.GetRequiredService<INavigationService>().Navigate<DetailsBook>(selectedBook);
                }
            }
        }

        private int _selectedInput;
        private int selectedLimit;

        public List<int> InputOptions { get; } = new List<int> { 10, 20, 30, 40, 50};
        public int SelectedLimit 
        {
            get => selectedLimit; set
            {
                selectedLimit = value;
                UpdateBooks();
            }
        }


        private RelayCommand nextButtonCommand;
        public ICommand NextButtonCommand => nextButtonCommand ??= new RelayCommand(buttonNextClicked, canPlayNext);

        private RelayCommand previousButtonCommand;
        public ICommand PreviousButtonCommand => previousButtonCommand ??= new RelayCommand(buttonPreviousClicked, canPlayPrevious);
      

        private void buttonNextClicked()
        {
            incrementOffset();
            canPlayNext();
            canPlayPrevious();
            ((RelayCommand)NextButtonCommand).NotifyCanExecuteChanged();
            ((RelayCommand)PreviousButtonCommand).NotifyCanExecuteChanged();
        }

        private bool canPlayNext()
        {
         
            if (_offset> 100)
            {
                return false;
            }
            else
            {
                return true;
            }

          
        }

        private bool canPlayPrevious()
        {
           
            if (_offset == 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        
        }

        private void buttonPreviousClicked()
        {
            decrementOffset();
            canPlayNext();
            canPlayPrevious();
            ((RelayCommand)NextButtonCommand).NotifyCanExecuteChanged();
            ((RelayCommand)PreviousButtonCommand).NotifyCanExecuteChanged();
        }

        private int _offset = 10;
        public int Offset
        {
            get => _offset; set
            {
                _offset = value;
                //UpdateBooks();
              

                // Notifiez l'interface utilisateur que l'état activé des commandes peut avoir changé
               
            }
        }

       public void incrementOffset()
        {
            _offset += SelectedLimit;
            UpdateBooks();
        }

        public void decrementOffset()
        {
            _offset -= SelectedLimit;

            if (_offset < 0)
            {
                _offset = 0;
                  
            }
               

            UpdateBooks();
        }
        
        

        void UpdateBooks()
        {
            Task.Run(async () =>
            {   
                if (_offset >= 0 && _offset < 100)
                {
                    await Ioc.Default.GetRequiredService<LibraryService>().FetchBookByIdAndPaginate(null, SelectedLimit, _offset);
                }
           
            });
        }

        Collection<int> selectedGenresInt = new Collection<int>();

        private RelayCommand<SelectionChangedEventArgs> selectionChangedGenreCommand;
        public ICommand SelectionChangedGenreCommand => selectionChangedGenreCommand ??= new RelayCommand<SelectionChangedEventArgs>(SelectionChangedGenre);

        private void SelectionChangedGenre(SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                Task.Run(async () =>
                {   
                    for (int i = 0; i < e.AddedItems.Count; i++)
                    {
                           var test = e.AddedItems[i] as Genre;

                        if (test != null)
                        {
                            selectedGenresInt.Add(test.Id);
                        }
                    }
                    
                   // var selectedGenre = e.AddedItems[0] as Genre;
                    if (selectedGenresInt != null)
                    {
                        await Ioc.Default.GetRequiredService<LibraryService>().FetchBookByIdAndPaginate(selectedGenresInt, null, null);
                    }
                });
            }
        }

        // n'oublier pas faire de faire le binding dans ListBook.xaml !!!!
        public ObservableCollection<Book> Books => Ioc.Default.GetRequiredService<LibraryService>().Books;
        public ObservableCollection<Genre> Genres => Ioc.Default.GetRequiredService<LibraryService>().Genres;
    }
}
