using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Medialooks.XMedia
{
    static public class Utils
    {
        private static readonly Dictionary<object, int> PinnedObjects = new();
        private static readonly Dictionary<IntPtr, object> PinnedObjectsByHandle = new();

        public static IntPtr ConvertObjectToIntPtr<T>(ref T obj) where T : class
        {
            if (PinnedObjects.TryGetValue(obj, out var handleWrapper))
            {
                return handleWrapper;
            }

            var hashCode = obj.GetHashCode();
            PinnedObjects.Add(obj, hashCode);
            PinnedObjectsByHandle.Add(hashCode, obj);
            return hashCode;
        }

        public static void FreePinnedHandle(ref object obj)
        {
            if (PinnedObjects.TryGetValue(obj, out var wrapper))
            {
                //TODO: maybe we need add ref counters here
                PinnedObjectsByHandle.Remove(wrapper);
                PinnedObjects.Remove(obj);
            }
        }

        public static void FreePinnedHandleByIntPtr(IntPtr ptr)
        {
            if (PinnedObjectsByHandle.ContainsKey(ptr))
            {
                //TODO: maybe we need add ref counters here
                PinnedObjects.Remove(PinnedObjectsByHandle[ptr]);
                PinnedObjectsByHandle.Remove(ptr);
            }
        }

        public static object ConvertIntPtrToObject(IntPtr ptr)
        {
            if (PinnedObjectsByHandle.ContainsKey(ptr))
            {
                return PinnedObjectsByHandle[ptr];
            }
            Debug.Assert(ptr != IntPtr.Zero, "Could not find ptr in PinnedObjectsByHandle");
            return null;
        }

        public static void UpdateObjectByIntPtr(IntPtr ptr, object newValue)
        {
            if (PinnedObjectsByHandle.ContainsKey(ptr))
            {
                PinnedObjectsByHandle[ptr] = newValue;
            }
            Debug.Assert(ptr != IntPtr.Zero, "Could not find ptr in PinnedObjectsByHandle");
        }
        public static string ToStringAnsiSafe(IntPtr ptr)
        {
            return ptr == IntPtr.Zero ? null : Marshal.PtrToStringAnsi(ptr);
        }
    }
}
