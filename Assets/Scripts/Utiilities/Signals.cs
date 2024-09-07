using System;
using GridManagement;

namespace Utilities
{
    public static class Signals
    {
        public static Action <GameState, int> GameStateChanged;
        public static Action <Cell> CellInteracted;
    }
}

public enum GameState
{
    Running,
    Finished
}