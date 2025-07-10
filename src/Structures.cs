using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using static Medialooks.XMedia.Enumerations;

namespace Medialooks.XMedia
{
    static public class Structures
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        internal struct XmError
        {
            public Int32 error_code;
            public IntPtr error_category;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class HandlerDesc
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string handler_type;
            [MarshalAs(UnmanagedType.LPStr)]
            public string handler_name;
            [MarshalAs(UnmanagedType.LPStr)]
            public string json_wrapping_props;
            [MarshalAs(UnmanagedType.LPStr)]
            public string open_url_or_json_scheme;
            [MarshalAs(UnmanagedType.LPStr)]
            public string json_init_props;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class XmHandler
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string handler_type;
            [MarshalAs(UnmanagedType.LPStr)]
            public string handler_name;
            [MarshalAs(UnmanagedType.LPStr)]
            public string handler_info;

            public ObjectType input_type;
            public ObjectType output_type;

            public IntPtr opaque_pv;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class Command
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string command_name;
            [MarshalAs(UnmanagedType.LPStr)]
            public string json_command_body;
            [MarshalAs(UnmanagedType.LPStr)]
            public string target_path;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class XmJsonResult
        {
            public Int32 error_code;
            public Int32 error_category;
            [MarshalAs(UnmanagedType.LPStr)]
            public string json_result_or_error_desc;

            public IntPtr opaque_pv;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct Uids
        {
            public UInt64 stream_uid;
            public UInt64 group_uid;
            public UInt64 source_uid;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct Rational
        {
            public Int32 num;
            public Int32 den;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct Time
        {
            public UInt64 timestamp;
            RateControl rc_type;
            public UInt64 duration; // 2Think: set as pointers too?
            public UInt64 segment_start;
            public UInt64 segment_end;
            Rational timebase;
            public Int32 nb_timecodes_p;
            public IntPtr timecodes_p;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class XmFrame
        {
            public ObjectType frame_type;
            public Uids uids;
            public Time time;
            public IntPtr opaque_pv;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class XmHandlersArray
        {
            public UInt32 nb_handlers;
            public IntPtr pp_handlers; //Handler**
            public IntPtr opaque_pv;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class XmFramesArray
        {
            public UInt32 nb_frames;
            public IntPtr p_frames; //Frame**
            public IntPtr opaque_pv;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class XmPacketsArray
        {
            public UInt32 nb_packets;
            public IntPtr p_packets; //Packet**
            public IntPtr opaque_pv;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class HandlerStatus
        {
            public HandlerState handler_state;
            public Int32 inputs_count;
            public Int64 inputs_activity_utc;
            public UInt64 outputs_count;
            public Int64 outputs_activity_utc;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class XmHandlerStat
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string json_stat;
            public IntPtr opaque_pv;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class XmPacket
        {
            public ObjectType packet_type;
            public Uids uids;
            public Time time;
            public Int32 packet_data_bytes;
            public IntPtr packet_data_p;
            public IntPtr opaque_pv;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class StreamInfo
        {
            public UInt64 stream_uid;
            public UInt64 group_uid;
            public UInt64 source_uid;
            public UInt64 original_stream_uid;
            [MarshalAs(UnmanagedType.LPStr)]
            public string json_metadata;
            public UInt32 stream_idx;
            public UInt32 stream_pid;
            public Int64 start_time;
            public Int64 duration;
            public Int64 frames_num;
            public Rational stream_timebase;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class Program
        {
            public UInt32 program_num;
            public UInt32 program_id;
            public UInt32 flags;
            public UInt32 pmt_pid;
            public UInt32 pcr_pid;
            public UInt32 pmt_version;
            [MarshalAs(UnmanagedType.LPStr)]
            public string json_metadata;

        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class XmProgramArray
        {
            public UInt32 nb_programs;
            public IntPtr p_programs;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class FormatV
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pixel_format;
            public Int32 width;
            public Int32 height;
            public FieldsOrder fields_order;
            public Rational frame_rate;
            public Rational picture_ar;
            public ColorRange color_range;
            public ColorPrimaries color_primaries;
            public ColorTransferCharacteristic color_trc;
            public ColorSpace color_space;
            public ChromaLocation chroma_location;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class XmPlaneV
        {
            public IntPtr video_p;
            public Int32 width;
            public Int32 height;
            public Int32 row_bytes; // raw_bytes?
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class XmVideoData
        {
            public FormatV format;
            public UInt32 nb_planes;
            public IntPtr p_planes;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class FormatA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string sample_format;
            public Int32 channels;
            public Int32 sample_rate;
            [MarshalAs(UnmanagedType.LPStr)]
            public string channels_layout;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class XmPlaneA
        {
            public IntPtr audio_p;
            public Int32 bytes;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class XmAudioData
        {
            public FormatA format;
            public UInt32 nb_planes;
            public IntPtr p_planes;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class XmFrameSideData
        {
            public FrameSideDataType type;
            public Int32 side_data_bytes;
            public IntPtr side_data_p;
            [MarshalAs(UnmanagedType.LPStr)]
            public string json_metadata;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class XmFrameSideDataArray
        {
            public Int32 nb_side_data;
            public IntPtr p_side_data;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class RectD
        {
            public double left;
            public double top;
            public double right;
            public double bottom;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class XmSubtitleImg
        {
            public RectD pos;
            public UInt32 colors;
            public UInt32 nb_planes;
            public IntPtr p_planes;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class XmSubtitle
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string text_plain;
            [MarshalAs(UnmanagedType.LPStr)]
            public string text_ass;
            public UInt32 nb_images;
            public IntPtr p_images;
            public UInt32 duration_msec;
            public bool is_forced;
            public Int32 start_offset_msec;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool XmOnFrameCallback(IntPtr userData, XmFrame frame);


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class XmFrameCallback
        {
            public ObjectType frame_types;
            [MarshalAs(UnmanagedType.LPStr)]
            public string json_details;

            public IntPtr cb_user_data;
            public XmOnFrameCallback frame_cb_pf;

            public IntPtr opaque_pv;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool XmOnPacketCallback(IntPtr userData, XmPacket packet);


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class XmPacketCallback
        {
            public ObjectType frame_types;
            [MarshalAs(UnmanagedType.LPStr)]
            public string json_details;

            public IntPtr cb_user_data;
            public XmOnPacketCallback packet_cb_pf;

            public IntPtr opaque_pv;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Int64 XmOnTimerCallback(IntPtr userData, XmTimerTickNative timerTick);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct XmTimerTickNative
        {
            /// @brief The scheduled time for the task
            public Int64 scheduled_time;
            /// @brief Counter for tracking the number of times a task is scheduled.
            public UInt64 scheduling_counter;
            /// @brief Pointer to a clock used for time keeping.
            public XmClockNative clock_p;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class XmTimerTick
        {
            /// @brief The scheduled time for the task
            public Int64 scheduled_time;
            /// @brief Counter for tracking the number of times a task is scheduled.
            public UInt64 scheduling_counter;
            /// @brief Pointer to a clock used for time keeping.
            public XmClock clock_p;

            public static XmTimerTick FromNative(XmTimerTickNative native)
            {
                return new XmTimerTick
                {
                    scheduled_time = native.scheduled_time,
                    scheduling_counter = native.scheduling_counter,
                    clock_p = XmClock.FromNative(native.clock_p)
                };
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class XmTimerCallback
        {
            public IntPtr clock_p;
            public bool is_stopped; // 2Think: Or use something like XmTimerCallbackIsStopped(..)

            public IntPtr cb_user_data;
            public XmOnTimerCallback timer_cb_pf;

            public IntPtr opaque_pv;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct XmClockNative
        {
            public Int64 sys_clock_offset;
            public bool is_true_monotonic;
            public IntPtr clock_type;

            public IntPtr opaque_pv;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class XmClock
        {
            public Int64 sys_clock_offset;
            public bool is_true_monotonic;
            [MarshalAs(UnmanagedType.LPStr)]
            public string clock_type;

            public IntPtr opaque_pv;

            public static XmClock FromNative(XmClockNative native)
            {
                return new XmClock
                {
                    sys_clock_offset = native.sys_clock_offset,
                    is_true_monotonic = native.is_true_monotonic,
                    clock_type = Utils.ToStringAnsiSafe(native.clock_type),
                    opaque_pv = native.opaque_pv
                };
            }
        }


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class XmSideDataArray
        {
            Int32 nb_side_data;
            public IntPtr p_side_data;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool XmOnEventCallback(IntPtr userData, XmEventNative xmEvent);


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class XmEventCallbackNative
        {
            public UInt32 nb_events_types;
            public IntPtr p_events_types; // here is pointer to an array of MediaEventType
            public IntPtr json_filter;

            public IntPtr cb_user_data;
            public XmOnEventCallback event_cb_pf;

            public IntPtr opaque_pv;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class XmEventCallback
        {
            public UInt32 nb_events_types;
            public IntPtr p_events_types; // here is pointer to an array of MediaEventType

            //anyway we add info for marshaling to skip multiple conversion from XmEventCallback to XmEventCallbackNative
            [MarshalAs(UnmanagedType.LPStr)]
            public string json_filter;

            public IntPtr cb_user_data;
            public XmOnEventCallback event_cb_pf;

            public IntPtr opaque_pv;

            public static XmEventCallback FromNative(XmEventCallbackNative native)
            {
                return new XmEventCallback
                {
                    nb_events_types = native.nb_events_types,
                    p_events_types = native.p_events_types, //TODO: fix it
                    json_filter = Utils.ToStringAnsiSafe(native.json_filter),
                    cb_user_data = native.cb_user_data,
                    event_cb_pf = native.event_cb_pf,
                    opaque_pv = native.opaque_pv
                };
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class XmEventNative
        {
            public MediaEventType event_type;
            public IntPtr json_event_desc;

            public Int64 timestamp_utc;
            public IntPtr from_handler_path;

            public IntPtr opaque_pv;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class XmEvent
        {
            public MediaEventType event_type;
            [MarshalAs(UnmanagedType.LPStr)]
            public string json_event_desc;

            public Int64 timestamp_utc;
            [MarshalAs(UnmanagedType.LPStr)]
            public string from_handler_path;

            public IntPtr opaque_pv;
            public static XmEvent FromNative(XmEventNative native)
            {
                return new XmEvent
                {
                    event_type = native.event_type,
                    json_event_desc = Utils.ToStringAnsiSafe(native.json_event_desc),
                    timestamp_utc = native.timestamp_utc,
                    from_handler_path = Utils.ToStringAnsiSafe(native.from_handler_path),
                    opaque_pv = native.opaque_pv
                };
            }
        }
    }
}
