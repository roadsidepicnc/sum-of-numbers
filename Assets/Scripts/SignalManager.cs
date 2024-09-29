using System;
using Gameplay;
using GridManagement;

public class SignalManager : Manager
{
    public Action <GameState> GameStateChanged;
    public Action <Cell> CellInteracted;

    public override void Initialize()
    {
        base.Initialize();

        IsInitialized = true;
    }
}