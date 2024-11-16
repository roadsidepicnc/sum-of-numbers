using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LevelManagement;
using ObjectPoolingSystem;
using UI;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class HeartManager : Manager
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private LevelManager _levelManager;
        [Inject] private ObjectPoolManager _objectPoolManager;

        [SerializeField] private Transform parent;
        
        [Header("Sprites")]
        [SerializeField] private Sprite activeHeartSprite;
        [SerializeField] private Sprite passiveHeartSprite;
        
        [Header("Colors")]
        [SerializeField] private Color activeHeartColor;
        [SerializeField] private Color passiveHeartColor;

        private List<Heart> _hearts;

        private int ActiveHeartCount => _hearts.FindAll(x => x.IsActive).Count;
        private int NonActiveHeartCount => _hearts.FindAll(x => !x.IsActive).Count;
        private int TotalHeartCount => _hearts.Count;

        public bool AreThereAnyActiveHeart => ActiveHeartCount > 0;

        public override void Initialize()
        {
            _hearts = new();

            base.Initialize();
            
            SetHearts(_levelManager.CurrentLevelCurrentHeartCount, _levelManager.CurrentLevelMaxHeartCount - _levelManager.CurrentLevelCurrentHeartCount);
            
            IsInitialized = true;
        }

        private void SetHearts(int activeCount, int passiveCount)
        {
            for (var i = 0; i < activeCount; i++)
            {
                var heart = _objectPoolManager.GetObject(PoolObjectType.Heart, parent) as Heart;
                heart?.Set(activeHeartSprite, activeHeartColor, true);
                _hearts.Add(heart);
            }
            
            for (var i = 0; i < passiveCount; i++)
            {
                var heart = _objectPoolManager.GetObject(PoolObjectType.Heart, parent) as Heart;
                heart?.Set(passiveHeartSprite, passiveHeartColor, false);
                _hearts.Add(heart);
            }
        }

        public async UniTask LoseHeart()
        {
            if (ActiveHeartCount <= 0)
            {
                return;
            }
            
            _levelManager.DecreaseHeartAtLevelData();
            
            for (var i = _hearts.Count - 1; i >= 0; i--)
            {
                if (_hearts[i].IsActive)
                {
                    var heart = _hearts[i];
                    await heart.PlayLoseAnimation();
                    heart.Set(passiveHeartSprite, passiveHeartColor, false);
                    break;
                }
            }
        }
    }
}