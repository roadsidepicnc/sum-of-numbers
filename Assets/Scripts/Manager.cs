using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay
{
    public abstract class Manager : MonoBehaviour
    {
        public bool IsInitialized { get; protected set; }

        protected List<Manager> _initializeDependencies = new();
        
        private void OnDestroy()
        {
            Deregister();
        }

        public virtual async void Initialize()
        {
            SetInitializeDependencies();
            await UniTask.WaitUntil(AreAllDependenciesProvided);
            Register();
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
        
        protected virtual void Register()
        {
        }

        protected virtual void Deregister()
        {
        }
    }
}