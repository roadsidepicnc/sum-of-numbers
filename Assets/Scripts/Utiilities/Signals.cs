using System;
using GridManagement;

namespace Utilities
{
    public static class Signals
    {
        //public static Action <GameState> GameStateChanged;
        //public static Action GameInitialized;
        //public static Action <Cell> CellInteracted;
        //public static Action ResetGrid;
    }
}

public enum GameState
{
    Loading,
    OnGameplay,
    OnMenu,
    Won,
    Lost,
    Paused
}