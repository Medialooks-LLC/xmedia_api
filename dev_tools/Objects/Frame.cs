using System;
using System.Runtime.InteropServices;
using static Medialooks.XMedia.Enumerations;
using static Medialooks.XMedia.Structures;
using static Medialooks.XMedia.FrameFunctions;

namespace Medialooks.XMedia.DevTools
{
    public class Frame : IDisposable
    {
        private readonly XmFrame _nativeFrame;
        private StreamInfo _streamInfo = null;
        private Program[] _programs = null;

        public ObjectType Type => _nativeFrame.frame_type;
        public Uids Uids => _nativeFrame.uids;
        public Time Time => _nativeFrame.time;

        public XmFrame Native
        {
            get
            {
                return _nativeFrame;
            }
        }
        public Frame(XmFrame nativeFrame)
        {
            _nativeFrame = nativeFrame;
        }

        public StreamInfo StreamInfo
        {
            get
            {
                if (_streamInfo == null)
                    _streamInfo = XmFrameStreamInfoGet(_nativeFrame);
                return _streamInfo;
            }
        }

        public Program[] Programs
        {
            get
            {
                if (_programs != null)
                    return _programs;

                var xmProgramArray = XmFrameProgramsGet(_nativeFrame);
                int count = (int)xmProgramArray.nb_programs;
                if (count == 0 || xmProgramArray.p_programs == IntPtr.Zero)
                    return Array.Empty<Program>();

                _programs = new Program[count];

                int structSize = Marshal.SizeOf<Program>();
                IntPtr currentPtr = xmProgramArray.p_programs;

                for (int i = 0; i < count; i++)
                {
                    _programs[i] = Marshal.PtrToStructure<Program>(currentPtr);
                    currentPtr += structSize;
                }

                return _programs;
            }
        }

        public FrameAudioData AudioData
        {
            get
            {
                FrameAudioData result = new FrameAudioData();
                var xmAudioData = XmFrameAudioDataGet(_nativeFrame);

                result.Format = xmAudioData.format;
                int count = (int)xmAudioData.nb_planes;
                if (count == 0 || xmAudioData.p_planes == IntPtr.Zero)
                {
                    result.Planes = Array.Empty<XmPlaneA>();
                    return result;
                }
                //TODO: Maybe can be simplified
                result.Planes = new XmPlaneA[xmAudioData.nb_planes];
                int structSize = Marshal.SizeOf<Byte>();
                IntPtr currentPtr = xmAudioData.p_planes;

                for (int i = 0; i < count; i++)
                {
                    result.Planes[i] = Marshal.PtrToStructure<XmPlaneA>(currentPtr);
                    currentPtr += structSize;
                }

                return result;
            }
        }

        //TODO: Add all other functions as fields

        public void Dispose()
        {
            XmFrameRelease(_nativeFrame);
        }
    }

}
