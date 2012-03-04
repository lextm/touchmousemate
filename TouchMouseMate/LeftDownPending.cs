namespace Lextm.TouchMouseMate
{
    public class LeftDownPending : IMouseState
    {
        public void Process(MouseEventFlags flag, StateMachine machine)
        {
            if (flag == MouseEventFlags.RightDown)
            {
                Log.Info("left down p->middle down p");
                machine.MiddleDownPending();
            }
            else if (flag == MouseEventFlags.Absolute)
            {
                Log.Info("left down p-> left down");
                machine.LeftDown();
            }
            else if (flag == MouseEventFlags.LeftUp)
            {
                Log.Info("left down p->idle");
                machine.Idle();
            }
            else if (flag == MouseEventFlags.Move)
            {
                Log.Info("left down p-> idle (move)");
                Log.Debug("left down cancelled");
                machine.Idle();
            }
        }
    }
}