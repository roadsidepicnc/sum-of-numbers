using System;
using GridManagement;

namespace Utilities
{
    public static class Signals
    {
        public static Action <GameState> GameStateChanged;
        public static Action <Cell> CellInteracted;
    }
}

public enum GameState
{
    Initializing,
    Running,
    Finished,
    Paused
}