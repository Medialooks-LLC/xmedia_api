using System.Text.Json.Serialization;

public class AudioFormat
{
    [JsonPropertyName("channels")]
    public int Channels { get; set; } = 2;
    [JsonPropertyName("sample_rate")]
    public int SampleRate { get; set; } = 32000;
    [JsonPropertyName("type")]
    public string Type { get; set; } = "Audio";
    [JsonPropertyName("sample_format")]
    public string SampleFormat { get; set; } = "flt";
}

public class VideoFormat
{
    [JsonPropertyName("field_order")]
    public string FieldOrder { get; set; } = "Progressive";

    [JsonPropertyName("frame_rate")]
    public double FrameRate { get; set; } = 50;

    [JsonPropertyName("height")]
    public int Height { get; set; } = 720;

    [JsonPropertyName("pixel_format")]
    public string PixelFormat { get; set; } = "bgra";

    [JsonPropertyName("type")]
    public string Type { get; set; } = "Video";

    [JsonPropertyName("width")]
    public int Width { get; set; } = 1280;
}

public class ConversionSettings
{
    [JsonPropertyName("audio_format")]
    public AudioFormat AudioFormat { get; set; } = new();

    [JsonPropertyName("video_format")]
    public VideoFormat VideoFormat { get; set; } = new();
}