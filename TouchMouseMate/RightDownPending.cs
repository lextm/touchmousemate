using System;

namespace Lextm.TouchMouseMate
{
    public class RightDownPending : IMouseState
    {
        public void Process(MouseEventFlags flag, StateMachine machine)
        {
            if (flag == MouseEventFlags.LeftDown)
            {
                Console.WriteLine("right down p->middle down");
                machine.Current = MiddleDown.Instance;
                if (NativeMethods.Section.MiddleClick)
                {
                    NativeMethods.MouseEvent(MouseEventFlags.MiddleDown);
                }

                machine.Timer.Enabled = false;
            }
            else if (flag == MouseEventFlags.Absolute)
            {
                Console.WriteLine("right down p->right down");
                machine.Current = RightDown.Instance;
                if (NativeMethods.Section.TouchOverClick)
                {
                    NativeMethods.MouseEvent(NativeMethods.Section.LeftHandMode ? MouseEventFlags.LeftDown : MouseEventFlags.RightDown);
                }

                machine.Timer.Enabled = false;
            }
        }

        public static RightDownPending Instance = new RightDownPending();
    }
}