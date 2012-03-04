using System;

namespace Lextm.TouchMouseMate
{
    internal struct TouchZone
    {
        private TouchPoint _current;
        private TouchPoint _previous;
        private TouchPoint _previousPrevious;
        private int _time;
        internal double _movement;

        public void Consume(int x, int y, int pixel)
        {
            _current.Consume(x, y, pixel);
        }

        public void AppendTime(int mDwTimeDelta)
        {
            _time += mDwTimeDelta;
        }

        public void ComputeCenter()
        {
            if (_current.Pixel > 0)
            {
                _current.ComputeCenter();
                if (_previous.Pixel > 0)
                {
                    _movement += (_current.CenterX - _previous.CenterX)*(_current.CenterX - _previous.CenterX)
                                 + (_current.CenterY - _previous.CenterY)*(_current.CenterY - _previous.CenterY);
                }
            }
        }

        public bool KeyUpDetected
        {
            get { return _current.Pixel == 0 && _previous.Pixel > 0; }
        }

        public bool KeyDownDetected
        {
            get { return _previous.Pixel > 0 && _previousPrevious.Pixel == 0; }
        }

        public bool MoveDetected
        {
            get { return _movement > 0.04; }
        }

        public void Prepare()
        {
           // Console.WriteLine("Move: {0}", _movement);

            _previousPrevious = _previous;
            _previous = _current;
            _current.Reset();
        }

        public void Reset()
        {
            _time = 0;
            _movement = 0F;
        }
    }
}