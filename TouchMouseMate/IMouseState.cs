namespace Lextm.TouchMouseMate
{
    public interface IMouseState
    {
        void Process(MouseEventFlags flag, StateMachine machine);
    }
}