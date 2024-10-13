using System;
using CommandManagement;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public abstract class Popup : MonoBehaviour
    {
        [Inject] private GameManager _gameManager;
        
        [SerializeField] protected RectTransform content;
        [SerializeField] protected Image background;
        [SerializeField] protected Button cancelButton;
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
            _gameManager.SetInputState(InputState.NonActive);
            IsOpening = true;
            gameObject.SetActive(true);
            content.DOKill();
            var duration = .3f;
            background.DOFade(180f / 255f, duration).From(0f);
            await content.DoHitScale(callback, duration);
            IsOpening = false;
            _gameManager.SetInputState(InputState.Active);
        }
        
        public virtual async UniTask Close(Action callback = null, bool ignoreCommand = false)
        {
            _gameManager.SetInputState(InputState.NonActive);
            content.DOKill();
            content.DOScale(Vector3.zero, .25f);
            background.DOKill();
            await background.DOFade(0f, .4f);
            gameObject.SetActive(false);
            _command?.Complete();
            _gameManager.SetInputState(InputState.Active);
        }
    }
}