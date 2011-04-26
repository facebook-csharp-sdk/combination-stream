
namespace CombinationStream
{
    using System;
    using System.Collections.Generic;

    // todo: implement BeginRead and EndRead.
    internal class CombinationStream : System.IO.Stream
    {
        private readonly IList<System.IO.Stream> _streams;
        private int _currentStreamIndex;
        private System.IO.Stream _currentStream;
        private long _length = -1;
        private long _postion;

        public CombinationStream(IList<System.IO.Stream> streams)
        {
            if (streams == null)
                throw new ArgumentNullException("streams");

            _streams = streams;
            if (streams.Count > 0)
                _currentStream = streams[_currentStreamIndex++];
        }

        public override void Flush()
        {
            if (_currentStream != null)
                _currentStream.Flush();
        }

        public override long Seek(long offset, System.IO.SeekOrigin origin)
        {
            throw new InvalidOperationException("Stream is not seekable.");
        }

        public override void SetLength(long value)
        {
            _length = value;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int result = 0;
            int buffPostion = offset;

            while (count > 0)
            {
                int bytesRead = _currentStream.Read(buffer, buffPostion, count);
                result += bytesRead;
                buffPostion += bytesRead;
                _postion += bytesRead;

                if (bytesRead <= count)
                    count -= bytesRead;

                if (count > 0)
                {
                    if (_currentStreamIndex >= _streams.Count)
                        break;

                    _currentStream = _streams[_currentStreamIndex++];
                }
            }

            return result;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new InvalidOperationException("Stream is not writable");
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override long Length
        {
            get
            {
                if (_length == -1)
                {
                    _length = 0;
                    foreach (var stream in _streams)
                        _length += stream.Length;
                }

                return _length;
            }
        }

        public override long Position
        {
            get { return _postion; }
            set { throw new NotImplementedException(); }
        }
    }
}