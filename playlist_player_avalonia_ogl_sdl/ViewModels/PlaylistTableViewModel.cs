using Avalonia;
using Avalonia.Collections;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
public class PlaylistTableViewModel : INotifyPropertyChanged
{
    private AvaloniaList<PlaylistItem> _playlistItems;

    public AvaloniaList<PlaylistItem> PlaylistItems
    {
        get => _playlistItems;
        set
        {
            _playlistItems = value;
            Debug.WriteLine($"PlaylistItems set, count: {_playlistItems?.Count ?? 0}");
            OnPropertyChanged(nameof(PlaylistItems));
        }
    }

    private string _playlistBackground;

    public string PlaylistBackground
    {
        get => _playlistBackground;
        set
        {
            _playlistBackground = value;
            Debug.WriteLine($"PlaylistBackground set: {_playlistBackground ?? "None"}");
            OnPropertyChanged(nameof(PlaylistBackground));
        }
    }

    public PlaylistTableViewModel()
    {
        _playlistItems = new AvaloniaList<PlaylistItem>();
        _playlistBackground = string.Empty;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public virtual void OnPropertyChanged(string propertyName)
    {
        Debug.WriteLine($"OnPropertyChanged called: {propertyName}");
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
