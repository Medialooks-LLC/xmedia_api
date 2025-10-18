using Avalonia.Threading;
using OpenGLAvalonia.Views;
using SDL3;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using static Medialooks.XMedia.FrameFunctions;
using static Medialooks.XMedia.Functions;
using static Medialooks.XMedia.Structures;

public class AudioPlayer : IDisposable
{
    private readonly ConcurrentQueue<XmAudioData> _audioQueue = new();
    private readonly IntPtr _stream;
    private bool _isPlaying = false;

    public event EventHandler? PlaybackStart;

    public static string[] GetAvaliableDrivers()
    {
        var driversNum = SDL.GetNumAudioDrivers();
        var res = new string[driversNum];
        for (int i = 0; i < driversNum; i++)
        {
            var driverName = SDL.GetAudioDriver(i);
            res[i] = driverName ?? "";
            Debug.WriteLine($"{i}: {driverName}");
        }
        return res;
    }

    public AudioPlayer(double sampleRate = 44100.0, int channels = 2)
    {
        if (!SDL.Init(SDL.InitFlags.Audio))
        {
            Debug.WriteLine($"SDL_Init failed: {SDL.GetError()}");
            throw new Exception($"SDL_Init failed: {SDL.GetError()}");
        }

        Debug.WriteLine($"Audio driver: {SDL.GetCurrentAudioDriver()}");
        Debug.WriteLine($"Audio device: {SDL.GetAudioDeviceName(SDL.AudioDeviceDefaultPlayback)}");


        var want = new SDL.AudioSpec
        {
            Freq = (int)sampleRate,
            Format = SDL.AudioFormat.AudioF32LE, // 32-bit float
            Channels = channels
        };

        _stream = SDL.OpenAudioDeviceStream(SDL.AudioDeviceDefaultPlayback, in want, AudioCallback, IntPtr.Zero);
        if (_stream == 0)
        {
            Debug.WriteLine($"Failed to open audio device: {SDL.GetError()}");
            SDL.Quit();
            throw new Exception($"Failed to open audio device: {SDL.GetError()}");
        }

        var res = SDL.GetAudioStreamFormat(_stream, out want, out var have);
        if (!res)
        {
            Debug.WriteLine($"Failed to get audio stream format: {SDL.GetError()}");
            SDL.Quit();
            return;
        }
        Debug.WriteLine($"Audio device opened:");
        Debug.WriteLine($"  Frequency: {want.Freq} Hz");
        Debug.WriteLine($"  Format: {want.Format}");
        Debug.WriteLine($"  Channels: {want.Channels}");

        Debug.WriteLine($"Audio device opened:");
        Debug.WriteLine($"  Frequency: {have.Freq} Hz");
        Debug.WriteLine($"  Format: {have.Format}");
        Debug.WriteLine($"  Channels: {have.Channels}");
    }

    public void Dispose()
    {
        if (_isPlaying)
        {
            SDL.DestroyAudioStream(_stream);
            SDL.Quit();
        }

        _isPlaying = false;
        Debug.WriteLine("\nPlayback finished.");
    }

    public void Start()
    {
        _isPlaying = true;
        SDL.ResumeAudioStreamDevice(_stream);
        PlaybackStart?.Invoke(this, EventArgs.Empty);
    }

    public void EnqueueAudio(XmFrame frame)
    {
        var audioData = XmFrameAudioDataGet(frame);
        if (audioData != null)
        {
            _audioQueue.Enqueue(audioData);
        }
        return;
    }

    private void AudioCallback(nint userdata, nint stream, int additionalAmount, int len)
    {
        //return;
        if (!_isPlaying)
        {
            Debug.WriteLine("not playing");
            return;
        }

        if (_audioQueue.TryDequeue(out var audioData))
        {
            //Prepare audio to play
            int channels = audioData.format.channels;

            nint firstPlanePtr = audioData.p_planes;
            var planeAudio = Marshal.PtrToStructure<XmPlaneA>(firstPlanePtr);
            if (planeAudio != null && planeAudio.audio_p != IntPtr.Zero)
            {
                SDL.PutAudioStreamData(stream, planeAudio.audio_p, planeAudio.bytes);
            }
        }
        //else {
        //    Debug.WriteLine("No audio data to play");
        //}
    }
}
