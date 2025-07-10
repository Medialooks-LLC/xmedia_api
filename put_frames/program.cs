using System;
using System.IO;
using System.Threading;
using static Medialooks.XMedia.Callbacks;
using static Medialooks.XMedia.DevTools.Utils;
using static Medialooks.XMedia.Enumerations;
using static Medialooks.XMedia.Functions;
using static Medialooks.XMedia.Handlers;
using static Medialooks.XMedia.Objects;
using static Medialooks.XMedia.Structures;
using static Medialooks.XMedia.FrameFunctions;

public class TimerProcessor
{
    private class CBTimerData
    {
        public Int64 StartTime { get; set; }
        public Int64 FrameDuration { get; set; }
        required public XmHandler Container { get; set; }
        public int CountCalls { get; set; }
        public bool HasError { get; set; }
        public bool IsRunning { get; set; }
    }

    private static readonly OnTimerCallback TimerCallbackHandler = (ref object userData, XmTimerTick tick) =>
    {
        var error = new Error();
        var timerData = (CBTimerData)userData;
        if (timerData.CountCalls == 299) { 
            timerData.IsRunning = false;
            return -1;
        }

        var capturedTime = (XmClockTime(null) - timerData.StartTime) / 10_000.0;//msec
        var vFrame = GenerateXmVideoFrame(timerData.CountCalls, capturedTime);
        var res = XmHandlerFramePut(timerData.Container, vFrame, HandlerPutFlags.kPutNoFlags, ref error);

        var udata = (userData as CBTimerData);
        if (udata == null)
            return -1; // TODO: which time should be here?

        if (!res)
        {
            udata.HasError = true;
            Console.WriteLine("Couldn't put video frame with num {0}", timerData.CountCalls);
            return -1; // TODO: which time should be here?
        }
        var aFrame = GenerateXmAudioFrame(timerData.CountCalls * 1764);
        res = XmHandlerFramePut(timerData.Container, aFrame, HandlerPutFlags.kPutNoFlags, ref error);

        if (!res)
        {
            udata.HasError = true;
            Console.WriteLine("Couldn't put audio frame with num {0}", timerData.CountCalls);
            return -1; // TODO: which time should be here?
        }

        udata.CountCalls++;
        return timerData.StartTime + udata.CountCalls * timerData.FrameDuration;
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
        var v_frame = GenerateXmVideoFrame(0, 0);
        var a_frame = GenerateXmAudioFrame(0);
        var container = XmFramesHandlerCreate(scheme, 0, "", [a_frame, v_frame], ref error);

        if (container == null || error.Code != 0)
        {
            throw new ApplicationException($"Failed to create handler: {error}");
        }

        var command = new Command
        {
            command_name = "output_start",
            target_path = "c_container"
        };

        var res = XmHandlerCommandExecute(container, command);

        var timerData = new CBTimerData()
        {
            StartTime = XmClockTime(null) + 40 * 10000,
            Container = container,
            CountCalls = 0,
            FrameDuration = 40 * 10000, // 40 msec
            HasError = false,
            IsRunning = true
        };

        var callback = TimerCallbackAdd((timerData as CBTimerData).StartTime,
                                                    ref timerData,
                                                    TimerCallbackHandler,
                                                    ref error);
        if (callback == null)
        {
            throw new ApplicationException($"Failed to add timer callback: {error}");
        }


            int i = 0;
            while (timerData.IsRunning)
            {
                Thread.Sleep(1000);
                i++;
                Console.WriteLine($"Waiting... ({i} second). Passed {timerData.CountCalls} frames.");
            }
            TimerCallbackStop(callback, ref error);
            TimerCallbackRelease(callback);

        XmHandlerClose(container);
        XmHandlerRelease(container);
    }
}

class Program
{
    static int Main(string[] args)
    {
        try
        {
            Console.WriteLine("XMedia API: timer callback usage sample (send frames to container)");

            if (args.Length == 0)
            {
                var execName = AppDomain.CurrentDomain.FriendlyName;
                Console.WriteLine($"Usage: {execName} <scheme_file_path>");
                Console.WriteLine($"Example: {execName} ./schemes/xm_encode_to_file.json");
                return 1;
            }

            var filePath = args[0];//@"E:/work/xmedia_api/build/bin/Debug/schemes/xm_encode_to_file.json";
            var timeout = args.Length > 1 && int.TryParse(args[1], out int seconds) ? seconds : 5;

            Console.WriteLine($"Executing scheme from file: {filePath}");

            TimerProcessor.ProcessSchemeFile(filePath);

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