using System;
using CommandManagement;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UI.Popup;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public abstract class Panel : MonoBehaviour
    {
        [SerializeField] protected RectTransform rectTransform;
        [SerializeField] protected Button cancelButton;
        [SerializeField] protected PanelType panelType;
        
        private PanelCommand _command;

        public PanelType PanelType => panelType;
        
        protected bool IsOpening;
        
        public bool IsActiveInHierarchy => rectTransform.gameObject.activeInHierarchy;
        
        public virtual bool Displayable => true;
        
        public void SetCommand(PanelCommand command)
        {
            _command = command;
        }

        public virtual void Initialize()
        {
            gameObject.SetActive(false);
            transform.localScale = Vector3.zero;
        }
        
        public virtual async UniTask Open(Action callback = null)
        {
            IsOpening = true;
            rectTransform.DOKill();
            rectTransform.gameObject.SetActive(true);
            var duration = .3f;
            await rectTransform.DOHitScale(callback, duration);
            IsOpening = false;
        }
        
        public virtual async UniTaskVoid Close(Action callback = null, bool ignoreCommand = false)
        {
            rectTransform.DOKill();
            rectTransform.gameObject.SetActive(false);
            _command?.Complete();
        }
    }

    public enum PanelType
    {
        WinPopup
    }
}