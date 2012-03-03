using System.Timers;

namespace Lextm.TouchMouseMate
{
    public class StateMachine
    {
        public IMouseState Current;
        public StateMachine()
        {
            Current = Idle.Instance;
            Timer = new Timer {Interval = NativeMethods.Section.MinClickTimeout, Enabled = false, AutoReset = false};
            Timer.Elapsed += (sender, args) => Process(MouseEventFlags.Absolute);
        }

        public void Process(MouseEventFlags flag)
        {
            Current.Process(flag, this);
        }

        public Timer Timer;

        public void Reset()
        {
            Current = Idle.Instance;
            Timer.Enabled = false;
        }
    }
}
