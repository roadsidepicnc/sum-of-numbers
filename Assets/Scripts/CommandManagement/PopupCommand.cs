using System;
using Cysharp.Threading.Tasks;
using UI;
using UnityEngine;

namespace CommandManagement
{
    public class PopupCommand : Command
    {
        public Popup Popup;
        
        public PopupCommand(Popup popup, Func<UniTask> task = null) : base(Task)
        {
            Popup = popup;
            popup.SetCommand(this);
        }
        
        public override async void Execute()
        {
            IsRunning = true;
            if (Popup.Displayable)
            {
                Debug.Log("Execute Command: " + ToString());
                if (Task != null)
                {
                    await Task();
                }
                
                await Popup.Open();
            }
            else
            {
                Debug.Log("Execute Command as Auto Complete: " + ToString());
                Complete();
            }
        }
        
        public override string ToString()
        {
            return Popup.gameObject.name;
        }
    }
}