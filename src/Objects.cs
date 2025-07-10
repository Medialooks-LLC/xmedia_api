using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static Medialooks.XMedia.Enumerations;
using static Medialooks.XMedia.Structures;

namespace Medialooks.XMedia
{
    static public class Objects
    {
        [DebuggerDisplay("Error {Code}, Category = {Category}")]
        public class Error
        {
            public Int32 Code;
            public string Category;

            public override string ToString()
            {
                //TODO: add saparating by Categories
                return $"Error {Code} ({Category}:{Enum.GetName(typeof(ErrorCode), Code)})";
            }
        }
        internal static void FillErrorInfo(ref XmError errorPtr, ref Error error)
        {
            error.Code = errorPtr.error_code;
            if (errorPtr.error_category != IntPtr.Zero)
            {
                error.Category = Marshal.PtrToStringAnsi(errorPtr.error_category);
            }
            else
            {
                error.Category = "unknown";
            }
        }
    }
}
