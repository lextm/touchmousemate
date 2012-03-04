namespace Lextm.TouchMouseMate
{
    public class Idle : IMouseState
    {
        public void Process(MouseEventFlags flag, StateMachine machine)
        {
            if (flag == MouseEventFlags.LeftDown)
            {
                Log.Info("idle -> left down p");
                machine.LeftDownPending();
            }
            else if (flag == MouseEventFlags.RightDown)
            {
                Log.Info("idle -> right down p");
                machine.RightDownPending();
            }
        }
    }
}