using System;
using System.Runtime.InteropServices;
using static Medialooks.XMedia.CApi;
using static Medialooks.XMedia.Enumerations;
using static Medialooks.XMedia.Structures;
using static Medialooks.XMedia.Objects;

namespace Medialooks.XMedia
{
    static public class Functions
    {
        public static string Version()
        {
            return "0.0.1";
        }

        public static Int64 XmClockTime(XmClock clock)
        {
            return CApi.XmClockTime(clock);
        }

        public static UInt64 XmNextUid()
        {
            return CApi.XmNextUid();
        }

        public static bool XmFrameSaveToFile(XmFrame frame,
                             string destPath,
                             string hintsJson,
                             ref Error error)
        {
            var errorPtr = new XmError()
            {
                error_code = 0,
                error_category = IntPtr.Zero
            };

            var res = CApi.XmFrameSaveToFile(frame, destPath, hintsJson, out errorPtr);

            FillErrorInfo(ref errorPtr, ref error);

            return res;
        }

        public static bool XmFrameSaveToFile(XmFramesArray frames,
                             string destPath,
                             string hintsJson,
                             ref Error error)
        {
            var errorPtr = new XmError()
            {
                error_code = 0,
                error_category = IntPtr.Zero
            };

            var res = CApi.XmFramesArraySaveToFile(frames, destPath, hintsJson, out errorPtr);

            FillErrorInfo(ref errorPtr, ref error);

            return res;
        }

        public static XmFramesArray XmAudioFramesLoadFromFile(string srcPath,
                             FormatA desiredFormat,
                             UInt64 maxCount,
                             string hintsJson,
                             ref Error error)
        {
            var errorPtr = new XmError()
            {
                error_code = 0,
                error_category = IntPtr.Zero
            };

            var framesPtr = CApi.XmAudioFramesLoadFromFile(srcPath,
                             desiredFormat,
                             maxCount,
                             hintsJson,
                             out errorPtr);

            FillErrorInfo(ref errorPtr, ref error);

            if (error.Code != 0 || framesPtr == IntPtr.Zero)
            {
                return null;
            }

            var frames = new XmFramesArray();
            Marshal.PtrToStructure(framesPtr, frames);

            return frames;
        }

        public static XmFramesArray XmVideoFramesLoadFromFile(string srcPath,
                             FormatV desiredFormat,
                             UInt64 maxCount,
                             string hintsJson,
                             ref Error error)
        {
            var errorPtr = new XmError()
            {
                error_code = 0,
                error_category = IntPtr.Zero
            };

            var framesPtr = CApi.XmVideoFramesLoadFromFile(srcPath,
                             desiredFormat,
                             maxCount,
                             hintsJson,
                             out errorPtr);

            FillErrorInfo(ref errorPtr, ref error);

            if (error.Code != 0 || framesPtr == IntPtr.Zero)
            {
                return null;
            }

            var frames = new XmFramesArray();
            Marshal.PtrToStructure(framesPtr, frames);

            return frames;
        }
    }
}
