namespace Lextm.TouchMouseMate
{
    public class MiddleDownPending : IMouseState
    {
        public void Process(MouseEventFlags flag, StateMachine machine)
        {
            if (flag == MouseEventFlags.Absolute)
            {
                Log.Info("middle down p->middle down");
                machine.MiddleDown();
            }
            else if (flag == MouseEventFlags.Move)
            {
                Log.Info("middle down p-> idle (move)");
                Log.Debug("middle down cancelled");
                machine.Idle();
            }
        }
    }
}