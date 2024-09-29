using System.Collections.Generic;
using CommandManagement;
using Gameplay;
using UnityEngine;
using Zenject;

namespace UI
{
    public class PopupManager : Manager
    {
        [Inject] private PopupPrefabCatalog popupPrefabCatalog;
        
        [SerializeField] private Transform parent;
        
        private Dictionary<PopupType, Popup> _instantiatedPopups;
        
        public override void Initialize()
        {
            base.Initialize();
            
            _instantiatedPopups = new();

            IsInitialized = true;
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
                popup.Initialize();
                _instantiatedPopups.Add(popupType, popup);
            }
            
            CommandManager.PushCommandInMainQueue(new PopupCommand(popup), interrupt, prepend);
        }
    }
}