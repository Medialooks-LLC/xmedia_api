using System;
using System.Threading;
using static Medialooks.XMedia.Handlers;
using static Medialooks.XMedia.Enumerations;
using static Medialooks.XMedia.Structures;
using static Medialooks.XMedia.Callbacks;
using static Medialooks.XMedia.FrameFunctions;
using static Medialooks.XMedia.Objects;
using static Medialooks.XMedia.DevTools.Handlers;
using System.IO;

public class PlaylistPlayer
{
    public static void ProcessSchemeFile(string schemePath, string playlistPath, string? conversionPath)
    {
        if (string.IsNullOrWhiteSpace(schemePath))
        {
            throw new ArgumentException("File path to scheme cannot be empty", nameof(schemePath));
        }
        if (string.IsNullOrWhiteSpace(playlistPath))
        {
            throw new ArgumentException("File path to playlist cannot be empty", nameof(playlistPath));
        }

        string scheme;
        try
        {
            scheme = File.ReadAllText(schemePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"File reading error: {ex.Message}");
            return;
        }

        string playlist;
        try
        {
            playlist = File.ReadAllText(playlistPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"File reading error: {ex.Message}");
            return;
        }

        string conversion = "";
        if (conversionPath != null)
        {
            try
            {
                conversion = File.ReadAllText(conversionPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"File reading error: {ex.Message}");
                return;
            }
        }

        scheme = scheme.Replace("playlist_place_holder", playlist.Replace("\"", "\\\"").Replace("\r\n", "").Replace("\n", ""));
        Console.WriteLine(scheme);

        var error = new Error();
        var container = XmHandlerCreate(scheme, 0, ref error);

        if (container == null || error.Code != 0)
        {
            throw new ApplicationException($"Failed to create handler: {error}");
        }

        try
        {
            var command = new Command
            {
                command_name = "output_start",
                target_path = "c_container"
            };

            XmHandlerCommandExecute(container, command);


            int i = 0;
            while (i < 110)
            {
                Thread.Sleep(1000);

                i++;
                if (i == 2 && conversionPath != null)
                {
                    command = new Command
                    {
                        command_name = "conversion_set",//"playback_start",
                        json_command_body = conversion,
                        target_path = "c_container::playlist_player"
                    };
                    var res = XmHandlerCommandExecute(container, command);
                    Console.WriteLine($"Json res: {res.json_result_or_error_desc}");
                }
                if (i == 3)
                {
                    command = new Command
                    {
                        command_name = "conversion_get",
                        target_path = "c_container::playlist_player"
                    };
                    var res = XmHandlerCommandExecute(container, command);
                    Console.WriteLine($"Json res: {res.json_result_or_error_desc}");
                }
                if (i == 5)
                {
                    command = new Command
                    {
                        command_name = "playback_start",
                        json_command_body = @"{ ""pos_in_sec"": 2 }",
                        target_path = "c_container::playlist_player"
                    };
                    var res =  XmHandlerCommandExecute(container, command);
                    Console.WriteLine($"Json res: {res.json_result_or_error_desc}");
                }
                Console.WriteLine($"Waiting... ({i} seconds)");
            }
        }
        finally
        {
            XmHandlerClose(container);
            XmHandlerRelease(container);
        }
    }
}

class Program
{
    static int Main(string[] args)
    {
        try
        {
            Console.WriteLine("XMedia API: playlist usage sample");

            if (args.Length == 0)
            {
                var execName = AppDomain.CurrentDomain.FriendlyName;
                Console.WriteLine($"Usage: {execName} <scheme_file_path> <playlist_json> [<conversion_json>]");
                Console.WriteLine($"Example: {execName} ./scheme/playlist_player_to_ndi.json sample_playlist.json");
                return 1;
            }

            var schemePath = args[0];
            var plalistPath = args[1];
            var conversionPath = args.Length > 2 ? args[2] : null;

            Console.WriteLine($"Executing scheme from file: {schemePath}");

            PlaylistPlayer.ProcessSchemeFile(schemePath, plalistPath, conversionPath);

            Console.WriteLine("Processing completed.");
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return -1;
        }
    }
}