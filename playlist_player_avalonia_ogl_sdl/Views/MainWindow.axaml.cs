using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Converters;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Rendering;
using Avalonia.Threading;
using Medialooks.XMedia;
using Microsoft.VisualBasic;
using OpenGLAvalonia.ViewModels;
using OpenGLAvalonia.Views;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http.Json;
using System.Numerics;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static Medialooks.XMedia.Callbacks;
using static Medialooks.XMedia.Enumerations;
using static Medialooks.XMedia.FrameFunctions;
using static Medialooks.XMedia.Handlers;
using static Medialooks.XMedia.Objects;
using static Medialooks.XMedia.Structures;

namespace OpenGLAvalonia
{
    public partial class MainWindow : Window
    {
        public string Playlist = "";
        private AudioPlayer? _audioPlayer;
        private VideoPlayer? _videoPlayer;
        private XmHandler? _container;
        private XmFrameCallback? _frameCallback;
        private bool _isPlaying = false;
        private bool _isPaused = false;
        private bool _playlistFromFile = false;
        private string _schemeWithPlaylist = string.Empty;
        private readonly string _scheme = @"{
            ""container_items"":[
                {
                    ""name"":""my_playlist_player"",
                    ""open_url"":""playlist_place_holder"",
                    ""subtype"":""playlist_player"",
                    ""wrappers"":[
                        {
                            ""correction_side"":""output"",
                            ""wrapper_type"":""times_corrector""
                        }
                    ]
                },
                {
                    ""name"":""ndi_out"",
                    ""open_url"":""ndi://handler_with_ndi_output"",
                    ""subtype"":""ndi_renderer""
                }
            ],
            ""subtype"":""serial_container""
        }";

#pragma warning disable 0414
        private readonly string _scheme_with_conv = @"{
            ""container_items"":[
                {
                    ""name"":""my_playlist_player"",
                    ""open_url"":""playlist_place_holder"",
                    ""subtype"":""playlist_player"",
                    ""wrappers"":[
                        {
                            ""correction_side"":""output"",
                            ""wrapper_type"":""times_corrector""
                        }
                    ]
                },
                {
                  ""init_props"": {
                    ""audio"": {
                      ""audio_format"": {
                        ""channels"": 2,
                        ""sample_rate"": 44100,
                        ""sample_format"": ""flt"",
                        ""type"": ""Audio""
                      }
                    },
                    ""video"": {
                      ""video_format"": {
                        ""field_order"": ""Progressive"",
                        ""frame_rate"": 29.97,
                        ""height"": 768,
                        ""pixel_format"": ""bgra"",
                        ""type"": ""Video"",
                        ""width"": 1024
                      }
                    }
                  },
                  ""subtype"": ""av_converter"",
                  ""name"":  ""my_converter"",
                  ""wrappers"": [
                    {
                      ""wrapper_type"": ""multistream""
                    }
                  ]
                },
                {
                    ""name"":""ndi_out"",
                    ""open_url"":""ndi://handler_with_ndi_output"",
                    ""subtype"":""ndi_renderer""
                }
            ],
            ""subtype"":""serial_container""
        }";
        private readonly string _scheme_play_file = @"{
            ""container_items"":[
                {
                    ""name"": ""my_demux_name"",
                    ""open_url"": ""E:/Media/Audio_Video_Sync_Test___Calibration_29.97fps.mp4"",
                    ""subtype"": ""av_demultiplexer""
                },
                {
                    ""type"": ""kDecoder"",
                    ""wrappers"": [{
                            ""wrapper_type"": ""multistream""
                        }]
                },
                {
                  ""init_props"": {
                    ""audio"": {
                      ""audio_format"": {
                        ""channels"": 2,
                        ""sample_rate"": 44100,
                        ""sample_format"": ""flt"",
                        ""type"": ""Audio""
                      }
                    },
                    ""video"": {
                      ""video_format"": {
                        ""field_order"": ""Progressive"",
                        ""frame_rate"": 29.97,
                        ""height"": 720,
                        ""pixel_format"": ""bgra"",
                        ""type"": ""Video"",
                        ""width"": 1280
                      }
                    }
                  },
                  ""subtype"": ""av_converter"",
                  ""name"":  ""my_converter"",
                  ""wrappers"": [
                    {
                      ""wrapper_type"": ""multistream""
                    }
                  ]
                },
                {
                    ""name"":""ndi_out"",
                    ""open_url"":""ndi://handler_with_ndi_output"",
                    ""subtype"":""ndi_renderer""
                }
            ],
            ""subtype"":""serial_container""
        }";
#pragma warning restore 0414
        private string _conversionJson = @"{
            ""audio_format"":{
                ""channels"":2,
                ""sample_rate"":44100,
                ""type"":""Audio"",
                ""sample_format"": ""flt""
            },
            ""video_format"":{
                ""field_order"":""Progressive"",
                ""frame_rate"": 29.97,
                ""height"":720,
                ""pixel_format"":""bgra"",
                ""type"":""Video"",
                ""width"":1280
            }
        }";

        private double _dpiScale = 1.0;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            NewButton.Click += OnNewPlaylist;
            OpenButton.Click += OnOpenFile;
            PlayButton.Click += OnPlayClick;
            PauseButton.Click += OnPauseClick;
            StopButton.Click += OnStopPlaying;
            SetConversionButton.Click += OnShowSettingsDialog;
            var visual = this as IRenderRoot;
            _dpiScale = visual?.RenderScaling ?? 1.0;
            GlControl.DpiScale = _dpiScale;
            var cSettings = ConversionSettingsViewModel.FromJson(_conversionJson);
            ConversionStatus.Text = $"V: {cSettings.VideoWidth}x{cSettings.VideoHeight}@{cSettings.VideoFrameRate} A: {cSettings.AudioSampleRate}Hz, {cSettings.AudioChannels} channels";
            PlaylistTable1.IsReadOnly = true;
            var plControl = (DataContext as MainViewModel)?.PlaylistViewModel;
            plControl!.PropertyChanged += PlControl_BackgroundChanged;
        }

        ~MainWindow()
        {
            CleanResources();
        }

        private void CleanResources()
        {
            if (_frameCallback != null)
            {
                FrameCallbackRelease(_frameCallback);
                _frameCallback = null;
            }
            if (_container != null)
            {
                XmHandlerClose(_container);
                XmHandlerRelease(_container);
                _container = null;
            }
            _audioPlayer?.Dispose();
            _audioPlayer = null;
            _videoPlayer?.Dispose();
            _videoPlayer = null;
        }
        //TODO: fix set background on started container
        private void PlControl_BackgroundChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "PlaylistBackground")
            {
                var plControl = (DataContext as MainViewModel)?.PlaylistViewModel;
                //TODO: check that file is available?
                if (plControl!.PlaylistBackground.Length > 0)
                {
                    Debug.WriteLine($"New background {plControl!.PlaylistBackground}");
                    if (_container != null)
                    {

                        var plBackground = new PlaylistItemJson()
                        {
                            Break = false,
                            FixedPosSec = null,
                            SplitUid = 0,
                            Media = new MediaItem()
                            {
                                InSec = null,
                                OpenUrl = plControl.PlaylistBackground,
                                OutSec = null,
                                //DurationSec = item.OutSec - item.OutSec,
                                Uid = 0
                            }
                        };
                        var plBackgroundData = JsonSerializer.Serialize<PlaylistItemJson>(plBackground);
                        var addBackgroundCommand = new Command
                        {
                            command_name = "background_set",
                            json_command_body = $"{plBackgroundData}",
                            target_path = "c_container::my_playlist_player"
                        };
                        var jsonAddRes = XmHandlerCommandExecute(_container, addBackgroundCommand);
                        if (jsonAddRes?.error_code != 0)
                        {
                            Status.Text = $"Failed to set new background: {jsonAddRes!.error_code} {jsonAddRes!.json_result_or_error_desc}\n File {plControl!.PlaylistBackground}";
                            return;
                        }
                    }
                }
            }
        }

        //2Think: maybe move following commands to extra class?
        private void OnPauseClick(object? sender, RoutedEventArgs e)
        {
            if (_isPaused)
            {
                var command = new Command
                {
                    command_name = "playback_continue",
                    target_path = "c_container::my_playlist_player"
                };
                var jsonRes = XmHandlerCommandExecute(_container, command);
                if (jsonRes?.error_code != 0)
                {
                    Status.Text = $"Failed to continue playback: {jsonRes!.error_code} {jsonRes!.json_result_or_error_desc}";
                    return;
                }
                _isPaused = false;
            }
            else
            {
                var command = new Command
                {
                    command_name = "playback_pause",
                    target_path = "c_container::my_playlist_player"
                };
                var jsonRes = XmHandlerCommandExecute(_container, command);
                if (jsonRes?.error_code != 0)
                {
                    Status.Text = $"Failed to pause playback: {jsonRes!.error_code} {jsonRes!.json_result_or_error_desc}";
                    return;
                }
                _isPaused = true;
            }
        }

        private bool CreateContainer(string scheme)
        {
            var error = new Error();
            _container = XmHandlerCreate(scheme, 0, ref error);

            if (_container == null || error.Code != 0)
            {
                Status.Text = $"Failed to create handler: {error}";
                return false;
            }
            return true;
        }

        private bool UpdateConversion(string conversionJson)
        {
            var command = new Command
            {
                command_name = "conversion_set",
                json_command_body = conversionJson,
                target_path = "c_container::my_playlist_player"
            };

            var jsonRes = XmHandlerCommandExecute(_container, command);
            if (jsonRes?.error_code != 0)
            {
                Status.Text = $"Failed to set conversion format: {jsonRes!.error_code} {jsonRes!.json_result_or_error_desc}";
                return false;
            }
            return true;
        }

        private bool OutputStart()
        {
            var command = new Command
            {
                command_name = "output_start",
                target_path = "c_container"
            };

            var jsonRes = XmHandlerCommandExecute(_container, command);
            if (jsonRes?.error_code != 0)
            {
                Status.Text = $"Failed to start container: {jsonRes!.error_code} {jsonRes!.json_result_or_error_desc}";
                return false;
            }
            return true;
        }

        private bool SubscribeOnFrameCallback()
        {
            var cSettings = ConversionSettingsViewModel.FromJson(_conversionJson);
            _videoPlayer = new VideoPlayer(GlControl, cSettings.VideoFrameRate);
            _audioPlayer = new AudioPlayer(sampleRate: cSettings.AudioSampleRate, channels: cSettings.AudioChannels);

            var player = XmHandlerOpen(_container, "my_playlist_player");// "my_converter");
            if (player == null)
            {
                Status.Text = $"Failed to get 'my_converter' handler";
                return false;
            }

            var stats = new FrameProcessingStats()
            {
                ContinueFrameReceiving = true,
                FrameCount = 0,
                AudioPlayer = _audioPlayer,
                VideoPlayer = _videoPlayer,
            };
            var error = new Error();
            _frameCallback = FrameCallbackAdd(
                player,
                ObjectType.FrameVideo,
                null,
                ref stats,
                FrameCallbackHandler,
                ref error);

            if (_frameCallback == null)
            {
                Status.Text = $"Failed to add frame callback: {error}";
                return false;
            }

            return true;
        }

        private bool InitPlaylistPlayerByScheme(string scheme)
        {
            CleanResources();

            if (!CreateContainer(scheme)) return false;

            if (!UpdateConversion(_conversionJson)) return false;

            if (!SubscribeOnFrameCallback()) return false;

            if (!OutputStart()) return false;

            _audioPlayer!.PlaybackStart += (sender, e) => _videoPlayer?.Start();
            _audioPlayer.Start();
            return true;
        }

        private void OnNewPlaylist(object? sender, RoutedEventArgs e)
        {
            _playlistFromFile = false;
            PlaylistTable1.IsReadOnly = false;
            var plControl = (DataContext as MainViewModel)?.PlaylistViewModel;
            Debug.Assert(plControl != null, "Playlist View Model is null");
            plControl.PlaylistItems.Clear();
            plControl.PlaylistBackground = string.Empty;
            InitPlaylistPlayerByScheme(_scheme.Replace("playlist_place_holder", ""));
        }

        private async void OnOpenFile(object? sender, RoutedEventArgs e)
        {
            var topLevel = TopLevel.GetTopLevel(this);
            Debug.Assert(topLevel != null, "Top level windiow is null");

            var jsonType = new FilePickerFileType("json");
            jsonType.Patterns = ["*.json"];
            // Start async operation to open the dialog.
            var files = await topLevel!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open JSON File",
                AllowMultiple = false,
                FileTypeFilter = [jsonType]
            });

            try
            {
                Playlist = await File.ReadAllTextAsync(files[0].Path.AbsolutePath);
                Status.Text = $"File {files[0].Name} loaded";
            }
            catch (Exception ex)
            {
                Status.Text = $"Error loading file: {ex.Message}";
                return;
            }

            _playlistFromFile = true;
            PlaylistTable1.IsReadOnly = true;
            var pl = PlaylistParser.ParsePlaylistFromJson(Playlist);
            var plControl = (DataContext as MainViewModel)?.PlaylistViewModel;
            Debug.Assert(plControl != null, "Playlist View Model is null");
            plControl.PlaylistItems = pl.Item1;
            plControl.PlaylistBackground = pl.Item2;

            _schemeWithPlaylist = _scheme.Replace("playlist_place_holder", Playlist.Replace("\"", "\\\"").Replace("\r\n", "").Replace("\n", ""));
            //var newScheme = _scheme_with_conv.Replace("playlist_place_holder", Playlist.Replace("\"", "\\\"").Replace("\r\n", "").Replace("\n", ""));
            //var newScheme = _scheme_play_file;
            InitPlaylistPlayerByScheme(_schemeWithPlaylist);
        }

        private void OnPlayClick(object? sender, RoutedEventArgs e)
        {
            PlaylistTable1.IsReadOnly = true;
            var cSettings = ConversionSettingsViewModel.FromJson(_conversionJson);
            if (!_playlistFromFile && !_isPlaying)
            {
                Status.Text = "";
                var plControl = (DataContext as MainViewModel)?.PlaylistViewModel;
                Debug.Assert(plControl != null, "Playlist View Model is null");
                foreach (var item in plControl.PlaylistItems)
                {
                    var plItem = new PlaylistItemJson()
                    {
                        Break = item.IsBreak,
                        FixedPosSec = item.FixedPosSec,
                        SplitUid = 0,
                        Media = new MediaItem()
                        {
                            InSec = item.InSec,
                            OpenUrl = item.OpenUrl,
                            OutSec = item.OutSec,
                            //DurationSec = item.OutSec - item.OutSec,
                            Uid = 0
                        }
                    };
                    var plItemData = JsonSerializer.Serialize<PlaylistItemJson>(plItem);
                    var addCommand = new Command
                    {
                        command_name = "playlist_item_add",
                        json_command_body = $@"{{""playlist_item"": {plItemData}}}",
                        target_path = "c_container::my_playlist_player"
                    };
                    var jsonAddRes = XmHandlerCommandExecute(_container, addCommand);
                    if (jsonAddRes?.error_code != 0)
                    {
                        Status.Text += $"Failed to add file to playlist: {jsonAddRes!.error_code} {jsonAddRes!.json_result_or_error_desc}\n";
                        return;
                    }
                }

                if (plControl.PlaylistBackground?.Length > 0)
                {
                    var plBackground = new PlaylistItemJson()
                    {
                        Break = false,
                        FixedPosSec = null,
                        SplitUid = 0,
                        Media = new MediaItem()
                        {
                            InSec = null,
                            OpenUrl = plControl.PlaylistBackground,
                            OutSec = null,
                            //DurationSec = item.OutSec - item.OutSec,
                            Uid = 0
                        }
                    };
                    var plBackgroundData = JsonSerializer.Serialize<PlaylistItemJson>(plBackground);
                    var addBackgroundCommand = new Command
                    {
                        command_name = "background_set",
                        json_command_body = $"{plBackgroundData}",
                        target_path = "c_container::my_playlist_player"
                    };
                    var jsonAddRes = XmHandlerCommandExecute(_container, addBackgroundCommand);
                    if (jsonAddRes?.error_code != 0)
                    {
                        Status.Text += $"Failed to set background: {jsonAddRes!.error_code} {jsonAddRes!.json_result_or_error_desc}\n";
                        return;
                    }
                }
            }
            _isPlaying = true;

            if (_isPaused)
            {
                var command = new Command
                {
                    command_name = "playback_continue",
                    target_path = "c_container::my_playlist_player"
                };
                var jsonRes = XmHandlerCommandExecute(_container, command);
                if (jsonRes?.error_code != 0)
                {
                    Status.Text = $"Failed to pause playback: {jsonRes!.error_code} {jsonRes!.json_result_or_error_desc}";
                    return;
                }
                _isPaused = false;
                Status.Text += $"Playback continued";
            }
            else
            {
                var command = new Command
                {
                    command_name = "playback_start",
                    target_path = "c_container::my_playlist_player"
                };
                var jsonRes = XmHandlerCommandExecute(_container, command);
                if (jsonRes?.error_code != 0)
                {
                    Status.Text = $"Failed to start playback: {jsonRes!.error_code} {jsonRes!.json_result_or_error_desc}";
                    return;
                }
                GlControl.start_time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                Status.Text += $"Playback started";
            }

        }

        private void OnStopPlaying(object? sender, RoutedEventArgs e)
        {
            PlaylistTable1.IsReadOnly = false;
            _isPlaying = false;

            var command = new Command
            {
                command_name = "playback_stop",
                target_path = "c_container::my_playlist_player"
            };
            var jsonRes = XmHandlerCommandExecute(_container, command);
            if (jsonRes?.error_code != 0)
            {
                Status.Text = $"Failed to stop playback: {jsonRes!.error_code} {jsonRes!.json_result_or_error_desc}";
                return;
            }
        }

        private async void OnShowSettingsDialog(object? sender, RoutedEventArgs e)
        {
            var json = await ConversionSettingsDialog.ShowDialog(this, _conversionJson, _isPlaying);
            if (json != null)
            {
                Debug.WriteLine($"Result: {json}");
                _conversionJson = json;
                var cSettings = ConversionSettingsViewModel.FromJson(json);
                ConversionStatus.Text = $"V: {cSettings.VideoWidth}x{cSettings.VideoHeight}@{cSettings.VideoFrameRate} A: {cSettings.AudioSampleRate}Hz, {cSettings.AudioChannels} channels";
                if (_container != null)
                {
                    UpdateConversion(_conversionJson);
                }
            }
        }

        private class FrameProcessingStats
        {
            public bool ContinueFrameReceiving { get; set; }
            public int FrameCount { get; set; }
            public required AudioPlayer AudioPlayer { get; set; }
            public required VideoPlayer VideoPlayer { get; set; }
        }

        private static unsafe readonly OnFrameCallback FrameCallbackHandler = (ref object userData, XmFrame frame) =>
        {
            try
            {
                if (frame == null) return true;

                var stats = (FrameProcessingStats)userData;

                if (frame?.frame_type == ObjectType.FrameVideo)
                {
                    stats.VideoPlayer.EnqueueVideo(frame);
                }
                if (frame?.frame_type == ObjectType.FrameAudio)
                {
                    stats.AudioPlayer.EnqueueAudio(frame);
                }
                stats.FrameCount++;
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in frame callback: {ex.Message}");
                return false;
            }
        };
    }
}