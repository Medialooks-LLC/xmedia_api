using Avalonia.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

// JSON attributes
public class MediaItem
{
    [JsonPropertyName("in_sec")]
    public double? InSec { get; set; }

    [JsonPropertyName("out_sec")]
    public double? OutSec { get; set; }

    [JsonPropertyName("open_url")]
    public string OpenUrl { get; set; } = string.Empty;

    [JsonPropertyName("duration_sec")]
    public double? DurationSec { get; set; }

    [JsonPropertyName("uid")]
    public int Uid { get; set; }
}

public class PlaylistItemJson
{
    [JsonPropertyName("media")]
    public MediaItem Media { get; set; } = new MediaItem();

    [JsonPropertyName("break")]
    public bool? Break { get; set; }

    [JsonPropertyName("fixed_pos_sec")]
    public double? FixedPosSec { get; set; }

    [JsonPropertyName("split_uid")]
    public int? SplitUid { get; set; }
}

public class BackgroundItem
{
    [JsonPropertyName("media")]
    public MediaItem Media { get; set; } = new MediaItem();

    [JsonPropertyName("split_uid")]
    public int? SplitUid { get; set; }
}

public class PlaylistData
{
    [JsonPropertyName("background")]
    public BackgroundItem Background { get; set; } = new BackgroundItem();

    [JsonPropertyName("playlist_items")]
    public List<PlaylistItemJson> PlaylistItems { get; set; } = [];
}

// Parser from JSON
public static class PlaylistParser
{
    public static (AvaloniaList<PlaylistItem>, string) ParsePlaylistFromJson(string json)
    {
        var playlistItems = new AvaloniaList<PlaylistItem>();
        var background = string.Empty;
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        try
        {
            var playlistData = JsonSerializer.Deserialize<PlaylistData>(json, options);

            if (playlistData?.PlaylistItems != null)
            {
                int position = 1;

                foreach (var item in playlistData.PlaylistItems)
                {
                    var playlistItem = new PlaylistItem
                    {
                        Pos = position++,
                        InSec = item.Media?.InSec ?? null,
                        OutSec = item.Media?.OutSec ?? null,
                        OpenUrl = item.Media?.OpenUrl ?? "",
                        IsBreak = item.Break ?? false,
                        FixedPosSec = item.FixedPosSec ?? null
                    };

                    playlistItems.Add(playlistItem);
                }
            }

            if (playlistData?.Background != null)
            {
                background = playlistData?.Background?.Media?.OpenUrl ?? "";
            }
        }
        catch (System.Exception ex)
        {
            Debug.WriteLine($"Error parsing playlist: {ex.Message}");
        }

        return (playlistItems, background);
    }
}