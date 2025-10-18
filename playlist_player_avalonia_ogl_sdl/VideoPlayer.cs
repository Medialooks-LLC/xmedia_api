using Avalonia.Threading;
using OpenGLAvalonia.Views;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using static Medialooks.XMedia.Callbacks;
using static Medialooks.XMedia.Functions;
using static Medialooks.XMedia.Objects;
using static Medialooks.XMedia.Structures;
using static Medialooks.XMedia.FrameFunctions;

namespace OpenGLAvalonia
{
    public class VideoPlayer : IDisposable
    {
        private readonly ConcurrentQueue<XmFrame> _videoQueue = new();
        private XmTimerCallback? _timerCallback;
        private readonly OpenGlVideoControl _openGlVideoControl;
        private readonly double _frameRate;

        public VideoPlayer(OpenGlVideoControl openGlVideoControl, double frameRate)
        {
            _openGlVideoControl = openGlVideoControl;
            _frameRate = frameRate;
        }
        public void Dispose()
        {
            if (_timerCallback != null)
            {
                var error = new Error();
                TimerCallbackStop(_timerCallback!, ref error);
                TimerCallbackRelease(_timerCallback!);
            }

            Debug.WriteLine("\nPlayback finished.");
        }

        public void Start()
        {
            var timerData = new CBTimerData()
            {
                StartTime = XmClockTime(null),
                CountCalls = 0,
                FrameDuration = 1000.0 * 10000.0 / _frameRate, // msec * 10000
                OutputContol = _openGlVideoControl,
                Queue = _videoQueue
            };

            var error = new Error();
            _timerCallback = TimerCallbackAdd(timerData.StartTime,
                                                        ref timerData,
                                                        TimerCallbackHandler,
                                                        ref error);
            if (_timerCallback == null)
            {
                throw new ApplicationException($"Failed to add timer callback: {error}");
            }
            Debug.WriteLine($"Video start: {DateTimeOffset.Now.ToUnixTimeMilliseconds()}");
        }

        public void EnqueueVideo(XmFrame frame)
        {
            _videoQueue.Enqueue(frame);
        }

        private class CBTimerData
        {
            public Int64 StartTime { get; set; }
            public double FrameDuration { get; set; }
            public int CountCalls { get; set; }
            public required OpenGlVideoControl OutputContol { get; set; }
            public required ConcurrentQueue<XmFrame> Queue { get; set; }
        }

        private long TimerCallbackHandler(ref object userData, XmTimerTick tick)
        {
            var timerData = (CBTimerData)userData;

            if (timerData == null)
                return -1;

            var capturedTime = (XmClockTime(null) - timerData.StartTime) / 10_000.0;//msec
            if (timerData.Queue.TryDequeue(out var _currentVideoFrame))
            {
                //Debug.WriteLine($"ts: {_currentVideoFrame.time.timestamp} d: {_currentVideoFrame.time.duration} ss: {_currentVideoFrame.time.segment_start} se:{_currentVideoFrame.time.segment_end}");
                Dispatcher.UIThread.Post(() =>
                {
                    timerData.OutputContol.UpdateFrame(_currentVideoFrame);
                });
            }

            timerData.CountCalls++;
            return (long)(timerData.StartTime + timerData.CountCalls * timerData.FrameDuration);
        }
    }
}