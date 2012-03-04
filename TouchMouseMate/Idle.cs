using System;

namespace Lextm.TouchMouseMate
{
    public class Idle : IMouseState
    {
        public void Process(MouseEventFlags flag, StateMachine machine)
        {
            if (flag == MouseEventFlags.LeftDown)
            {
                Console.WriteLine("idle -> left down p");
                machine.LeftDownPending();
            }
            else if (flag == MouseEventFlags.RightDown)
            {
                Console.WriteLine("idle -> right down p");
                machine.RightDownPending();
            }
        }

        public static Idle Instance = new Idle();
    }
}