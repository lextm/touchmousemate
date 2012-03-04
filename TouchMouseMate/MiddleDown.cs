namespace Lextm.TouchMouseMate
{
    public class MiddleDown : IMouseState
    {
        public void Process(MouseEventFlags flag, StateMachine machine)
        {
            if (flag == MouseEventFlags.LeftUp || flag == MouseEventFlags.RightUp)
            {
                Log.Info("middle down->idle");
                machine.Idle();
            }
        }
    }
}