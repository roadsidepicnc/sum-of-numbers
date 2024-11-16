using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay;
using UnityEngine;
using Zenject;

public abstract class SceneLoader : MonoBehaviour
{
    [Inject] protected SignalBus SignalBus;
    
    protected List<Manager> Managers;
    private CancellationTokenSource _cancellationTokenSource;
    private const int InitializationTimeout = 5000;
    
    private void Start()
    {
        Initialize();
    }

    protected abstract void Initialize();
    protected abstract void AddManagers();
    
    protected async UniTask InitializeManagers()
    {
        _cancellationTokenSource = new(InitializationTimeout);
        Managers = new();
        AddManagers();

        foreach (var manager in Managers)
        {
            manager.Initialize();
        }

        await UniTask.WaitUntil(AreAllManagersInitialized, cancellationToken: _cancellationTokenSource.Token);
    }

    private bool AreAllManagersInitialized() => Managers.All(manager => manager.IsInitialized);
}