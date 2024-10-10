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
        [SerializeField] protected RectTransform content;
        [SerializeField] protected Image background;
        [SerializeField] protected Button cancelButton;
        [SerializeField] protected CanvasGroup canvasGroup;
        [SerializeField] protected PopupType popupType;
        
        private PopupCommand _command;

        public PopupType PopupType => popupType;
        
        protected bool IsOpening;
        
        public bool IsActiveInHierarchy => gameObject.activeInHierarchy;
        
        public virtual bool Displayable => true;
        
        public void SetCommand(PopupCommand command)
        {
            _command = command;
        }

        public virtual void Initialize()
        {
            gameObject.SetActive(false);
            //transform.localScale = Vector3.zero;
        }
        
        public virtual async UniTask Open(Action callback = null)
        {
            IsOpening = true;
            gameObject.SetActive(true);
            content.DOKill();
            var duration = .3f;
            background.DOFade(180f / 255f, duration).From(0f);
            await content.DoHitScale(callback, duration);
            IsOpening = false;
        }
        
        public virtual async UniTaskVoid Close(Action callback = null, bool ignoreCommand = false)
        {
            canvasGroup.transform.DOKill();
            content.DOScale(Vector3.zero, .25f);
            await background.DOFade(0f, .4f);
            gameObject.SetActive(false);
            _command?.Complete();
        }
    }
}