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

public class EventProcessor
{
    private class EventProcessingStats
    {
        public bool IsEventReceived { get; set; }
    }

    private static readonly OnEventCallback EventCallbackHandler = (ref object userData, XmEvent xmEvent) =>
    {
        try
        {
            var udata = (userData as EventProcessingStats);
            if (udata == null)
                return false;

            udata.IsEventReceived = true;
            EventRelease(xmEvent);

            return true;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error in event callback: {ex.Message}");
            return false;
        }
    };

    public static void ProcessSchemeFile(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("File path cannot be empty", nameof(filePath));
        }

        string scheme;
        try
        {
            scheme = File.ReadAllText(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"File reading error: {ex.Message}");
            return;
        }

        var error = new Error();
        var container = XmHandlerCreate(scheme, 0, ref error);

        if (container == null || error.Code != 0)
        {
            throw new ApplicationException($"Failed to create handler: {error}");
        }

        try
        {
            using (var handlers = HandlersGet(container))
            {
                if (handlers.Count == 0)
                {
                    Console.WriteLine("Could not get handlers from scheme");
                    return;
                }

                var firstHandler = handlers[0];//TODO: may be better to check last handler, but not sure that it's working
                var stats = new EventProcessingStats();
                var eventType = MediaEventType.EOFReached;
                var callback = EventCallbackAdd(container,
                                            [eventType],
                                            ref stats,
                                            EventCallbackHandler,
                                            ref error);

                if (callback == null)
                {
                    throw new ApplicationException($"Failed to add event callback: {error}");
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
                    while (!stats.IsEventReceived)
                    {
                        Thread.Sleep(1000);

                        if (stats.IsEventReceived)
                        {
                            Console.WriteLine($"Success! Scheme finished.");
                            break;
                        }
                        i++;
                        Console.WriteLine($"Waiting... ({i} seconds)");
                    }
                }
                finally
                {
                    EventCallbackRelease(callback);
                }
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
            Console.WriteLine("XMedia API: event callback usage sample (with EOF event)");

            if (args.Length == 0)
            {
                var execName = AppDomain.CurrentDomain.FriendlyName;
                Console.WriteLine($"Usage: {execName} <scheme_file_path>");
                Console.WriteLine($"Example: {execName} ./scheme/transcoding_demo.json");
                return 1;
            }

            var filePath = args[0];

            Console.WriteLine($"Executing scheme from file: {filePath}");

            EventProcessor.ProcessSchemeFile(filePath);

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