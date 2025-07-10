using System;
using System.Runtime.InteropServices;
using static Medialooks.XMedia.CApi;
using static Medialooks.XMedia.Enumerations;
using static Medialooks.XMedia.Structures;
using static Medialooks.XMedia.Objects;

namespace Medialooks.XMedia
{
    static public class Callbacks
    {
        public delegate bool OnFrameCallback(ref object userData, XmFrame frame);
        public delegate bool OnPacketCallback(ref object userData, XmPacket packet);
        public delegate Int64 OnTimerCallback(ref object userData, XmTimerTick timerTick);
        public delegate bool OnEventCallback(ref object userData, XmEvent xmEvent);

        static XmOnFrameCallback WrapCallback(OnFrameCallback userCallback)
        {
            return (IntPtr userDataPtr, XmFrame frame) =>
            {
                var userData = Utils.ConvertIntPtrToObject(userDataPtr);
                var res = userCallback(ref userData, frame);
                if (userData != Utils.ConvertIntPtrToObject(userDataPtr))
                {
                    Utils.UpdateObjectByIntPtr(userDataPtr, userData);
                }
                return res;
            };
        }

        public static XmFrameCallback FrameCallbackAdd<T>(XmHandler handler, ObjectType objectType,
            string jsonCallbackProps, ref T userData, OnFrameCallback onFrameCB, ref Error error) where T : class
        {
            XmError errorPtr;
            errorPtr.error_code = 0;
            errorPtr.error_category = IntPtr.Zero;

            var userDataPtr = Utils.ConvertObjectToIntPtr(ref userData);
            var wrappedCB = WrapCallback(onFrameCB);
            var frameCallbackPtr = XmFrameCallbackAdd(handler,
                                          objectType,
                                          jsonCallbackProps,
                                          userDataPtr,
                                          wrappedCB,
                                          out errorPtr);

            FillErrorInfo(ref errorPtr, ref error);

            if (frameCallbackPtr == IntPtr.Zero)
                return null;

            var frameCallback = new XmFrameCallback();
            Marshal.PtrToStructure(frameCallbackPtr, frameCallback);

            return frameCallback;
        }

        public static XmFrameCallback FrameCallbackAddRef(XmFrameCallback frameCallback)
        {
            var frameCallbackPtr = XmFrameCallbackAddRef(frameCallback);

            if (frameCallbackPtr == IntPtr.Zero)
                return null;

            return frameCallback;
        }

        public static bool FrameCallbackRelease(XmFrameCallback frameCallback)
        {
            var userData = frameCallback.cb_user_data;
            var res = XmFrameCallbackRelease(frameCallback);
            Utils.FreePinnedHandleByIntPtr(userData);
            return res;
        }

        public static bool FrameCallbackReleaseSafe(ref XmFrameCallback frameCallback)
        {
            bool res = XmFrameCallbackRelease(frameCallback);
            frameCallback = null;
            return res;
        }

        static XmOnPacketCallback WrapCallback(OnPacketCallback userCallback)
        {
            return (IntPtr userDataPtr, XmPacket packet) =>
            {
                var userData = Utils.ConvertIntPtrToObject(userDataPtr);
                var res = userCallback(ref userData, packet);
                if (userData != Utils.ConvertIntPtrToObject(userDataPtr))
                {
                    Utils.UpdateObjectByIntPtr(userDataPtr, userData);
                }
                return res;
            };
        }

        public static XmPacketCallback PacketCallbackAdd<T>(XmHandler handler, ObjectType objectType,
            string jsonCallbackProps, ref T userData, OnPacketCallback onPacketCB, ref Error error) where T : class
        {
            XmError errorPtr;
            errorPtr.error_code = 0;
            errorPtr.error_category = IntPtr.Zero;

            var userDataPtr = Utils.ConvertObjectToIntPtr(ref userData);
            var wrappedCB = WrapCallback(onPacketCB);
            var packetCallbackPtr = XmPacketCallbackAdd(handler,
                                          objectType,
                                          jsonCallbackProps,
                                          userDataPtr,
                                          wrappedCB,
                                          out errorPtr);

            FillErrorInfo(ref errorPtr, ref error);

            if (packetCallbackPtr == IntPtr.Zero)
                return null;

            var packetCallback = new XmPacketCallback();
            Marshal.PtrToStructure(packetCallbackPtr, packetCallback);

            return packetCallback;
        }

        public static XmPacketCallback PacketCallbackAddRef(XmPacketCallback packetCallback)
        {
            var packetCallbackPtr = XmPacketCallbackAddRef(packetCallback);

            if (packetCallbackPtr == IntPtr.Zero)
                return null;

            return packetCallback;
        }

        public static bool PacketCallbackRelease(XmPacketCallback packetCallback)
        {
            var userData = packetCallback.cb_user_data;
            var res = XmPacketCallbackRelease(packetCallback);
            Utils.FreePinnedHandleByIntPtr(userData);
            return res;
        }

        public static bool PacketCallbackReleaseSafe(ref XmPacketCallback packetCallback)
        {
            bool res = XmPacketCallbackRelease(packetCallback);
            packetCallback = null;
            return res;
        }


        static XmOnTimerCallback WrapCallback(OnTimerCallback userCallback)
        {
            return (IntPtr userDataPtr, XmTimerTickNative tick) =>
            {
                var userData = Utils.ConvertIntPtrToObject(userDataPtr);
                var res = userCallback(ref userData, XmTimerTick.FromNative(tick));
                if (userData != Utils.ConvertIntPtrToObject(userDataPtr))
                {
                    Utils.UpdateObjectByIntPtr(userDataPtr, userData);
                }
                return res;
            };
        }

        public static XmTimerCallback TimerCallbackAdd<T>(Int64 scheduledTime, ref T userData, OnTimerCallback onTimerCB, ref Error error, XmClock clock = null) where T : class
        {
            XmError errorPtr;
            errorPtr.error_code = 0;
            errorPtr.error_category = IntPtr.Zero;

            var userDataPtr = Utils.ConvertObjectToIntPtr(ref userData);
            var wrappedCB = WrapCallback(onTimerCB);
            var timerCallbackPtr = XmTimerCallbackAdd(scheduledTime,
                                          userDataPtr,
                                          wrappedCB,
                                          clock,
                                          out errorPtr);

            FillErrorInfo(ref errorPtr, ref error);

            if (timerCallbackPtr == IntPtr.Zero)
                return null;

            var timerCallback = new XmTimerCallback();
            Marshal.PtrToStructure(timerCallbackPtr, timerCallback);

            return timerCallback;
        }

        public static bool TimerCallbackStop(XmTimerCallback timerCallback, ref Error error)
        {
            var errorPtr = new XmError()
            {
                error_code = 0,
                error_category = IntPtr.Zero
            };

            bool res = XmTimerCallbackStop(timerCallback, out errorPtr);

            FillErrorInfo(ref errorPtr, ref error);

            return res;
        }

        public static bool TimerCallbackIsStopped(XmTimerCallback timerCallback, ref Error error)
        {
            var errorPtr = new XmError()
            {
                error_code = 0,
                error_category = IntPtr.Zero
            };

            bool res = XmTimerCallbackIsStopped(timerCallback, out errorPtr);

            FillErrorInfo(ref errorPtr, ref error);

            return res;
        }

        public static XmTimerCallback TimerCallbackAddRef(XmTimerCallback timerCallback)
        {
            var timerCallbackPtr = XmTimerCallbackAddRef(timerCallback);

            if (timerCallbackPtr == IntPtr.Zero)
                return null;

            return timerCallback;
        }

        public static bool TimerCallbackRelease(XmTimerCallback timerCallback)
        {
            var userData = timerCallback.cb_user_data;
            var res = XmTimerCallbackRelease(timerCallback);
            Utils.FreePinnedHandleByIntPtr(userData);
            return res;
        }

        public static bool TimerCallbackReleaseSafe(ref XmTimerCallback timerCallback)
        {
            bool res = XmTimerCallbackRelease(timerCallback);
            timerCallback = null;
            return res;
        }

        static XmOnEventCallback WrapCallback(OnEventCallback userCallback)
        {
            return (IntPtr userDataPtr, XmEventNative xmEvent) =>
            {
                var userData = Utils.ConvertIntPtrToObject(userDataPtr);
                var res = userCallback(ref userData, XmEvent.FromNative(xmEvent));
                if (userData != Utils.ConvertIntPtrToObject(userDataPtr))
                {
                    Utils.UpdateObjectByIntPtr(userDataPtr, userData);
                }
                return res;
            };
        }

        unsafe public static XmEventCallback EventCallbackAdd<T>(XmHandler handler, MediaEventType[] eventTypes,
            ref T userData, OnEventCallback onEventCB, ref Error error) where T : class
        {
            XmError errorPtr;
            errorPtr.error_code = 0;
            errorPtr.error_category = IntPtr.Zero;

            var userDataPtr = Utils.ConvertObjectToIntPtr(ref userData);
            var wrappedCB = WrapCallback(onEventCB);
            IntPtr eventCallbackPtr;
            fixed (MediaEventType* eventTypesPtr = eventTypes) // To check
            { 
                eventCallbackPtr = XmEventCallbackAdd(handler,
                                          (UInt32)eventTypes.Length,
                                          eventTypesPtr,
                                          userDataPtr,
                                          wrappedCB,
                                          out errorPtr);
            }

            FillErrorInfo(ref errorPtr, ref error);

            if (eventCallbackPtr == IntPtr.Zero)
                return null;

            var eventCallback = new XmEventCallbackNative();
            Marshal.PtrToStructure(eventCallbackPtr, eventCallback);

            return XmEventCallback.FromNative(eventCallback);
        }

        public static XmEventCallback EventCallbackAddRef(XmEventCallback eventCallback)
        {
            var eventCallbackPtr = XmEventCallbackAddRef(eventCallback);

            if (eventCallbackPtr == IntPtr.Zero)
                return null;

            return eventCallback;
        }

        public static bool EventCallbackRelease(XmEventCallback eventCallback)
        {
            var userData = eventCallback.cb_user_data;
            var res = XmEventCallbackRelease(eventCallback);
            Utils.FreePinnedHandleByIntPtr(userData);
            return res;
        }

        public static bool EventCallbackReleaseSafe(ref XmEventCallback eventCallback)
        {
            bool res = XmEventCallbackRelease(eventCallback);
            eventCallback = null;
            return res;
        }

        public static XmEvent EventAddRef(XmEvent xmEvent)
        {
            var eventPtr = XmEventAddRef(xmEvent);

            if (eventPtr == IntPtr.Zero)
                return null;

            return xmEvent;
        }

        public static bool EventRelease(XmEvent xmEvent)
        {
            return XmEventRelease(xmEvent);
        }

        public static bool EventReleaseSafe(ref XmEvent xmEvent)
        {
            bool res = XmEventRelease(xmEvent);
            xmEvent = null;
            return res;
        }
    }
}
