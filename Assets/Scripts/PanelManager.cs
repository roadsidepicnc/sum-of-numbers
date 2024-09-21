using System.Collections.Generic;
using CommandManagement;
using Gameplay;
using UnityEngine;
using Zenject;

namespace UI.Popup
{
    public class PanelManager : Manager
    {
        [SerializeField] private Transform parent;
        
        private PanelPrefabHelper _panelPrefabHelper;
        
        private Dictionary<PanelType, Panel> _instantiatedPanels;
        
        
        [Inject]
        private void InstallDependencies(PanelPrefabHelper panelPrefabHelper)
        {
            _panelPrefabHelper = panelPrefabHelper;
        }
        
        public override void Initialize()
        {
            base.Initialize();
            
            _instantiatedPanels = new();

            IsInitialized = true;
        }

        public void Show(PanelType panelType, bool interrupt = true, bool prepend = false)
        {
            Panel panel;
            
            if (_instantiatedPanels.ContainsKey(panelType))
            {
                panel = _instantiatedPanels[panelType];
            }
            else
            {
                var prefab = _panelPrefabHelper.GetPrefab(panelType);
                if (prefab == null)
                {
                    Debug.LogError("Couldn't get panel prefab from helper!");
                    return;
                }

                panel = Instantiate(prefab, parent);
                panel.Initialize();
                _instantiatedPanels.Add(panelType, panel);
            }
            
            CommandManager.PushCommandInMainQueue(new PanelCommand(panel), interrupt, prepend);
        }
    }
}