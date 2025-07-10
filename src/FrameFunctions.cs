using System;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using static Medialooks.XMedia.CApi;
using static Medialooks.XMedia.Enumerations;
using static Medialooks.XMedia.Structures;
using static Medialooks.XMedia.Objects;

namespace Medialooks.XMedia
{
    static public class FrameFunctions
    {
        public static string Version()
        {
            return "0.0.1";
        }

        public static XmFrame XmFrameVideoAlloc(Uids uids,
                                     Time time,
                                     XmVideoData videoData,
                                     StreamInfo streamInfo,
                                     XmProgramArray programsArray,
                                     XmFrameSideDataArray frameSideDataArray,
                                     ref Error error)
        {
            var errorPtr = new XmError()
            {
                error_code = 0,
                error_category = IntPtr.Zero
            };
            var framePtr = CApi.XmFrameVideoAlloc(uids, time, videoData, streamInfo, programsArray, frameSideDataArray, out errorPtr);

            FillErrorInfo(ref errorPtr, ref error);

            if (framePtr == IntPtr.Zero)
                return null;

            XmFrame frame = Marshal.PtrToStructure<XmFrame>(framePtr);

            return frame;
        }

        public static XmFrame XmFrameAudioAlloc(Uids uids,
                                     Time time,
                                     XmAudioData audioData,
                                     StreamInfo streamInfo,
                                     XmProgramArray programsArray,
                                     XmFrameSideDataArray frameSideDataArray,
                                     ref Error error)
        {
            var errorPtr = new XmError()
            {
                error_code = 0,
                error_category = IntPtr.Zero
            };
            var framePtr = CApi.XmFrameAudioAlloc(uids, time, audioData, streamInfo, programsArray, frameSideDataArray, out errorPtr);

            FillErrorInfo(ref errorPtr, ref error);

            if (framePtr == IntPtr.Zero)
                return null;

            XmFrame frame = Marshal.PtrToStructure<XmFrame>(framePtr);

            return frame;
        }

        public static XmFrame XmFrameAddRef(XmFrame frame)
        {
            var framePtr = CApi.XmFrameAddRef(frame);

            if (framePtr == IntPtr.Zero)
                return null;

            return frame;
        }

        public static XmFrame XmFrameMakeCopy(XmFrame srcFrame,
                                                Uids? uids = null,
                                                Time? time = null,
                                                StreamInfo streamInfo = null,
                                                XmProgramArray programArray = null,
                                                XmFrameSideDataArray frameSideDataArray = null,
                                                FrameCopyFlags copyFlags = FrameCopyFlags.kFrameCopyReference)
        {
            IntPtr updatedUids = IntPtr.Zero;
            GCHandle gcUids = new();
            if (uids != null)
            {
                gcUids = GCHandle.Alloc(uids!.Value, GCHandleType.Pinned);
                updatedUids = gcUids.AddrOfPinnedObject();
            }

            IntPtr updatedTime = IntPtr.Zero;
            GCHandle gcTime = new();
            if (uids != null)
            {
                gcTime = GCHandle.Alloc(time!.Value, GCHandleType.Pinned);
                updatedTime = gcTime.AddrOfPinnedObject();
            }
            else
            {
                updatedTime = IntPtr.Zero;
            }

            var framePtr = CApi.XmFrameMakeCopy(srcFrame, updatedUids, updatedTime, streamInfo, programArray, frameSideDataArray, copyFlags);

            if (updatedUids != IntPtr.Zero && gcUids.IsAllocated)
            {
                gcUids.Free();
            }
            if (updatedTime != IntPtr.Zero && gcTime.IsAllocated)
            {
                gcTime.Free();
            }

            if (framePtr == IntPtr.Zero)
                return null;

            XmFrame frame = Marshal.PtrToStructure<XmFrame>(framePtr);

            return frame;
        }

        public static StreamInfo XmFrameStreamInfoGet(XmFrame frame)
        {
            if (frame == null) return null;
            var streamInfoPtr = CApi.XmFrameStreamInfoGet(frame);

            if (streamInfoPtr == IntPtr.Zero)
                return null;

            var streamInfo = new StreamInfo();
            Marshal.PtrToStructure(streamInfoPtr, streamInfo);

            return streamInfo;
        }

        public static XmProgramArray XmFrameProgramsGet(XmFrame frame)
        {
            if (frame == null) return null;
            var programsPtr = CApi.XmFrameProgramsGet(frame);

            if (programsPtr == IntPtr.Zero)
                return null;

            var programs = new XmProgramArray();
            Marshal.PtrToStructure(programsPtr, programs);

            return programs;
        }

        public static XmAudioData XmFrameAudioDataGet(XmFrame frame)
        {
            if (frame == null) return null;
            var audioDataPtr = CApi.XmFrameAudioDataGet(frame);

            if (audioDataPtr == IntPtr.Zero)
                return null;

            var audioData = new XmAudioData();
            Marshal.PtrToStructure(audioDataPtr, audioData);

            return audioData;
        }

        public static XmVideoData XmFrameVideoDataGet(XmFrame frame)
        {
            if (frame == null) return null;
            var videoDataPtr = CApi.XmFrameVideoDataGet(frame);

            if (videoDataPtr == IntPtr.Zero)
                return null;

            var videoData = new XmVideoData();
            Marshal.PtrToStructure(videoDataPtr, videoData);

            return videoData;
        }

        public static XmSubtitle XmFrameSubtitleGet(XmFrame frame)
        {
            if (frame == null) return null;
            var subtitlePtr = CApi.XmFrameSubtitleGet(frame);

            if (subtitlePtr == IntPtr.Zero)
                return null;

            var subtitle = new XmSubtitle();
            Marshal.PtrToStructure(subtitlePtr, subtitle);

            return subtitle;
        }

        public static XmFrameSideDataArray XmFrameSideDataGet(XmFrame frame)
        {
            if (frame == null) return null;
            var frameSideDataArrayPtr = CApi.XmFrameSideDataGet(frame);

            if (frameSideDataArrayPtr == IntPtr.Zero)
                return null;

            var frameSideDataArray = new XmFrameSideDataArray();
            Marshal.PtrToStructure(frameSideDataArrayPtr, frameSideDataArray);

            return frameSideDataArray;
        }

        public static bool XmFrameReleaseSafe(ref XmFrame frame)
        {
            if (frame == null) return false;
            var res = CApi.XmFrameRelease(frame);
            frame = null;
            return res;
        }

        public static bool XmFrameRelease(XmFrame frame)
        {
            if (frame == null) return false;
            return CApi.XmFrameRelease(frame);
        }

        public static XmFramesArray XmFramesArrayAddRef(XmFramesArray frames)
        {
            var framesPtr = CApi.XmFramesArrayAddRef(frames);

            if (framesPtr == IntPtr.Zero)
                return null;

            return frames;
        }

        public static bool XmFramesArrayReleaseSafe(ref XmFramesArray frames)
        {
            var res = CApi.XmFramesArrayRelease(frames);
            frames = null;
            return res;
        }

        public static bool XmFramesArrayRelease(XmFramesArray frames)
        {
            return CApi.XmFramesArrayRelease(frames);
        }
    }
}
