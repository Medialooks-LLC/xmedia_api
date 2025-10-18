using Avalonia.Collections;
using System.ComponentModel;

namespace OpenGLAvalonia.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private PlaylistTableViewModel _playlistViewModel;

        public PlaylistTableViewModel PlaylistViewModel
        {
            get => _playlistViewModel;
            set
            {
                _playlistViewModel = value;
                OnPropertyChanged(nameof(PlaylistViewModel));
            }
        }

        public AvaloniaList<PlaylistItem> PlaylistItems { get; set; } = [];
        public MainViewModel()
        {
            _playlistViewModel = new PlaylistTableViewModel();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}