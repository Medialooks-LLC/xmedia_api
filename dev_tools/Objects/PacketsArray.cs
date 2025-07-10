using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static Medialooks.XMedia.Structures;
using static Medialooks.XMedia.PacketFunctions;

namespace Medialooks.XMedia.DevTools
{
    public class PacketsArray : IReadOnlyList<XmPacket>, IDisposable
    {
        private static readonly int _pointerSize = Marshal.SizeOf(typeof(IntPtr));
        private readonly XmPacketsArray _nativePacketsArray;

        public XmPacketsArray Native
        {
            get
            {
                return _nativePacketsArray;
            }
        }
        public PacketsArray(XmPacketsArray nativePacketsArray)
        {
            _nativePacketsArray = nativePacketsArray;
        }

        public int Count => (int)_nativePacketsArray.nb_packets;


        public XmPacket this[int index]
        {
            get
            {
                IntPtr packetPtr = Marshal.ReadIntPtr(_nativePacketsArray.p_packets, index * _pointerSize);
                var packet = Marshal.PtrToStructure<XmPacket>(packetPtr);
                return packet;
            }
        }

        public IEnumerator<XmPacket> GetEnumerator()
        {
            return new PacketsArrayEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public void Dispose()
        {
            XmPacketsArrayRelease(_nativePacketsArray);
        }
    }

    public class PacketsArrayEnumerator : IEnumerator<XmPacket>
    {
        private readonly PacketsArray _array;
        private int _index;

        public PacketsArrayEnumerator(PacketsArray array)
        {
            _array = array;
            _index = -1;
        }

        public XmPacket Current
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
        }
    }


}
