using System;
using Cysharp.Threading.Tasks;
using UI;
using UnityEngine;

namespace CommandManagement
{
    public class PanelCommand : Command
    {
        public Panel Panel;
        
        public PanelCommand(Panel panel, Func<UniTask> task = null) : base(Task)
        {
            Panel = panel;
            panel.SetCommand(this);
        }
        
        public override async void Execute()
        {
            IsRunning = true;
            if (Panel.Displayable)
            {
                Debug.Log("Execute Command: " + ToString());
                if (Task != null)
                {
                    await Task();
                }
                
                await Panel.Open();
            }
            else
            {
                Debug.Log("Execute Command as Auto Complete: " + ToString());
                Complete();
            }
        }
        
        public override string ToString()
        {
            return Panel.gameObject.name;
        }
    }
}