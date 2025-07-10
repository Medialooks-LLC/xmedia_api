using System;
using System.Linq;
using System.Runtime.InteropServices;
using static Medialooks.XMedia.CApi;
using static Medialooks.XMedia.Enumerations;
using static Medialooks.XMedia.Structures;
using static Medialooks.XMedia.Objects;

namespace Medialooks.XMedia
{
    static public class Handlers
    {
        public static string Version()
        {
            return "0.0.1";
        }

        // Live cicle
        public static XmHandler XmHandlerCreateByDesc(HandlerDesc description, ref Error error)
        {
            XmError errorPtr;
            errorPtr.error_code = 0;
            errorPtr.error_category = IntPtr.Zero;

            var handlerPtr = CApi.XmHandlerCreateByDesc(description, out errorPtr);

            FillErrorInfo(ref errorPtr, ref error);

            if (handlerPtr == IntPtr.Zero)
                return null;

            var handler = new XmHandler();
            Marshal.PtrToStructure(handlerPtr, handler);

            return handler;
        }
        unsafe public static XmHandler XmPacketsHandlerCreateByDesc(HandlerDesc handlerDesc,
                                                XmPacket[] inputPackets,
                                                ref Error error)
        {
            XmError errorPtr;
            errorPtr.error_code = 0;
            errorPtr.error_category = IntPtr.Zero;

            IntPtr handlerPtr = IntPtr.Zero;

            IntPtr[] packetsPointers = new IntPtr[inputPackets.Length];
            GCHandle[] handles = new GCHandle[inputPackets.Length];
            for (int i = 0; i < inputPackets.Length; i++)
            {
                handles[i] = GCHandle.Alloc(inputPackets[i], GCHandleType.Pinned);
                packetsPointers[i] = handles[i].AddrOfPinnedObject();
            }
            IntPtr packetsArrayPtr = IntPtr.Zero;
            GCHandle packetsArrayHandle = default;
            try
            {
                packetsArrayHandle = GCHandle.Alloc(packetsPointers, GCHandleType.Pinned);
                packetsArrayPtr = packetsArrayHandle.AddrOfPinnedObject();

                handlerPtr = CApi.XmPacketsHandlerCreateByDesc(
                    handlerDesc,
                    inputPackets.Length,
                    (XmPacket**)packetsArrayPtr,
                    out errorPtr);

            }
            finally
            {
                foreach (var handle in handles)
                {
                    if (handle.IsAllocated)
                        handle.Free();
                }

                if (packetsArrayHandle.IsAllocated)
                    packetsArrayHandle.Free();
            }

            FillErrorInfo(ref errorPtr, ref error);

            if (handlerPtr == IntPtr.Zero)
                return null;

            var handler = new XmHandler();
            Marshal.PtrToStructure(handlerPtr, handler);

            return handler;
        }
        unsafe public static XmHandler XmFramesHandlerCreateByDesc(HandlerDesc handlerDesc,
                                                        XmFrame[] inputFrames,
                                                        ref Error error)
        {
            XmError errorPtr;
            errorPtr.error_code = 0;
            errorPtr.error_category = IntPtr.Zero;

            IntPtr handlerPtr = IntPtr.Zero;

            IntPtr[] framePointers = new IntPtr[inputFrames.Length];
            GCHandle[] handles = new GCHandle[inputFrames.Length];
            for (int i = 0; i < inputFrames.Length; i++)
            {
                handles[i] = GCHandle.Alloc(inputFrames[i], GCHandleType.Pinned);
                framePointers[i] = handles[i].AddrOfPinnedObject();
            }
            IntPtr framesArrayPtr = IntPtr.Zero;
            GCHandle framesArrayHandle = default;
            try
            {
                framesArrayHandle = GCHandle.Alloc(framePointers, GCHandleType.Pinned);
                framesArrayPtr = framesArrayHandle.AddrOfPinnedObject();

                handlerPtr = CApi.XmFramesHandlerCreateByDesc(
                    handlerDesc,
                    inputFrames.Length,
                    (XmFrame**)framesArrayPtr,
                    out errorPtr);

            }
            finally
            {
                foreach (var handle in handles)
                {
                    if (handle.IsAllocated)
                        handle.Free();
                }

                if (framesArrayHandle.IsAllocated)
                    framesArrayHandle.Free();
            }

            FillErrorInfo(ref errorPtr, ref error);

            if (handlerPtr == IntPtr.Zero)
                return null;

            var handler = new XmHandler();
            Marshal.PtrToStructure(handlerPtr, handler);

            return handler;
        }

        public static bool XmHandlerClose(XmHandler handler)
        {
            return CApi.XmHandlerClose(handler);
        }
        public static XmHandler XmHandlerAddRef(XmHandler handler)
        {
            var handlerPtr = CApi.XmHandlerAddRef(handler);

            if (handlerPtr == IntPtr.Zero)
                return null;

            return handler;
        }
        public static bool XmHandlerReleaseSafe(ref XmHandler handler)
        {
            bool res = CApi.XmHandlerRelease(handler);
            handler = null;
            return res;
        }
        public static bool XmHandlerRelease(XmHandler handler)
        {
            return CApi.XmHandlerRelease(handler);
        }

        // Persistance
        public static XmHandler XmHandlerCreate(string jsonDesc, Int32 loadFlag, ref Error error)
        {
            XmError errorPtr;
            errorPtr.error_code = 0;
            errorPtr.error_category = IntPtr.Zero;

            var handlerPtr = CApi.XmHandlerCreate(jsonDesc, loadFlag, out errorPtr);

            FillErrorInfo(ref errorPtr, ref error);

            if (handlerPtr == IntPtr.Zero)
                return null;

            var handler = new XmHandler();
            Marshal.PtrToStructure(handlerPtr, handler);

            return handler;
        }
        unsafe public static XmHandler XmPacketsHandlerCreate(string jsonDesc,
                                                        Int32 loadFlags,
                                                        string jsonInitProps,
                                                        XmPacket[] inputPackets,
                                                        ref Error error)
        {
            XmError errorPtr;
            errorPtr.error_code = 0;
            errorPtr.error_category = IntPtr.Zero;

            IntPtr handlerPtr = IntPtr.Zero;

            IntPtr[] packetPointers = new IntPtr[inputPackets.Length];
            GCHandle[] handles = new GCHandle[inputPackets.Length];
            for (int i = 0; i < inputPackets.Length; i++)
            {
                handles[i] = GCHandle.Alloc(inputPackets[i], GCHandleType.Pinned);
                packetPointers[i] = handles[i].AddrOfPinnedObject();
            }
            IntPtr packetsArrayPtr = IntPtr.Zero;
            GCHandle packetsArrayHandle = default;
            try
            {
                packetsArrayHandle = GCHandle.Alloc(packetPointers, GCHandleType.Pinned);
                packetsArrayPtr = packetsArrayHandle.AddrOfPinnedObject();

                handlerPtr = CApi.XmPacketsHandlerCreate(
                    jsonDesc,
                    loadFlags,
                    jsonInitProps,
                    inputPackets.Length,
                    (XmPacket**)packetsArrayPtr,
                    out errorPtr);

            }
            finally
            {
                foreach (var handle in handles)
                {
                    if (handle.IsAllocated)
                        handle.Free();
                }

                if (packetsArrayHandle.IsAllocated)
                    packetsArrayHandle.Free();
            }

            FillErrorInfo(ref errorPtr, ref error);

            if (handlerPtr == IntPtr.Zero)
                return null;

            var handler = Marshal.PtrToStructure<XmHandler>(handlerPtr);

            return handler;
        }
        unsafe public static XmHandler XmFramesHandlerCreate(string jsonDesc,
                                                                Int32 loadFlags,
                                                                string jsonInitProps,
                                                                XmFrame[] inputFrames,
                                                                ref Error error)
        {
            XmError errorPtr;
            errorPtr.error_code = 0;
            errorPtr.error_category = IntPtr.Zero;

            IntPtr handlerPtr = IntPtr.Zero;

            IntPtr[] framePointers = new IntPtr[inputFrames.Length];
            GCHandle[] handles = new GCHandle[inputFrames.Length];
            for (int i = 0; i < inputFrames.Length; i++)
            {
                handles[i] = GCHandle.Alloc(inputFrames[i], GCHandleType.Pinned);
                framePointers[i] = handles[i].AddrOfPinnedObject();
            }
            IntPtr framesArrayPtr = IntPtr.Zero;
            GCHandle framesArrayHandle = default;
            try
            {
                framesArrayHandle = GCHandle.Alloc(framePointers, GCHandleType.Pinned);
                framesArrayPtr = framesArrayHandle.AddrOfPinnedObject();

                handlerPtr = CApi.XmFramesHandlerCreate(
                    jsonDesc,
                    loadFlags,
                    jsonInitProps,
                    inputFrames.Length,
                    (XmFrame**)framesArrayPtr,
                    out errorPtr);

            }
            finally
            {
                foreach (var handle in handles)
                {
                    if (handle.IsAllocated)
                        handle.Free();
                }

                if (framesArrayHandle.IsAllocated)
                    framesArrayHandle.Free();
            }

            FillErrorInfo(ref errorPtr, ref error);

            if (handlerPtr == IntPtr.Zero)
                return null;

            var handler = Marshal.PtrToStructure<XmHandler>(handlerPtr);

            return handler;
        }
        public static XmJsonResult XmHandlerStore(XmHandler handler, Int32 storeFlags)
        {
            var jsonResultPtr = CApi.XmHandlerStore(handler, storeFlags);

            if (jsonResultPtr == IntPtr.Zero)
                return null;

            var jsonResult = new XmJsonResult();
            Marshal.PtrToStructure(jsonResultPtr, jsonResult);

            return jsonResult;
        }

        // Json result
        public static XmJsonResult XmJsonResultAddRef(XmJsonResult jsonResult)
        {
            var jsonResultPtr = CApi.XmJsonResultAddRef(jsonResult);

            if (jsonResultPtr == IntPtr.Zero)
                return null;

            return jsonResult;
        }
        public static bool XmJsonResultReleaseSafe(ref XmJsonResult jsonResult)
        {
            var res = CApi.XmJsonResultRelease(jsonResult);
            jsonResult = null;
            return res;
        }
        public static bool XmJsonResultRelease(XmJsonResult jsonResult)
        {
            return CApi.XmJsonResultRelease(jsonResult);
        }

        // Commands
        // Sample of commands:
        // - {"output_start", "", "c_container"})
        // - {"output_stop", "", "c_container"})
        public static XmJsonResult XmHandlerCommandExecute(XmHandler sourceHandler, Command command)
        {
            var jsonResultPtr = CApi.XmHandlerCommandExecute(sourceHandler, command);

            if (jsonResultPtr == IntPtr.Zero)
                return null;

            var jsonResult = new XmJsonResult();
            Marshal.PtrToStructure(jsonResultPtr, jsonResult);

            return jsonResult;
        }

        // Handlers enumerate

        public static XmHandlersArray XmHandlersGet(XmHandler rootHandler)
        {
            var nativeHandlersPtr = CApi.XmHandlersGet(rootHandler);
            if (nativeHandlersPtr == IntPtr.Zero)
                return null;

            return Marshal.PtrToStructure<XmHandlersArray>(nativeHandlersPtr);
        }

        public static XmHandlersArray XmHandlersArrayAddRef(XmHandlersArray handlersArray)
        {
            var handlersArrayPtr = CApi.XmHandlersArrayAddRef(handlersArray);

            if (handlersArrayPtr == IntPtr.Zero)
                return null;

            return handlersArray;
        }

        public static bool XmHandlersArrayReleaseSafe(ref XmHandlersArray handlersArray)
        {
            var res = CApi.XmHandlersArrayRelease(handlersArray);
            handlersArray = null;
            return res;
        }

        public static bool XmHandlersArrayRelease(XmHandlersArray handlersArray)
        {
            return CApi.XmHandlersArrayRelease(handlersArray);
        }

        // Required for get inner handler for callbacks & frame_get  IActiveContainer
        public static XmHandler XmHandlerOpen(XmHandler rootHandler, string handlerPath)
        {
            var handlerPtr = CApi.XmHandlerOpen(rootHandler, handlerPath);

            if (handlerPtr == IntPtr.Zero)
                return null;

            var handler = new XmHandler();
            Marshal.PtrToStructure(handlerPtr, handler);

            return handler;
        }

        public static HandlerStatus XmHandlerStatusGet(XmHandler handler)
        {
            var handlerStatusPtr = CApi.XmHandlerStatusGet(handler);

            if (handlerStatusPtr == IntPtr.Zero)
                return null;

            var handlerStatus = new HandlerStatus();
            Marshal.PtrToStructure(handlerStatusPtr, handlerStatus);

            return handlerStatus;
        }
        public static XmHandlerStat XmHandlerStatGet(XmHandler handler, string statPath)
        {
            var handlerStatPtr = CApi.XmHandlerStatGet(handler, statPath);

            if (handlerStatPtr == IntPtr.Zero)
                return null;

            var handlerStat = new XmHandlerStat();
            Marshal.PtrToStructure(handlerStatPtr, handlerStat);

            return handlerStat;
        }
        public static XmHandlerStat XmHandlerStatAddRef(XmHandlerStat handlerStat)
        {
            var handlerStatPtr = CApi.XmHandlerStatAddRef(handlerStat);

            if (handlerStatPtr == IntPtr.Zero)
                return null;

            return handlerStat;
        }
        public static bool XmHandlerStatReleaseSafe(ref XmHandlerStat handlerStat)
        {
            var res = CApi.XmHandlerStatRelease(handlerStat);
            handlerStat = null;
            return res;
        }
        public static bool XmHandlerStatRelease(XmHandlerStat handlerStat)
        {
            return CApi.XmHandlerStatRelease(handlerStat);
        }

        // Input/output
        public static XmFramesArray XmHandlerFrameInputs(XmHandler handler)
        {
            var nativeInputsPtr = CApi.XmHandlerFrameInputs(handler);
            if (nativeInputsPtr == IntPtr.Zero)
                return null;

            return Marshal.PtrToStructure<XmFramesArray>(nativeInputsPtr);
        }
        public static XmFramesArray XmHandlerFrameOutputs(XmHandler handler)
        {
            var nativeOutputsPtr = CApi.XmHandlerFrameOutputs(handler);
            if (nativeOutputsPtr == IntPtr.Zero)
                return null;

            return Marshal.PtrToStructure<XmFramesArray>(nativeOutputsPtr);
        }

        public static XmPacketsArray XmHandlerPacketInputs(XmHandler handler)
        {
            var nativeInputsPtr = CApi.XmHandlerPacketInputs(handler);
            if (nativeInputsPtr == IntPtr.Zero)
                return null;

            return Marshal.PtrToStructure<XmPacketsArray>(nativeInputsPtr);
        }
        public static XmPacketsArray XmHandlerPacketOutputs(XmHandler handler)
        {
            var nativeOutputsPtr = CApi.XmHandlerPacketOutputs(handler);
            if (nativeOutputsPtr == IntPtr.Zero)
                return null;

            return Marshal.PtrToStructure<XmPacketsArray>(nativeOutputsPtr);
        }

        // Data put
        public static bool XmHandlerFramePut(XmHandler handler, XmFrame frame, HandlerPutFlags flags, ref Error error)
        {
            XmError errorPtr;
            errorPtr.error_code = 0;
            errorPtr.error_category = IntPtr.Zero;

            var result = CApi.XmHandlerFramePut(handler, frame, flags, out errorPtr);

            FillErrorInfo(ref errorPtr, ref error);

            return result;
        }
        public static bool XmHandlerPacketPut(XmHandler handler, XmPacket packet, HandlerPutFlags flags, ref Error error)
        {
            XmError errorPtr;
            errorPtr.error_code = 0;
            errorPtr.error_category = IntPtr.Zero;

            var result = CApi.XmHandlerPacketPut(handler, packet, flags, out errorPtr);

            FillErrorInfo(ref errorPtr, ref error);

            return result;
        }

        // Data get
        public static XmFrame XmHandlerFrameGet(XmHandler handler, HandlerGetFlags flags,
                                                string jsonHints, ref Error error)
        {
            XmError errorPtr;
            errorPtr.error_code = 0;
            errorPtr.error_category = IntPtr.Zero;

            var framePtr = CApi.XmHandlerFrameGet(handler, flags, jsonHints, out errorPtr);

            FillErrorInfo(ref errorPtr, ref error);

            if (framePtr == IntPtr.Zero)
                return null;

            XmFrame frame = Marshal.PtrToStructure<XmFrame>(framePtr);

            return frame;
        }
        public static XmPacket XmHandlerPacketGet(XmHandler handler, HandlerGetFlags flags,
                                                string jsonHints, ref Error error)
        {
            XmError errorPtr;
            errorPtr.error_code = 0;
            errorPtr.error_category = IntPtr.Zero;

            var packetPtr = CApi.XmHandlerPacketGet(handler, flags, jsonHints, out errorPtr);

            FillErrorInfo(ref errorPtr, ref error);

            if (packetPtr == IntPtr.Zero)
                return null;

            XmPacket packet = Marshal.PtrToStructure<XmPacket>(packetPtr);

            return packet;
        }

    }
}
