using LevelManagement;
using ObjectPoolingSystem;
using UnityEngine;
using Zenject;

namespace UI
{
    public abstract class SceneUIHandler : MonoBehaviour
    {
        [Inject] protected SignalBus SignalBus;
        [Inject] protected LevelManager LevelManager;
        [Inject] protected PopupManager PopupManager;
        
        private void Start()
        {
            Initialize();
        }
   
        private void Initialize()
        {
            SetButtons();
            SetTexts();
        }
        
        protected abstract void SetButtons();
        protected abstract void SetTexts();
    }
}