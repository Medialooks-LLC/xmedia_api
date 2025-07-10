using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading.Channels;
using static Medialooks.XMedia.Enumerations;
using static Medialooks.XMedia.Objects;
using static Medialooks.XMedia.Structures;
using static Medialooks.XMedia.FrameFunctions;
using static Medialooks.XMedia.Functions;


namespace Medialooks.XMedia.DevTools
{
    static public partial class Utils
    {
        static Utils()
        {
            video_stream_uid = XmNextUid();
            video_group_uid = XmNextUid();
            video_source_uid = XmNextUid();

            audio_stream_uid = XmNextUid();
            audio_group_uid = XmNextUid();
            audio_source_uid = XmNextUid();
        }
    }
}
