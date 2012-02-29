using System;

namespace Lextm.TouchMouseMate
{
    public class LeftDown : IMouseState
    {
        public void Process(MouseEventFlags flag, StateMachine machine)
        {
            if (flag == MouseEventFlags.LeftUp)
            {
                Console.WriteLine("left down->idle");
                machine.Current = Idle.Instance;
                if (NativeMethods.Section.TouchOverClick)
                {
                    NativeMethods.MouseEvent(NativeMethods.Section.LeftHandMode ? MouseEventFlags.RightUp : MouseEventFlags.LeftUp);
                }
            }
        }

        public static LeftDown Instance = new LeftDown();
    }
}