namespace Lextm.TouchMouseMate
{
    public class LeftDown : IMouseState
    {
        public void Process(MouseEventFlags flag, StateMachine machine)
        {
            if (flag == MouseEventFlags.LeftUp)
            {
                Log.Info("left down->idle");
                machine.Idle();
            }
        }
    }
}