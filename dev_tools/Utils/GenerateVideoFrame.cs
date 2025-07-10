using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static Medialooks.XMedia.Structures;
using static Medialooks.XMedia.Objects;
using static Medialooks.XMedia.FrameFunctions;

namespace Medialooks.XMedia.DevTools
{
    static public partial class Utils
    {
        internal static void DrawCharacterBGRA(char c, int x, int y, int scale,
                                     byte[] frame, int width, int height)
        {
            if (c < 32 || c > 126) return;

            byte[] charData = PixelFont[c - 32];

            for (int dy = 0; dy < 7; dy++)
            {
                for (int dx = 0; dx < 5; dx++)
                {
                    if ((charData[dy] & (1 << (4 - dx))) != 0)
                    {
                        for (int sy = 0; sy < scale; sy++)
                        {
                            for (int sx = 0; sx < scale; sx++)
                            {
                                int px = x + dx * scale + sx;
                                int py = y + dy * scale + sy;

                                if (px >= 0 && px < width && py >= 0 && py < height)
                                {
                                    int offset = (py * width + px) * 4;
                                    frame[offset + 0] = 255; // B
                                    frame[offset + 1] = 255; // G
                                    frame[offset + 2] = 255; // R
                                    frame[offset + 3] = 255; // A
                                }
                            }
                        }
                    }
                }
            }
        }
        public static void DrawStringBGRA(string text, int x, int y, int scale,
                                 byte[] frame, int width, int height)
        {
            for (int i = 0; i < text.Length; i++)
            {
                DrawCharacterBGRA(text[i], x + i * 6 * scale, y, scale, frame, width, height);
            }
        }
        public static byte[] GenerateBGRAVideoFrameData(int frameNum, double capturedTimeMs,
                                           int width = 720, int height = 576, double durationMs = 40)
        {
            var videoData = new byte[width * height * 4];
            Array.Fill(videoData, (byte)127);

            int pos = frameNum % width;
            for (int i = 0; i < height; i++)
            {
                int offset = (i * width + pos) * 4;
                videoData[offset + 0] = 255; // B
                videoData[offset + 1] = 255; // G
                videoData[offset + 2] = 255; // R
                videoData[offset + 3] = 255; // A
            }

            string text = $"Frame: {frameNum}";
            int scale = 8;
            int textWidth = text.Length * 6 * scale;
            int startX = (width - textWidth) / 2;
            int startY = (height - 7 * scale) / 2;
            DrawStringBGRA(text, startX, startY, scale, videoData, width, height);

            text = $"time: {capturedTimeMs:F0}ms";
            scale = 2;
            DrawStringBGRA(text, 4, 4, scale, videoData, width, height);

            return videoData;
        }
        private static readonly UInt64 video_stream_uid;
        private static readonly UInt64 video_group_uid;
        private static readonly UInt64 video_source_uid;
        //TODO: add EOS processing
        public static XmFrame GenerateXmVideoFrame(int frameNum, double capturedTimeMs, bool eos = false,
                                           int width = 720, int height = 576, double durationMs = 40)
        {
            var uids = new Uids() { stream_uid = video_stream_uid, group_uid = video_group_uid, source_uid = video_source_uid };
            var time = new Time();
            //_frame_num * time64::FromMsec(_dur_ms); //2Think: may be add time utils to API?
            time.timestamp = (UInt64)(frameNum * (durationMs * 10_000));

            var formatV = new FormatV()
            {
                pixel_format = "bgra",
                picture_ar = new Rational() { num = 5, den = 4 },
                width = width,
                height = height,
                fields_order = Enumerations.FieldsOrder.Progressive,
                frame_rate = new Rational() { num = 25, den = 1 }
            };

            var streamInfo = new StreamInfo();
            var programsArray = new XmProgramArray();
            var frameSideDataArray = new XmFrameSideDataArray();

            var error = new Error();
            XmFrame frame;
            unsafe
            {
                fixed (byte* pVideo = GenerateBGRAVideoFrameData(frameNum, capturedTimeMs, width, height, durationMs))
                {
                    var plane_v = new XmPlaneV()
                    {
                        video_p = (IntPtr)pVideo,
                        height = height,
                        width = width,
                        row_bytes = width * 4 // 4 colors
                    };
                    var planesPtr = Marshal.AllocHGlobal(Marshal.SizeOf<XmPlaneV>());
                    Marshal.StructureToPtr(plane_v, planesPtr, false);

                    var videoData = new XmVideoData
                    {
                        format = formatV,
                        nb_planes = 1,
                        p_planes = planesPtr
                    };

                    frame = XmFrameVideoAlloc(uids,
                                     time,
                                     videoData,
                                     streamInfo,
                                     programsArray,
                                     frameSideDataArray,
                                     ref error);
                }
            }
            if (error.Code != 0)
            {
                Console.WriteLine($"Some error occured during frame creation: {error}");
            }

            return frame;
        }

    }
}
