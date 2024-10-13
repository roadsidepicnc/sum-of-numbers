using System.Collections.Generic;
using CommandManagement;
using Gameplay;
using UnityEngine;
using Utilities.Signals;
using Zenject;

namespace UI
{
    public class PopupManager : Manager
    {
        [Inject] private PopupPrefabCatalog popupPrefabCatalog;
        [Inject] private DiContainer _diContainer;
        [Inject] private SignalBus _signalBus;
        
        [SerializeField] private Transform parent;
        
        private Dictionary<PopupType, Popup> _instantiatedPopups;
        
        public override void Initialize()
        {
            base.Initialize();
            
            _instantiatedPopups = new();

            IsInitialized = true;
        }
        
        public override void Subscribe()
        {
            _signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);

        }

        public override void Unsubscribe()
        {
            _signalBus.Unsubscribe<GameStateChangedSignal>(OnGameStateChanged);
        }

        public void Show(PopupType popupType, bool interrupt = true, bool prepend = false)
        {
            Popup popup;
            
            if (_instantiatedPopups.ContainsKey(popupType))
            {
                popup = _instantiatedPopups[popupType];
            }
            else
            {
                var prefab = popupPrefabCatalog.GetPrefab(popupType);
                if (prefab == null)
                {
                    Debug.LogError("Couldn't get panel prefab from helper!");
                    return;
                }

                popup = Instantiate(prefab, parent);
                _diContainer.InjectGameObject(popup.gameObject);
                popup.Initialize();
                _instantiatedPopups.Add(popupType, popup);
            }
            
            CommandManager.PushCommandInMainQueue(new PopupCommand(popup), interrupt, prepend);
        }

        private void OnGameStateChanged(GameStateChangedSignal gameStateChangedSignal)
        {
            if (gameStateChangedSignal.GameState == GameState.SceneIsChanged)
            {
                CommandManager.ClearAllCommands();
            }
        }
    }
}