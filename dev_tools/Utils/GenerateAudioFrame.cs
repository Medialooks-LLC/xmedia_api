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


namespace Medialooks.XMedia.DevTools
{
    static public partial class Utils
    {
        public static float[] GenerateAudioFrameData(int audioPosInSamples,
                                        int numSamples = 1764)
        {
            const int channels = 2;
            const int sampleRate = 44100;
            const float freq = 1000f;
            float durSeconds = (float)numSamples / (float)sampleRate;

            float[] audioSamples = new float[numSamples * channels];

            for (int i = 0; i < numSamples; i++)
            {
                float angle = (float)(2.0 * Math.PI * (audioPosInSamples + i) / (freq * durSeconds));
                audioSamples[i * channels] = (float)Math.Sin(angle);
                angle = (float)(2.0 * Math.PI * (audioPosInSamples + i) / (freq * durSeconds)) * 5;
                audioSamples[i * channels + 1] = (float)Math.Sin(angle);
            }

            return audioSamples;
        }

        private static readonly UInt64 audio_stream_uid;
        private static readonly UInt64 audio_group_uid;
        private static readonly UInt64 audio_source_uid;

        //TODO: add EOS processing
        public static XmFrame GenerateXmAudioFrame(int audioPosInSamples, bool eos = false,
                                        int numSamples = 1764)
        {
            var uids = new Uids() { stream_uid = audio_stream_uid, group_uid = audio_group_uid, source_uid = audio_source_uid };
            var formatA = new FormatA() { channels = 2, sample_format = "flt", sample_rate = 44100 };
            var time = new Time();
            //time64::FromSec(_audio_pos_in_samples / (double)format.sample_rate); //2Think: may be add time utils to API?
            time.timestamp = (UInt64)(audioPosInSamples / (double)formatA.sample_rate * 10_000_000);

            var streamInfo = new StreamInfo();
            var programsArray = new XmProgramArray();
            var frameSideDataArray = new XmFrameSideDataArray();

            var error = new Error();
            XmFrame frame;
            unsafe
            {
                fixed (float* pAudio = GenerateAudioFrameData(audioPosInSamples, numSamples))
                {
                    byte* pAudioBytes = (byte*)pAudio;

                    var plane_a = new XmPlaneA()
                    {
                        audio_p = (IntPtr)pAudio, //Bytes,
                        bytes = numSamples * formatA.channels * sizeof(float)
                    };
                    var planesPtr = Marshal.AllocHGlobal(Marshal.SizeOf<XmPlaneA>());
                    Marshal.StructureToPtr(plane_a, planesPtr, false);

                    var audioData = new XmAudioData
                    {
                        format = formatA,
                        nb_planes = 1,
                        p_planes = planesPtr
                    };

                    frame = XmFrameAudioAlloc(uids,
                                     time,
                                     audioData,
                                     streamInfo,
                                     programsArray,
                                     frameSideDataArray,
                                     ref error);
                }
            }
            if (error.Code != 0)
            {
                Console.WriteLine($"Some error occured during frame creation: {error}");
            }


            return frame;
        }
    }
}
