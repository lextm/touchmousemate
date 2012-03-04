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
                machine.Idle();
                if (NativeMethods.Section.TouchOverClick)
                {
                    NativeMethods.MouseEvent(NativeMethods.Section.LeftHandMode ? MouseEventFlags.RightUp : MouseEventFlags.LeftUp);
                }
            }
        }
    }
}