using System.Timers;

namespace Lextm.TouchMouseMate
{
    public class StateMachine
    {
        private readonly object _locker = new object();
        private IMouseState _current;
        private readonly Timer _timer;
        private readonly IMouseState _idle = new Idle();
        private readonly IMouseState _leftDown = new LeftDown();
        private readonly IMouseState _rightDown = new RightDown();
        private readonly IMouseState _middleDown = new MiddleDown();
        private readonly IMouseState _leftDownPending = new LeftDownPending();
        private readonly IMouseState _rightDownPending = new RightDownPending();
        private readonly IMouseState _middleDownPending = new MiddleDownPending();

        public StateMachine()
        {
            _timer = new Timer {Interval = NativeMethods.Section.MinClickTimeout, Enabled = false, AutoReset = false};
            _timer.Elapsed += (sender, args) => Process(MouseEventFlags.Absolute);
            Idle();
        }

        public void Process(MouseEventFlags flag)
        {
            lock (_locker)
            {
                _current.Process(flag, this);
            }
        }

        public void Idle()
        {
            _timer.Enabled = false;
            if (_current == _leftDown)
            {
                if (NativeMethods.Section.TouchOverClick)
                {
                    NativeMethods.MouseEvent(NativeMethods.Section.LeftHandMode ? MouseEventFlags.RightUp : MouseEventFlags.LeftUp);
                }
            }
            else if (_current == _rightDown)
            {
                if (NativeMethods.Section.TouchOverClick)
                {
                    NativeMethods.MouseEvent(NativeMethods.Section.LeftHandMode ? MouseEventFlags.LeftUp : MouseEventFlags.RightUp);
                }
            }
            else if (_current == _middleDown)
            {
                if (NativeMethods.Section.MiddleClick)
                {
                    NativeMethods.MouseEvent(MouseEventFlags.MiddleUp);
                }
            }
            _current = _idle;
        }

        public void LeftDown()
        {
            _timer.Enabled = false;
            _current = _leftDown;
            if (NativeMethods.Section.TouchOverClick)
            {
                NativeMethods.MouseEvent(NativeMethods.Section.LeftHandMode ? MouseEventFlags.RightDown : MouseEventFlags.LeftDown);
            }
        }

        public void RightDown()
        {
            _timer.Enabled = false; 
            _current = _rightDown;
            if (NativeMethods.Section.TouchOverClick)
            {
                NativeMethods.MouseEvent(NativeMethods.Section.LeftHandMode ? MouseEventFlags.LeftDown : MouseEventFlags.RightDown);
            }
        }

        public void MiddleDown()
        {
            _timer.Enabled = false;
            _current = _middleDown;
            if (NativeMethods.Section.MiddleClick)
            {
                NativeMethods.MouseEvent(MouseEventFlags.MiddleDown);
            }
        }

        public void LeftDownPending()
        {
            _current = _leftDownPending;
            _timer.Enabled = true;
        }

        public void RightDownPending()
        {
            _current = _rightDownPending;
            _timer.Enabled = true;
        }

        public void MiddleDownPending()
        {
            _current = _middleDownPending;
            _timer.Enabled = true;
        }
    }
}
