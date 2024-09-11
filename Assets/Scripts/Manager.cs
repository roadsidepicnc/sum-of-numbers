using UnityEngine;

namespace Gameplay
{
    public abstract class Manager : MonoBehaviour
    {
        public bool IsInitialized { get; protected set; }
        
        private void OnDestroy()
        {
            Deregister();
        }

        public virtual void Initialize()
        {
            Register();
        }

        protected virtual void Register()
        {
        }

        protected virtual void Deregister()
        {
        }
    }
}