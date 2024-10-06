using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utilities;

namespace Gameplay
{
    public abstract class Manager : MonoBehaviour, ISubscribable
    {
        public bool IsInitialized { get; protected set; }

        protected List<Manager> _initializeDependencies = new();
        
        private void OnDestroy()
        {
            Unsubscribe();
        }

        public virtual async void Initialize()
        {
            SetInitializeDependencies();
            await UniTask.WaitUntil(AreAllDependenciesProvided);
            Subscribe();
        }

        protected virtual void SetInitializeDependencies()
        {
        }

        private bool AreAllDependenciesProvided()
        {
            foreach (var manager in _initializeDependencies)
            {
                if (!manager.IsInitialized)
                {
                    return false;
                }
            }

            return true;
        }
        
        public virtual void Subscribe()
        {
        }

        public virtual void Unsubscribe()
        {
        }
    }
}