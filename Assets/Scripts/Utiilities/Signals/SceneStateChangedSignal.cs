namespace Utilities.Signals
{
    public struct SceneStateChangedSignal
    {
        public SceneState SceneState { get; private set; }

        public SceneStateChangedSignal(SceneState sceneState)
        {
            SceneState = sceneState;
        }
    }
}