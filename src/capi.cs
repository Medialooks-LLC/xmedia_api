using System;
using System.Runtime.InteropServices;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Medialooks.XMedia.Enumerations;
using static Medialooks.XMedia.Structures;

namespace Medialooks.XMedia
{
    internal class CApi
    {
#if WINDOWS
     public const string LIB_NAME = "xmedia_c_api.dll";
#elif MACOS
     public const string LIB_NAME = "xmedia_c_api.dylib";
#else // LINUX
     public const string LIB_NAME = "xmedia_c_api.so";
#endif

        ///////////////////
        /// Packet
        ///////////////////
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmPacketVideoAlloc")]
        internal static extern IntPtr XmPacketVideoAlloc(Uids _uids,
                                                        Time _time,
                                                        XmVideoData _video_data_p,
                                                        StreamInfo _stream_info_p,
                                                        XmProgramArray _programs_arr_p,
                                                        XmSideDataArray _frame_side_data_arr,
                                                        out XmError _error_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmPacketAudioAlloc")]
        internal static extern IntPtr XmPacketAudioAlloc(Uids _uids,
                                                        Time _time,
                                                        XmAudioData _audio_data_p,
                                                        StreamInfo _stream_info_p,
                                                        XmProgramArray _programs_arr_p,
                                                        XmSideDataArray _frame_side_data_arr,
                                                        out XmError _error_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmPacketAddRef")]
        internal static extern IntPtr XmPacketAddRef(XmPacket _frame_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmPacketMakeCopy")]
        internal static extern IntPtr XmPacketMakeCopy(XmPacket _frame_p,
                                                        IntPtr _update_uids,
                                                        IntPtr _update_time_p,
                                                        StreamInfo _update_stream_info_p,
                                                        XmProgramArray _update_programs_arr_p,
                                                        XmSideDataArray _update_side_data_arr);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmPacketStreamInfoGet")]
        internal static extern IntPtr XmPacketStreamInfoGet(XmPacket _frame_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmPacketProgramsGet")]
        internal static extern IntPtr XmPacketProgramsGet(XmPacket _frame_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmPacketAudioDataGet")]
        internal static extern IntPtr XmPacketAudioDataGet(XmPacket _frame_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmPacketVideoDataGet")]
        internal static extern IntPtr XmPacketVideoDataGet(XmPacket _frame_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmPacketSubtitleGet")]
        internal static extern IntPtr XmPacketSubtitleGet(XmPacket _frame_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmPacketSideDataGet")]
        internal static extern IntPtr XmPacketSideDataGet(XmPacket _frame_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmPacketRelease")]
        internal static extern bool XmPacketRelease(XmPacket _frame_for_free_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmPacketsArrayAddRef")]
        internal static extern IntPtr XmPacketsArrayAddRef(XmPacketsArray _packets_arr_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmPacketsArrayRelease")]
        internal static extern bool XmPacketsArrayRelease(XmPacketsArray _packets_arr_for_free_p);

        ///////////////////
        /// Frame
        ///////////////////
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmFrameVideoAlloc")]
        internal static extern IntPtr XmFrameVideoAlloc(Uids _uids,
                                                        Time _time,
                                                        XmVideoData _video_data_p,
                                                        StreamInfo _stream_info_p,
                                                        XmProgramArray _programs_arr_p,
                                                        XmFrameSideDataArray _frame_side_data_arr,
                                                        out XmError _error_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmFrameAudioAlloc")]
        internal static extern IntPtr XmFrameAudioAlloc(Uids _uids,
                                                        Time _time,
                                                        XmAudioData _audio_data_p,
                                                        StreamInfo _stream_info_p,
                                                        XmProgramArray _programs_arr_p,
                                                        XmFrameSideDataArray _frame_side_data_arr,
                                                        out XmError _error_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmFrameAddRef")]
        internal static extern IntPtr XmFrameAddRef(XmFrame _frame_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmFrameMakeCopy")]
        internal static extern IntPtr XmFrameMakeCopy(XmFrame _frame_p,
                                                        IntPtr _update_uids,
                                                        IntPtr _update_time_p,
                                                        StreamInfo _update_stream_info_p,
                                                        XmProgramArray _update_programs_arr_p,
                                                        XmFrameSideDataArray _update_side_data_arr,
                                                        FrameCopyFlags _copy_flags);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmFrameStreamInfoGet")]
        internal static extern IntPtr XmFrameStreamInfoGet(XmFrame _frame_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmFrameProgramsGet")]
        internal static extern IntPtr XmFrameProgramsGet(XmFrame _frame_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmFrameAudioDataGet")]
        internal static extern IntPtr XmFrameAudioDataGet(XmFrame _frame_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmFrameVideoDataGet")]
        internal static extern IntPtr XmFrameVideoDataGet(XmFrame _frame_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmFrameSubtitleGet")]
        internal static extern IntPtr XmFrameSubtitleGet(XmFrame _frame_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmFrameSideDataGet")]
        internal static extern IntPtr XmFrameSideDataGet(XmFrame _frame_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmFrameRelease")]
        internal static extern bool XmFrameRelease(XmFrame _frame_for_free_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmFramesArrayAddRef")]
        internal static extern IntPtr XmFramesArrayAddRef(XmFramesArray _frames_arr_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmFramesArrayRelease")]
        internal static extern bool XmFramesArrayRelease(XmFramesArray _frames_arr_for_free_p);

        ///////////////////
        /// Handlers: Live cicle
        ///////////////////
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmHandlerCreateByDesc")]
        internal static extern IntPtr XmHandlerCreateByDesc(HandlerDesc _handler_desc, out XmError _error_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmPacketsHandlerCreateByDesc")]
        unsafe internal static extern IntPtr XmPacketsHandlerCreateByDesc(HandlerDesc _handler_desc,
                                                                                Int32 _nb_input_packets,
                                                                                XmPacket** _input_packets,
                                                                                out XmError _error_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmFramesHandlerCreateByDesc")]
        unsafe internal static extern IntPtr XmFramesHandlerCreateByDesc(HandlerDesc _handler_desc,
                                                                        Int32 _nb_input_frames,
                                                                        XmFrame** _input_frames,
                                                                        out XmError _error_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmHandlerClose")]
        internal static extern bool XmHandlerClose(XmHandler _handler_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmHandlerAddRef")]
        internal static extern IntPtr XmHandlerAddRef(XmHandler _src_handler_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmHandlerRelease")]
        internal static extern bool XmHandlerRelease(XmHandler _handler_for_free_p);

        ///////////////////
        /// Handlers: Persistance
        ///////////////////
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmHandlerCreate")]
        internal static extern IntPtr XmHandlerCreate(string _json_handler_desc, Int32 _load_flags, out XmError _error_p);


        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmPacketsHandlerCreate")]
        unsafe internal static extern IntPtr XmPacketsHandlerCreate(string _json_handler_desc,
                                                                Int32 _load_flags,
                                                                string _json_init_props,
                                                                Int32 _nb_input_packets,
                                                                XmPacket** _input_packets,
                                                                out XmError _error_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmFramesHandlerCreate")]
        unsafe internal static extern IntPtr XmFramesHandlerCreate(string _json_handler_desc,
                                                                        Int32 _load_flags,
                                                                        string _json_init_props,
                                                                        Int32 _nb_input_frames,
                                                                        XmFrame** _input_frames,
                                                                        out XmError _error_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmHandlerStore")]
        internal static extern IntPtr XmHandlerStore(XmHandler _handler_p, Int32 _store_flags);

        ///////////////////
        /// Handlers: JSON
        ///////////////////
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmJsonResultAddRef")]
        internal static extern IntPtr XmJsonResultAddRef(XmJsonResult _src_json_res_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmJsonResultRelease")]
        internal static extern bool XmJsonResultRelease(XmJsonResult _json_res_p);

        ///////////////////
        /// Handlers: Commands
        ///////////////////
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmHandlerCommandExecute")]
        internal static extern IntPtr XmHandlerCommandExecute(XmHandler _source_handler, Command _command);

        ///////////////////
        /// Handlers: enumerate
        ///////////////////
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmHandlersGet")]
        internal static extern IntPtr XmHandlersGet(XmHandler _root_handler);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmHandlersArrayAddRef")]
        internal static extern IntPtr XmHandlersArrayAddRef(XmHandlersArray _src_handlers_arr_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmHandlersArrayRelease")]
        internal static extern bool XmHandlersArrayRelease(XmHandlersArray _handlers_arr_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmHandlerOpen")]
        internal static extern IntPtr XmHandlerOpen(XmHandler _root_handler, string _handler_path);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmHandlerStatGet")]
        internal static extern IntPtr XmHandlerStatGet(XmHandler _handler, string _stat_path);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmHandlerStatAddRef")]
        internal static extern IntPtr XmHandlerStatAddRef(XmHandlerStat _src_stat_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmHandlerStatRelease")]
        internal static extern bool XmHandlerStatRelease(XmHandlerStat _stat_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmHandlerStatusGet")]
        internal static extern IntPtr XmHandlerStatusGet(XmHandler _handler);

        ///////////////////
        /// Handlers: Input/output
        ///////////////////
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmHandlerFrameInputs")]
        internal static extern IntPtr XmHandlerFrameInputs(XmHandler _handler_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmHandlerFrameOutputs")]
        internal static extern IntPtr XmHandlerFrameOutputs(XmHandler _handler_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmHandlerPacketInputs")]
        internal static extern IntPtr XmHandlerPacketInputs(XmHandler _handler_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmHandlerPacketOutputs")]
        internal static extern IntPtr XmHandlerPacketOutputs(XmHandler _handler_p);

        ///////////////////
        /// Handlers: Data put
        ///////////////////
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmHandlerFramePut")]
        internal static extern bool XmHandlerFramePut(XmHandler _handler, XmFrame _frame_p, HandlerPutFlags _flags, out XmError _error_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmHandlerPacketPut")]
        internal static extern bool XmHandlerPacketPut(XmHandler _handler, XmPacket _packet_p, HandlerPutFlags _flags, out XmError _error_p);

        ///////////////////
        /// Handlers: Data get
        ///////////////////
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmHandlerFrameGet")]
        internal static extern IntPtr XmHandlerFrameGet(XmHandler _handler, HandlerGetFlags _flags, string _json_hints, out XmError _error_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmHandlerPacketGet")]
        internal static extern IntPtr XmHandlerPacketGet(XmHandler _handler, HandlerGetFlags _flags, string _json_hints, out XmError _error_p);

        ///////////////////
        // Callbacks
        ///////////////////
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmFrameCallbackAdd")]
        internal static extern IntPtr XmFrameCallbackAdd(XmHandler _handler,
                                          ObjectType _object_type,
                                          string _json_callback_props,
                                          IntPtr _user_data,
                                          XmOnFrameCallback _on_frame_cb,
                                          out XmError _error_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmFrameCallbackAddRef")]
        internal static extern IntPtr XmFrameCallbackAddRef(XmFrameCallback _src_frame_cb_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmFrameCallbackRelease")]
        internal static extern bool XmFrameCallbackRelease(XmFrameCallback _frame_cb_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmPacketCallbackAdd")]
        internal static extern IntPtr XmPacketCallbackAdd(XmHandler _handler,
                                          ObjectType _object_type,
                                          string _json_callback_props,
                                          IntPtr _user_data,
                                          XmOnPacketCallback _on_packet_cb,
                                          out XmError _error_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmPacketCallbackAddRef")]
        internal static extern IntPtr XmPacketCallbackAddRef(XmPacketCallback _src_packet_cb_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmPacketCallbackRelease")]
        internal static extern bool XmPacketCallbackRelease(XmPacketCallback _packet_cb_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmTimerCallbackAdd")]
        internal static extern IntPtr XmTimerCallbackAdd(Int64 _scheduled_time,
                                          IntPtr _user_data,
                                          XmOnTimerCallback _on_timer_cb,
                                          XmClock _optional_clock_p,
                                          out XmError _error_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmTimerCallbackStop")]
        internal static extern bool XmTimerCallbackStop(Structures.XmTimerCallback _timer_cb_p, out XmError _error_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmTimerCallbackIsStopped")]
        internal static extern bool XmTimerCallbackIsStopped(Structures.XmTimerCallback _timer_cb_p, out XmError _error_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmTimerCallbackAddRef")]
        internal static extern IntPtr XmTimerCallbackAddRef(Structures.XmTimerCallback _src_timer_cb_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmTimerCallbackRelease")]
        internal static extern bool XmTimerCallbackRelease(Structures.XmTimerCallback _timer_cb_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmEventCallbackAdd")]
        unsafe internal static extern IntPtr XmEventCallbackAdd(XmHandler _handler,
                                                        UInt32 _nv_event_type,
                                                        MediaEventType* _p_event_type,
                                                        IntPtr _user_data,
                                                        XmOnEventCallback _on_event_cb,
                                                        out XmError _error_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmEventCallbackAddRef")]
        internal static extern IntPtr XmEventCallbackAddRef(XmEventCallback _src_event_cb_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmEventCallbackRelease")]
        internal static extern bool XmEventCallbackRelease(XmEventCallback _event_cb_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmEventAddRef")]
        internal static extern IntPtr XmEventAddRef(XmEvent _src_event_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmEventRelease")]
        internal static extern bool XmEventRelease(XmEvent _packet_cb_p);

        ///////////////////
        // Other
        ///////////////////
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmClockTime")]
        internal static extern Int64 XmClockTime(XmClock _clock_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmNextUid")]
        internal static extern UInt64 XmNextUid();

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmFrameSaveToFile")]
        internal static extern bool XmFrameSaveToFile(XmFrame _frame_p,
                             string _dest_path,
                             string _hints_json,
                             out XmError _error_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmFramesArraySaveToFile")]
        internal static extern bool XmFramesArraySaveToFile(XmFramesArray _frames_arr_p,
                             string _dest_path,
                             string _hints_json,
                             out XmError _error_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmAudioFramesLoadFromFile")]
        internal static extern IntPtr XmAudioFramesLoadFromFile(string _src_path,
                             FormatA _desired_format,
                             UInt64 _max_count,
                             string _hints_json,
                             out XmError _error_p);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "XmVideoFramesLoadFromFile")]
        internal static extern IntPtr XmVideoFramesLoadFromFile(string _src_path,
                             FormatV _desired_format,
                             UInt64 _max_count,
                             string _hints_json,
                             out XmError _error_p);
    }
}
