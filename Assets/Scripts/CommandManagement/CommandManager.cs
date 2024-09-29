using System;
using System.Collections.Generic;
using UnityEngine;

namespace CommandManagement
{
    public static class CommandManager
    {
        private static readonly LinkedList<Command> MainQueue = new ();
        private static Command _current;
        
        public static Action OnAllCommandsCompleted;
        
        private static bool IsCurrentCommandRunning => _current is { IsRunning: true };
        
        public static void PushCommandInMainQueue(Command command, bool interrupt = true, bool prepend = false)
        {
            Debug.Log(("Command:", command, "is pushed as interrupt:", interrupt, "- prepend:", prepend));
            command.OnCompleted += RemoveCommand;
            
            if (prepend)
            {
                MainQueue.AddFirst(command);
            }
            else
            {
                MainQueue.AddLast(command);
            }
            
            if (!interrupt)
            {
                return;
            }
            
            if (IsCurrentCommandRunning)
            {
                CompleteCurrentCommand();
            }
            else
            {
                Next();
            }
        }
        
        public static void Next()
        {
            if (_current is { IsRunning: false })
            {
                return;
            }
            
            _current = MainQueue.Count > 0 ? MainQueue.First.Value : null;

            switch (_current)
            {
                case { IsRunning: false }:
                    _current.Execute();
                    break;
                case null:
                    OnAllCommandsCompleted?.Invoke();
                    break;
            }
        }
        
        public static void CompleteCurrentCommand(bool ignoreCommand = false)
        {
            switch (_current)
            {
                case PopupCommand panelCommand:
                    panelCommand.Popup.Close(ignoreCommand:ignoreCommand);
                    break;
                default:
                    _current.Complete();
                    break;
            }
        }
        
        public static void RemoveCommand()
        {
            Debug.Log("Remove Current Command: " + (_current != null ? _current.ToString() : " Null!"));
            
            if (_current == null)
            {
                return;
            }
            
            _current.OnCompleted -= RemoveCommand;
                
            if (MainQueue.Contains(_current))
            {
                MainQueue.Remove(_current);
            }
                
            Next();
        }
    }
}