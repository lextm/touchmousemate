using System;

namespace Lextm.TouchMouseMate
{
    public class LeftDownPending : IMouseState
    {
        public void Process(MouseEventFlags flag, StateMachine machine)
        {
            if (flag == MouseEventFlags.RightDown)
            {
                Console.WriteLine("left down p->middle down p");
                machine.MiddleDownPending();
            }
            else if (flag == MouseEventFlags.Absolute)
            {
                Console.WriteLine("left down p-> left down");
                machine.LeftDown();
                if (NativeMethods.Section.TouchOverClick)
                {
                    NativeMethods.MouseEvent(NativeMethods.Section.LeftHandMode ? MouseEventFlags.RightDown : MouseEventFlags.LeftDown);
                }
            }
            else if (flag == MouseEventFlags.LeftUp)
            {
                Console.WriteLine("left down p->idle");
                machine.Idle();
            }
            else if (flag == MouseEventFlags.Move)
            {
                Console.WriteLine("left down p-> idle (move)");
                machine.Idle();
            }
        }
    }
}