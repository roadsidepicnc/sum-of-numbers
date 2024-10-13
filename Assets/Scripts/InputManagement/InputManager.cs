using Gameplay;
using UnityEngine;

namespace InputManagement
{
    public class InputManager : Manager
    {
        public static bool IsEnabled { get; private set; }
        
        private int _counter;
        
        public override void Initialize()
        {
            base.Initialize();

            IsInitialized = true;
        }
        
        private void SetEventSystemState(bool isActive)
        {
            if (isActive)
            {
                _counter++;
            }
            else
            {
                _counter--;
            }
            
            Debug.Log($"Input counter: {_counter}");
            CounterChanged();
        }
        
        private void CounterChanged()
        {
            IsEnabled = _counter == 0;
        }
    }
}