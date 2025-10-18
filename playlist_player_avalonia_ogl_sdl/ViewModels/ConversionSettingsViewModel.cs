using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;

public class ConversionSettingsViewModel : INotifyPropertyChanged
{
    private ConversionSettings _settings = new();
    private bool _isPlaying = false;

    public bool IsPlaying
    {
        get => _isPlaying;
        set
        {
            _isPlaying = value;
            OnPropertyChanged();
        }
    }

    public int AudioChannels
    {
        get => _settings.AudioFormat.Channels;
        set
        {
            if (value >= 0)
            {
                _settings.AudioFormat.Channels = value;
                OnPropertyChanged();
            }
        }
    }

    public int AudioSampleRate
    {
        get => _settings.AudioFormat.SampleRate;
        set
        {
            if (value >= 0)
            {
                _settings.AudioFormat.SampleRate = value;
                OnPropertyChanged();
            }
        }
    }

    public string VideoFieldOrder
    {
        get => _settings.VideoFormat.FieldOrder;
        set
        {
            _settings.VideoFormat.FieldOrder = value;
            OnPropertyChanged();
        }
    }

    public double VideoFrameRate
    {
        get => _settings.VideoFormat.FrameRate;
        set
        {
            if (value > 0)
            {
                _settings.VideoFormat.FrameRate = value;
                OnPropertyChanged();
            }
        }
    }

    public int VideoHeight
    {
        get => _settings.VideoFormat.Height;
        set
        {
            _settings.VideoFormat.Height = value;
            OnPropertyChanged();
        }
    }

    public string VideoPixelFormat
    {
        get => _settings.VideoFormat.PixelFormat;
        set
        {
            _settings.VideoFormat.PixelFormat = value;
            OnPropertyChanged();
        }
    }

    public int VideoWidth
    {
        get => _settings.VideoFormat.Width;
        set
        {
            _settings.VideoFormat.Width = value;
            OnPropertyChanged();
        }
    }

    public static ConversionSettingsViewModel FromJson(string json)
    {
        try
        {
            var settings = JsonSerializer.Deserialize<ConversionSettings>(json);
            if (settings != null)
            {
                return new ConversionSettingsViewModel
                {
                    _settings = settings
                };
            }
        }
        catch
        {
            // In case of deserialization error, we use default values
        }

        return new ConversionSettingsViewModel();
    }

    public ConversionSettingsViewModel() { }

    public string GenerateJson()
    {
        return JsonSerializer.Serialize(_settings, new JsonSerializerOptions { WriteIndented = true });
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}