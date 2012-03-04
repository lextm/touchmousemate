using System;

namespace Lextm.TouchMouseMate
{
    public class MiddleDownPending : IMouseState
    {
        public void Process(MouseEventFlags flag, StateMachine machine)
        {
            if (flag == MouseEventFlags.Absolute)
            {
                Console.WriteLine("middle down p->middle down");
                machine.MiddleDown();
                if (NativeMethods.Section.MiddleClick)
                {
                    NativeMethods.MouseEvent(MouseEventFlags.MiddleDown);
                }
            }
            else if (flag == MouseEventFlags.Move)
            {
                Console.WriteLine("middle down p-> idle (move)");
                machine.Idle();
            }
        }
    }
}