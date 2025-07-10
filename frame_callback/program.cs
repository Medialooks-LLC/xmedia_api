using System;
using System.IO;
using System.Threading;
using static Medialooks.XMedia.Callbacks;
using static Medialooks.XMedia.Enumerations;
using static Medialooks.XMedia.Functions;
using static Medialooks.XMedia.Handlers;
using static Medialooks.XMedia.Objects;
using static Medialooks.XMedia.Structures;
using static Medialooks.XMedia.FrameFunctions;

public class FrameProcessor
{
    private class FrameProcessingStats
    {
        public bool ContinueFrameReceiving { get; set; }
        public int FrameCount { get; set; }
        public int VideoFramesReceived { get; set; }
        required public string OutputDir { get; set; }
    }

    private static readonly OnFrameCallback FrameCallbackHandler = (ref object userData, XmFrame frame) =>
    {
        try
        {
            if (frame == null) return true;

            var stats = (FrameProcessingStats)userData;

            if (stats.VideoFramesReceived > 300)
                stats.ContinueFrameReceiving = false;

            if (frame?.frame_type == ObjectType.FrameVideo)
            {
                if (stats.VideoFramesReceived % 30 == 0)
                {
                    string numStr = stats.VideoFramesReceived.ToString();
                    numStr = numStr.PadLeft(4, '0');// format to 000N view

                    string dstPath = Path.Combine(stats.OutputDir, "captured_" + numStr + ".png");
                    var error = new Error();
                    var res = XmFrameSaveToFile(frame, dstPath, null, ref error);
                    if (!res)
                    {
                        Console.WriteLine($"Couldn't save {numStr} frame to file: {dstPath} with error: {error}");
                    }

                }
                stats.VideoFramesReceived++;
            }
            stats.FrameCount++;
            XmFrameRelease(frame);
            return true;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error in frame callback: {ex.Message}");
            return false;
        }
    };

    public static void ProcessVideoFile(string filePath, string ouputDir, int timeoutSeconds = 5)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("File path cannot be empty", nameof(filePath));
        }

        const string schemeTemplate = @"
        {
            ""container_items"": [
                {
                    ""open_url"": ""file_place_holder"",
                    ""name"": ""my_demux_name"",
                    ""subtype"": ""av_demultiplexer""
                },
                {
                    ""type"": ""kDecoder"",
                    ""name"": ""my_decoder"",
                    ""wrappers"": [{ ""wrapper_type"": ""multistream"" }]
                }
            ],
            ""subtype"": ""serial_container""
        }";
        var scheme = schemeTemplate.Replace("file_place_holder", filePath.Replace("\\", "/"));
        var error = new Error();
        var container = XmHandlerCreate(scheme, 0, ref error);

        if (container == null)
        {
            throw new ApplicationException($"Failed to create handler: {error}");
        }

        try
        {
            var decoder = XmHandlerOpen(container, "my_decoder");
            var stats = new FrameProcessingStats()
            {
                ContinueFrameReceiving = true,
                FrameCount = 0,
                OutputDir = ouputDir,
                VideoFramesReceived = 0
            };

            var callback = FrameCallbackAdd(
                decoder,
                ObjectType.FrameVideo,
                null,
                ref stats,
                FrameCallbackHandler,
                ref error);

            if (callback == null)
            {
                throw new ApplicationException($"Failed to add frame callback: {error}");
            }

            try
            {
                var command = new Command
                {
                    command_name = "output_start",
                    target_path = "c_container"
                };

                XmHandlerCommandExecute(container, command);

                for (int i = 0; i < timeoutSeconds; i++)
                {
                    Thread.Sleep(1000);

                    if (!stats.ContinueFrameReceiving)
                    {
                        Console.WriteLine($"Success! Processed {stats.FrameCount} frames.");
                        break;
                    }

                    Console.WriteLine($"Waiting... ({i + 1}/{timeoutSeconds} seconds)");
                }

                if (!stats.ContinueFrameReceiving)
                {
                    Console.WriteLine("No frames received within timeout period.");
                }
            }
            finally
            {
                FrameCallbackRelease(callback);
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
            Console.WriteLine("XMedia API: frame callback usage sample");

            if (args.Length == 0)
            {
                var execName = AppDomain.CurrentDomain.FriendlyName;
                Console.WriteLine($"Usage: {execName} <file_path> <output_directory> [timeout_seconds]");
                Console.WriteLine($"Example: {execName} \"C:\\videos\\test.mp4\" \"C:\\videos\\\" 10");
                return 1;
            }

            var filePath = args[0];
            var outputDir = args[1];
            var timeout = args.Length > 1 && int.TryParse(args[2], out int seconds) ? seconds : 5;

            Console.WriteLine($"Processing file: {filePath}");
            Console.WriteLine($"Timeout: {timeout} seconds");

            FrameProcessor.ProcessVideoFile(filePath, outputDir, timeout);

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