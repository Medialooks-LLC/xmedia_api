using System;
using System.Runtime.InteropServices;

namespace Medialooks.XMedia
{
    static public class Enumerations
    {
        public enum ErrorCode
        {
            Ok,
            Repeat,
            GeneralFailure = -1,
            InvalidArgument = -1000,
            WrongFormat,
            NotEnoughData,
            NotInitialized,
            InitializationFailed,
            InvalidIndex,
            NullPointer,
            WrongMediatype,
            UnsupportedMediatype,
            OpenFailed,
            ParseFailed,
            NoReadyStreams,
            NotFound,
            Unexpected,
            InvalidCast,
            NotRegistered,
            NotImplemented,
            WrongStream,
            StreamsNotSpecified,
            DecoderNotFound,
            TimeGetFailed,
            BuffersAreFull,
            NotRunning,
            Stopped,
            Closed,
            NotSuitableData,
            StartError,
            Disabled,
            AlreadyStarted,
            Expired,
            DuplicatedName,
            DuplicatedUid,
            AlreadyExists,
            StreamNotInitialized,
            MediaNotSuitable,
            MaxCountExceeded,
            MissedStreamUid,
            InvalidFlowMode,
            LinksUsed,
            AlreadyInit,
            InvalidMode,
            LinkSourceAfterDest,
            LinkSourceNotFound,
            LinkSourceMissed,
            InBlankState,
            BlankStateRequired,
            MediaLinkRequired,
            InvalidWrapperType,
            WrongHandlerType,
            MissedFormat,
            ArrayNodeRequired,
            MapNodeRequired,
            TypeOrSubtypeMissed,
            CommandNotFound,
            CommandNoExecutors,
            CommandTargetNotFound,
            CommandNullResult,
            CommandNameMissed,
            CommandBodyMissed,
            CommandPathRequired,
            CommandPathNotFound,
            NotSupportedInSerialMode,
            NotContainer,
            NoEventsHanders,
            EndOfFile,
            StreamFinished,
            NotSupportChanges,
            UnwrapFailed,
            NoInputStreams,
            NoOutputStreams,
            CommandsNotSupported,
            CodecNotFound,
            Timeout,
            NotReady,
            RecreationInProgress,
            Corrupted,
            NotSupported,
            WrongTarget,
            PositionRequired,
            PositionTaken,
            InvalidItemFlags,
            DurationOrOutRequired,
            OpenUrlMissed,
            Busy,
            Canceled,
            IsEmpty,
            StreamBufferIsFull,
            BuffersAreEmpty,
            StreamBufferIsEmpty
        }

        public enum ObjectType
        {
            None = 0,
            Chunk = 1,
            Props = 2,
            Packet = 4 | Props,
            Frame = 8 | Props,
            ObjectTypes = Chunk | Props | Packet | Frame,

            Audio = 0x100,
            Video = 0x200,
            Subtitle = 0x400,
            Data = 0x800,
            AV = Audio | Video,
            AudioS = Audio | Subtitle,
            VideoS = Video | Subtitle,
            AVS = Audio | Video | Subtitle,
            MediaTypes = Audio | Video | Subtitle | Data,

            PropsAudio = Props | Audio,
            PropsVideo = Props | Video,
            PropsSubtitle = Props | Subtitle,
            PropsData = Props | Data,
            PropsAV = Props | AV,

            ChunkAudio = Chunk | Audio,
            ChunkVideo = Chunk | Video,
            ChunkSubtitle = Chunk | Subtitle,
            ChunkData = Chunk | Data,
            ChunkAV = Chunk | AV,

            PacketAudio = Packet | Audio,
            PacketVideo = Packet | Video,
            PacketSubtitle = Packet | Subtitle,
            PacketData = Packet | Data,
            PacketAV = Packet | AV,

            FrameAudio = Frame | Audio,
            FrameVideo = Frame | Video,
            FrameSubtitle = Frame | Subtitle,
            FrameData = Frame | Data,
            FrameAV = Frame | AV
        }

        public enum HandlerGetFlags
        {
            kGetNoFlags,
            kGetKeyFrame,
            kGetSequenced
        }

        public enum HandlerPutFlags
        {
            kPutNoFlags,
            kTakeOwnership
        }

        public enum HandlerState
        {
            Error,
            Blank,
            Closed,
            Closing,
            WaitForData,
            Ready,
            Running
        }

        public enum RateControl
        {
            None = 0,
            Dest,
            Source,
            SourceSync
        }
        public enum FieldsOrder
        {
            Unknown,
            Progressive,
            TopFirst,               ///< Top coded_first, top displayed first
            BottomFirst,            ///< Bottom coded first, bottom displayed first
            TopCoded_BottomDisplay, ///< Top coded first, bottom displayed first
            BottomCoded_TopDisplay  ///< Bottom coded first, top displayed first
        }

        public enum ColorRange
        {
            UNSPECIFIED = 0,
            MPEG = 1,
            JPEG = 2
        }
        public enum ColorPrimaries
        {
            RESERVED0 = 0,
            BT709 = 1, ///< also ITU-R BT1361 / IEC 61966-2-4 / SMPTE RP 177 Annex B
            UNSPECIFIED = 2,
            RESERVED = 3,
            BT470M = 4,  ///< also FCC Title 47 Code of Federal Regulations 73.682 (a)(20)
            BT470BG = 5,  ///< also ITU-R BT601-6 625 / ITU-R BT1358 625 / ITU-R BT1700 625 PAL & SECAM
            SMPTE170M = 6,  ///< also ITU-R BT601-6 525 / ITU-R BT1358 525 / ITU-R BT1700 NTSC
            SMPTE240M = 7,  ///< identical to above, also called "SMPTE C" even though it uses D65
            FILM = 8,  ///< colour filters using Illuminant C
            BT2020 = 9,  ///< ITU-R BT2020
            SMPTE428 = 10, ///< SMPTE ST 428-1 (CIE 1931 XYZ)
            SMPTEST428_1 = 10, // SMPTE428
            SMPTE431 = 11, ///< SMPTE ST 431-2 (2011) / DCI P3
            SMPTE432 = 12, ///< SMPTE ST 432-1 (2010) / P3 D65 / Display P3
            EBU3213 = 22, ///< EBU Tech. 3213-E (nothing there) / one of JEDEC P22 group phosphors
            JEDEC_P22 = 22  // EBU3213
        }

        public enum ColorTransferCharacteristic
        {
            RESERVED0 = 0,
            BT709 = 1, ///< also ITU-R BT1361
            UNSPECIFIED = 2,
            RESERVED = 3,
            GAMMA22 = 4, ///< also ITU-R BT470M / ITU-R BT1700 625 PAL & SECAM
            GAMMA28 = 5, ///< also ITU-R BT470BG
            SMPTE170M =
                6, ///< also ITU-R BT601-6 525 or 625 / ITU-R BT1358 525 or 625 / ITU-R BT1700 NTSC
            SMPTE240M = 7,
            LINEAR = 8,  ///< "Linear transfer characteristics"
            LOG = 9,  ///< "Logarithmic transfer characteristic (100:1 range)"
            LOG_SQRT = 10, ///< "Logarithmic transfer characteristic (100 * Sqrt(10) : 1 range)"
            IEC61966_2_4 = 11, ///< IEC 61966-2-4
            BT1361_ECG = 12, ///< ITU-R BT1361 Extended Colour Gamut
            IEC61966_2_1 = 13, ///< IEC 61966-2-1 (sRGB or sYCC)
            BT2020_10 = 14, ///< ITU-R BT2020 for 10-bit system
            BT2020_12 = 15, ///< ITU-R BT2020 for 12-bit system
            SMPTE2084 = 16, ///< SMPTE ST 2084 for 10-, 12-, 14- and 16-bit systems
            SMPTEST2084 = 16, // SMPTE2084
            SMPTE428 = 17, ///< SMPTE ST 428-1
            SMPTEST428_1 = 17, // SMPTE428
            ARIB_STD_B67 = 18  ///< ARIB STD-B67, known as "Hybrid log-gamma"
        }
        public enum ColorSpace
        {
            RGB = 0, ///< order of coefficients is actually GBR, also IEC 61966-2-1 (sRGB), YZX and ST 428-1
            BT709 = 1, ///< also ITU-R BT1361 / IEC 61966-2-4 xvYCC709 / derived in SMPTE RP 177 Annex B
            UNSPECIFIED = 2,
            RESERVED = 3, ///< reserved for future use by ITU-T and ISO/IEC just like 15-255 are
            FCC = 4, ///< FCC Title 47 Code of Federal Regulations 73.682 (a)(20)
            BT470BG =
                5, ///< also ITU-R BT601-6 625 / ITU-R BT1358 625 / ITU-R BT1700 625 PAL & SECAM / IEC 61966-2-4 xvYCC601
            SMPTE170M =
                6, ///< also ITU-R BT601-6 525 / ITU-R BT1358 525 / ITU-R BT1700 NTSC / functionally identical to above
            SMPTE240M =
                7, ///< derived from 170M primaries and D65 white point, 170M is derived from BT470 System M's primaries
            YCGCO = 8, ///< used by Dirac / VC-2 and H.264 FRext, see ITU-T SG16
            YCOCG = YCGCO,
            BT2020_NCL = 9,  ///< ITU-R BT2020 non-constant luminance system
            BT2020_CL = 10, ///< ITU-R BT2020 constant luminance system
            SMPTE2085 = 11, ///< SMPTE 2085, Y'D'zD'x
            CHROMA_DERIVED_NCL = 12, ///< Chromaticity-derived non-constant luminance system
            CHROMA_DERIVED_CL = 13, ///< Chromaticity-derived constant luminance system
            ICTCP = 14  ///< ITU-R BT.2100-0, ICtCp
        }
        public enum ChromaLocation
        {
            UNSPECIFIED = 0,
            LEFT = 1, ///< MPEG-2/4 4:2:0, H.264 default for 4:2:0
            CENTER = 2, ///< MPEG-1 4:2:0, JPEG 4:2:0, H.263 4:2:0
            TOPLEFT = 3, ///< ITU-R 601, SMPTE 274M 296M S314M(DV 4:1:1), mpeg2 4:2:2
            TOP = 4,
            BOTTOMLEFT = 5,
            BOTTOM = 6
        }

        public enum FrameSideDataType
        {
            EMPTY = -1,
            PANSCAN,
            A53_CC,
            STEREO3D,
            MATRIXENCODING,
            DOWNMIX_INFO,
            REPLAYGAIN,
            DISPLAYMATRIX,
            AFD,
            MOTION_VECTORS,
            SKIP_SAMPLES,
            AUDIO_SERVICE_TYPE,
            MASTERING_DISPLAY_METADATA,
            GOP_TIMECODE,
            SPHERICAL,
            CONTENT_LIGHT_LEVEL,
            ICC_PROFILE,
            S12M_TIMECODE,
            DYNAMIC_HDR_PLUS,
            REGIONS_OF_INTEREST,
            VIDEO_ENC_PARAMS,
            SEI_UNREGISTERED,
            FILM_GRAIN_PARAMS,
            DETECTION_BBOXES,
            DOVI_RPU_BUFFER,
            DOVI_METADATA,
            DYNAMIC_HDR_VIVID,
            AMBIENT_VIEWING_ENVIRONMENT,
            VIDEO_HINT
        }
        public enum FrameCopyFlags
        {
            kFrameCopyReference,
            kFrameCopyNewBuffer 
        }

        public enum MediaEventType
        {
            None = 0,
            StateChanged = 1,
            EOFReached,
            MediaGetError,
            ConnectionBroken,
            RecreationStart,
            RecreationFailed,
            RecreationSucceeded
        }
    }
}
