using System;
using System.Collections.Generic;
using System.Text;

namespace Generics
{
    public class CircularBuffer_Double
    {
        private double[] _buffer;
        private int _start;
        private int _end;

        public CircularBuffer_Double()
            : this(10)
        { }

        public CircularBuffer_Double(int capacity)
        {
            _buffer = new double[capacity + 1];
            _start = 0;
            _end = 0;
        }

        public void Write(double value)
        {
            _buffer[_end] = value;
            _end = (_end + 1) % _buffer.Length;

            if (_end == _start)
                _start = (_start + 1) % _buffer.Length;
        }

        public double Read()
        {
            var result = _buffer[_start];
            _start = (_start + 1) % _buffer.Length;
            return result;
        }

        public int Capacity
        {
            get { return _buffer.Length; }
        }

        public bool IsEmpty
        {
            get { return _end == _start; }
        }

        public bool IsFull
        {
            get { return (_end + 1) % _buffer.Length == _start; }
        }
    }

    public class CircularBuffer_Object
    {
        private object[] _buffer;
        private int _start;
        private int _end;

        public CircularBuffer_Object()
            : this(10)
        { }

        public CircularBuffer_Object(int capacity)
        {
            _buffer = new object[capacity + 1];
            _start = 0;
            _end = 0;
        }

        public void Write(object value)
        {
            _buffer[_end] = value;
            _end = (_end + 1) % _buffer.Length;

            if (_end == _start)
                _start = (_start + 1) % _buffer.Length;
        }

        public object Read()
        {
            var result = _buffer[_start];
            _start = (_start + 1) % _buffer.Length;
            return result;
        }

        public int Capacity
        {
            get { return _buffer.Length; }
        }

        public bool IsEmpty
        {
            get { return _end == _start; }
        }

        public bool IsFull
        {
            get { return (_end + 1) % _buffer.Length == _start; }
        }
    }

    /// <summary>
    /// This is my circular buffer
    /// </summary>
    /// <typeparam name="T">The type of buffer you want to use</typeparam>
    public class CircularBuffer<T> where T: struct
    {
        private T[] _buffer;
        private int _start;
        private int _end;

        public CircularBuffer()
            : this(10)
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="capacity">The capacity of the buffer</param>
        public CircularBuffer(int capacity)
        {
            _buffer = new T[capacity + 1];
            _start = 0;
            _end = 0;
        }

        public void Write(T value)
        {
            _buffer[_end] = value;
            _end = (_end + 1) % _buffer.Length;

            if (_end == _start)
                _start = (_start + 1) % _buffer.Length;
        }

        public T Read()
        {
            var result = _buffer[_start];
            _start = (_start + 1) % _buffer.Length;
            return result;
        }

        public int Capacity
        {
            get { return _buffer.Length; }
        }

        public bool IsEmpty
        {
            get { return _end == _start; }
        }

        public bool IsFull
        {
            get { return (_end + 1) % _buffer.Length == _start; }
        }
    }
}
