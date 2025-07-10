using System;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using static Medialooks.XMedia.Structures;
using static Medialooks.XMedia.Objects;

namespace Medialooks.XMedia
{
    static public class PacketFunctions
    {
        public static string Version()
        {
            return "0.0.1";
        }

        public static XmPacket XmPacketVideoAlloc(Uids uids,
                                     Time time,
                                     XmVideoData videoData,
                                     StreamInfo streamInfo,
                                     XmProgramArray programsArray,
                                     XmSideDataArray sideDataArray,
                                     ref Error error)
        {
            var errorPtr = new XmError()
            {
                error_code = 0,
                error_category = IntPtr.Zero
            };
            var framePtr = CApi.XmPacketVideoAlloc(uids, time, videoData, streamInfo, programsArray, sideDataArray, out errorPtr);

            FillErrorInfo(ref errorPtr, ref error);

            if (framePtr == IntPtr.Zero)
                return null;

            var frame = new XmPacket();
            Marshal.PtrToStructure(framePtr, frame);

            return frame;
        }

        public static XmPacket XmPacketAudioAlloc(Uids uids,
                                     Time time,
                                     XmAudioData audioData,
                                     StreamInfo streamInfo,
                                     XmProgramArray programsArray,
                                     XmSideDataArray sideDataArray,
                                     ref Error error)
        {
            var errorPtr = new XmError()
            {
                error_code = 0,
                error_category = IntPtr.Zero
            };
            var framePtr = CApi.XmPacketAudioAlloc(uids, time, audioData, streamInfo, programsArray, sideDataArray, out errorPtr);

            FillErrorInfo(ref errorPtr, ref error);

            if (framePtr == IntPtr.Zero)
                return null;

            var frame = new XmPacket();
            Marshal.PtrToStructure(framePtr, frame);

            return frame;
        }

        public static XmPacket XmPacketAddRef(XmPacket frame)
        {
            var framePtr = CApi.XmPacketAddRef(frame);

            if (framePtr == IntPtr.Zero)
                return null;

            return frame;
        }

        public static XmPacket XmPacketMakeCopy(XmPacket srcPacket,
                                                Uids? uids = null,
                                                Time? time = null,
                                                StreamInfo streamInfo = null,
                                                XmProgramArray programArray = null,
                                                XmSideDataArray packetSideDataArray = null)
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

            var packetPtr = CApi.XmPacketMakeCopy(srcPacket, updatedUids, updatedTime, streamInfo, programArray, packetSideDataArray);

            if (updatedUids != IntPtr.Zero && gcUids.IsAllocated)
            {
                gcUids.Free();
            }
            if (updatedTime != IntPtr.Zero && gcTime.IsAllocated)
            {
                gcTime.Free();
            }


            if (packetPtr == IntPtr.Zero)
                return null;

            var packet = new XmPacket();
            Marshal.PtrToStructure(packetPtr, packet);

            return packet;
        }

        public static StreamInfo XmPacketStreamInfoGet(XmPacket packet)
        {
            if (packet == null) return null;
            var streamInfoPtr = CApi.XmPacketStreamInfoGet(packet);

            if (streamInfoPtr == IntPtr.Zero)
                return null;

            var streamInfo = new StreamInfo();
            Marshal.PtrToStructure(streamInfoPtr, streamInfo);

            return streamInfo;
        }

        public static XmProgramArray XmPacketProgramsGet(XmPacket packet)
        {
            if (packet == null) return null;
            var programsPtr = CApi.XmPacketProgramsGet(packet);

            if (programsPtr == IntPtr.Zero)
                return null;

            var programs = new XmProgramArray();
            Marshal.PtrToStructure(programsPtr, programs);

            return programs;
        }

        public static XmAudioData XmPacketAudioDataGet(XmPacket packet)
        {
            if (packet == null) return null;
            var audioDataPtr = CApi.XmPacketAudioDataGet(packet);

            if (audioDataPtr == IntPtr.Zero)
                return null;

            var audioData = new XmAudioData();
            Marshal.PtrToStructure(audioDataPtr, audioData);

            return audioData;
        }

        public static XmVideoData XmPacketVideoDataGet(XmPacket packet)
        {
            if (packet == null) return null;
            var videoDataPtr = CApi.XmPacketVideoDataGet(packet);

            if (videoDataPtr == IntPtr.Zero)
                return null;

            var videoData = new XmVideoData();
            Marshal.PtrToStructure(videoDataPtr, videoData);

            return videoData;
        }

        public static XmSubtitle XmPacketSubtitleGet(XmPacket packet)
        {
            if (packet == null) return null;
            var subtitlePtr = CApi.XmPacketSubtitleGet(packet);

            if (subtitlePtr == IntPtr.Zero)
                return null;

            var subtitle = new XmSubtitle();
            Marshal.PtrToStructure(subtitlePtr, subtitle);

            return subtitle;
        }

        public static XmSideDataArray XmPacketSideDataGet(XmPacket packet)
        {
            if (packet == null) return null;
            var frameSideDataArrayPtr = CApi.XmPacketSideDataGet(packet);

            if (frameSideDataArrayPtr == IntPtr.Zero)
                return null;

            var frameSideDataArray = new XmSideDataArray();
            Marshal.PtrToStructure(frameSideDataArrayPtr, frameSideDataArray);

            return frameSideDataArray;
        }

        public static bool XmPacketReleaseSafe(ref XmPacket packet)
        {
            if (packet == null) return false;
            var res = CApi.XmPacketRelease(packet);
            packet = null;
            return res;
        }

        public static bool XmPacketRelease(XmPacket packet)
        {
            if (packet == null) return false;
            return CApi.XmPacketRelease(packet);
        }

        public static XmPacketsArray XmPacketsArrayAddRef(XmPacketsArray packets)
        {
            var packetsPtr = CApi.XmPacketsArrayAddRef(packets);

            if (packetsPtr == IntPtr.Zero)
                return null;

            return packets;
        }

        public static bool XmPacketsArrayReleaseSafe(ref XmPacketsArray packets)
        {
            var res = CApi.XmPacketsArrayRelease(packets);
            packets = null;
            return res;
        }

        public static bool XmPacketsArrayRelease(XmPacketsArray packets)
        {
            return CApi.XmPacketsArrayRelease(packets);
        }
    }
}
