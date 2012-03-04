using System;

namespace Lextm.TouchMouseMate
{
    public class RightDownPending : IMouseState
    {
        public void Process(MouseEventFlags flag, StateMachine machine)
        {
            if (flag == MouseEventFlags.LeftDown)
            {
                Console.WriteLine("right down p->middle down p");
                machine.MiddleDownPending();
            }
            else if (flag == MouseEventFlags.Absolute)
            {
                Console.WriteLine("right down p->right down");
                machine.RightDown();
            }
            else if (flag == MouseEventFlags.RightUp)
            {
                Console.WriteLine("right down p->idle");
                machine.Idle();
            }
            else if (flag == MouseEventFlags.Move)
            {
                Console.WriteLine("right down p-> idle (move)");
                Log.Debug("right down cancelled");
                machine.Idle();
            }
        }
    }
}