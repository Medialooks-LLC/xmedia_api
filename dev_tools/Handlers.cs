using System;
using System.Linq;
using System.Runtime.InteropServices;
using static Medialooks.XMedia.Enumerations;
using static Medialooks.XMedia.Structures;
using static Medialooks.XMedia.Objects;
using static Medialooks.XMedia.Handlers;
//using XMediaAPI.DevTools;

namespace Medialooks.XMedia.DevTools
{
    static public class Handlers
    {
        public static string Version()
        {
            return "0.0.1";
        }

        // Handlers enumerate
        public static HandlersArray HandlersGet(XmHandler rootHandler)
        {
            var nativeHandlers = XmHandlersGet(rootHandler);
            if(nativeHandlers == null) return null;

            return new HandlersArray(nativeHandlers);
        }

        // Input/output
        public static FramesArray HandlerFrameInputs(XmHandler handler)
        {
            var nativeInputs = XmHandlerFrameInputs(handler);
            if(nativeInputs == null) return null;

            return new FramesArray(nativeInputs);
        }
        public static FramesArray HandlerFrameOutputs(XmHandler handler)
        {
            var nativeOutputs = XmHandlerFrameOutputs(handler);
            if(nativeOutputs == null) return null;

            return new FramesArray(nativeOutputs);
        }

        public static PacketsArray HandlerPacketInputs(XmHandler handler)
        {
            var nativeInputs = XmHandlerPacketInputs(handler);
            if(nativeInputs == null) return null;

            return new PacketsArray(nativeInputs);
        }
        public static PacketsArray HandlerPacketOutputs(XmHandler handler)
        {
            var nativeOutputs = XmHandlerPacketOutputs(handler);
            if(nativeOutputs == null) return null;

            return new PacketsArray(nativeOutputs);
        }
    }
}
