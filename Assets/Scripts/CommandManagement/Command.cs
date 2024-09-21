using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CommandManagement
{
    public class Command : ICommand
    {
        public bool IsRunning;
        public bool IsCompleted;
        protected static Func<UniTask> Task;
        public Action OnCompleted;
        
        public Command(Func<UniTask> task)
        {
            Task = task;
        }
        
        public virtual async void Execute()
        {
            Debug.Log(("Execute Command:", ToString()));
            IsRunning = true;

            if (Task != null)
            {
                await Task();
            }
            
            Complete();
        }

        public virtual void Complete(bool closeMask = true)
        {
            OnCompleted?.Invoke();
            IsRunning = false;
            IsCompleted = true;
        }
        
        public override string ToString()
        {
            return Task.Method.Name;
        }
    }
}