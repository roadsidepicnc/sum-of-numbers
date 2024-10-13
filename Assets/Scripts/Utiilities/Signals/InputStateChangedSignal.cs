using Gameplay;

namespace Utilities.Signals
{
    public struct InputStateChangedSignal
    {
        public InputState InputState { get; private set; }

        public InputStateChangedSignal(InputState inputState)
        {
            InputState = inputState;
        }
    }
}