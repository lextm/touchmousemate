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
                machine.Current = Idle.Instance;
                if (NativeMethods.Section.MiddleClick)
                {
                    NativeMethods.MouseEvent(MouseEventFlags.MiddleUp);
                }
            }
        }

        public static MiddleDown Instance = new MiddleDown();
    }
}