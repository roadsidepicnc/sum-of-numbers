using System;
using Gameplay;
using GridManagement;

public class SignalManager : Manager
{
    public Action <GameState> GameStateChanged;
    public Action <Cell> CellInteracted;
    public Action ResetGrid;

    public override void Initialize()
    {
        base.Initialize();

        IsInitialized = true;
    }
}