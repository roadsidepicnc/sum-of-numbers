using System;
using Gameplay;
using GridManagement;

public class SignalManager : Manager
{
    public Action <GameState> GameStateChanged;
    public Action <Cell> CellInteracted;
    public Action ClickModeChanged;

    public override void Initialize()
    {
        base.Initialize();

        IsInitialized = true;
    }
}