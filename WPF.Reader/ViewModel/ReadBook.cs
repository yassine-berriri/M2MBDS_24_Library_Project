using CommunityToolkit.Mvvm.DependencyInjection;
using System.ComponentModel;
using WPF.Reader.Model;
using WPF.Reader.Service;
using System.Speech.Synthesis;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Controls;

namespace WPF.Reader.ViewModel
{
    public partial class ReadBook : INotifyPropertyChanged
    {
        private readonly LibraryService _libraryService;
        private Book _book;
        private SpeechSynthesizer synth = new SpeechSynthesizer();

        public ICommand PlayCommand { get; }
        public ICommand PauseCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ReadBook(int id)
        {
            _libraryService = Ioc.Default.GetRequiredService<LibraryService>();
            LoadBook(id);

            PlayCommand = new RelayCommand(PlayText, CanPlayText);
            PauseCommand = new RelayCommand(PauseSpeech, CanPauseSpeech);

            synth.SetOutputToDefaultAudioDevice();
            synth.StateChanged += Synth_StateChanged;
        }


        public ReadBook()
        {
            PlayCommand = new RelayCommand(PlayText, CanPlayText);
            PauseCommand = new RelayCommand(PauseSpeech, CanPauseSpeech);

            synth.SetOutputToDefaultAudioDevice();
            synth.StateChanged += Synth_StateChanged;
        }

        private void Synth_StateChanged(object sender, StateChangedEventArgs e)
        {
            // Met à jour l'état des commandes
            ((RelayCommand)PlayCommand).NotifyCanExecuteChanged();
            ((RelayCommand)PauseCommand).NotifyCanExecuteChanged();
        }

        public Book Book
        {
            get => _book;
            set
            {
                if (_book != value)
                {
                    _book = value;
                    OnPropertyChanged(nameof(Book));
                }
            }
        }

        private async void LoadBook(int id)
        {
            var book = await _libraryService.FetchBookById(id);
            if (book != null)
            {
                Book = book;
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool CanPlayText()
        {
            return synth.State != SynthesizerState.Speaking;
        }

        private bool CanPauseSpeech()
        {
            return synth.State == SynthesizerState.Speaking;
        }

        private void PlayText()
        {
            if (Book != null && !string.IsNullOrWhiteSpace(Book.Content))
            {
                if (synth.State == SynthesizerState.Paused)
                {
                    synth.Resume();
                }
                else
                {
                    synth.SpeakAsync(Book.Content);
                }
            }
        }

        private void PauseSpeech()
        {
            if (synth.State == SynthesizerState.Speaking)
            {
                synth.Pause();
            }
        }

        public void ReadFromSelection(string selectedText)
        {
            if (!string.IsNullOrWhiteSpace(selectedText))
            {
                synth.SpeakAsyncCancelAll(); // Arrêtez toute lecture en cours
                synth.SpeakAsync(selectedText);
            }
        }


    }
}