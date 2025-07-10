using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static Medialooks.XMedia.Structures;
using static Medialooks.XMedia.FrameFunctions;

namespace Medialooks.XMedia.DevTools
{
    public class FramesArray : IReadOnlyList<XmFrame>, IDisposable
    {
        private static readonly int _pointerSize = Marshal.SizeOf(typeof(IntPtr));
        private readonly XmFramesArray _nativeFramesArray;

        public XmFramesArray Native
        {
            get
            {
                return _nativeFramesArray;
            }
        }
        public FramesArray(XmFramesArray nativeFramesArray)
        {
            _nativeFramesArray = nativeFramesArray;
        }

        public int Count => (int)_nativeFramesArray.nb_frames;


        public XmFrame this[int index]
        {
            get
            {
                IntPtr framePtr = Marshal.ReadIntPtr(_nativeFramesArray.p_frames, index * _pointerSize);
                var frame = Marshal.PtrToStructure<XmFrame>(framePtr);
                return frame;
            }
        }

        public IEnumerator<XmFrame> GetEnumerator()
        {
            return new FramesArrayEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public void Dispose()
        {
            XmFramesArrayRelease(_nativeFramesArray);
        }
    }

    public class FramesArrayEnumerator : IEnumerator<XmFrame>
    {
        private readonly FramesArray _array;
        private int _index;

        public FramesArrayEnumerator(FramesArray array)
        {
            _array = array;
            _index = -1;
        }

        public XmFrame Current
        {
            get
            {
                if (_index < 0 || _index >= _array.Count)
                    throw new InvalidOperationException();

                return _array[_index];
            }
        }

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            _index++;
            return _index < _array.Count;
        }

        public void Reset()
        {
            _index = -1;
        }

        public void Dispose()
        {
            //2Think: Add XmRelease here and add ref in constructor?
        }
    }


}
