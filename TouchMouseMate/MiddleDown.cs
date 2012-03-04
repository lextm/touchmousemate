using System;

namespace Lextm.TouchMouseMate
{
    public class MiddleDown : IMouseState
    {
        public void Process(MouseEventFlags flag, StateMachine machine)
        {
            if (flag == MouseEventFlags.LeftUp || flag == MouseEventFlags.RightUp)
            {
                Console.WriteLine("middle down->idle");
                machine.Idle();
                if (NativeMethods.Section.MiddleClick)
                {
                    NativeMethods.MouseEvent(MouseEventFlags.MiddleUp);
                }
            }
        }
    }
}