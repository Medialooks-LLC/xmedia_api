using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static Medialooks.XMedia.Handlers;
using static Medialooks.XMedia.Structures;

namespace Medialooks.XMedia.DevTools
{
    public class HandlersArray : IReadOnlyList<XmHandler>, IDisposable
    {
        private static readonly int _pointerSize = Marshal.SizeOf(typeof(IntPtr));
        private readonly XmHandlersArray _nativeHandlersArray;

        public XmHandlersArray Native
        {
            get
            {
                return _nativeHandlersArray;
            }
        }
        public HandlersArray(XmHandlersArray nativeHandlersArray)
        {
            _nativeHandlersArray = nativeHandlersArray;
        }

        public int Count => (int)_nativeHandlersArray.nb_handlers;


        public XmHandler this[int index]
        {
            get
            {
                IntPtr handlerPtr = Marshal.ReadIntPtr(_nativeHandlersArray.pp_handlers, index * _pointerSize);
                var handler = Marshal.PtrToStructure<XmHandler>(handlerPtr);
                return handler;
            }
        }

        public IEnumerator<XmHandler> GetEnumerator()
        {
            return new HandlersArrayEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            XmHandlersArrayRelease(_nativeHandlersArray);
        }
    }

    public class HandlersArrayEnumerator : IEnumerator<XmHandler>
    {
        private readonly HandlersArray _array;
        private int _index;

        public HandlersArrayEnumerator(HandlersArray array)
        {
            _array = array;
            _index = -1;
        }

        public XmHandler Current
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
