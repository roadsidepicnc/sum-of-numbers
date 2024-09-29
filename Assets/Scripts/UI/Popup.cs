using System;
using CommandManagement;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public abstract class Popup : MonoBehaviour
    {
        [SerializeField] protected RectTransform rectTransform;
        [SerializeField] protected Button cancelButton;
        [SerializeField] protected PopupType popupType;
        
        private PopupCommand _command;

        public PopupType PopupType => popupType;
        
        protected bool IsOpening;
        
        public bool IsActiveInHierarchy => rectTransform.gameObject.activeInHierarchy;
        
        public virtual bool Displayable => true;
        
        public void SetCommand(PopupCommand command)
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
}