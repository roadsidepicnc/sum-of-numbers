using Cysharp.Threading.Tasks;
using DG.Tweening;
using ObjectPoolingSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Heart : PoolObject
    {
        public bool IsActive { get; private set; }
        
        [SerializeField] private Image image;
        
        public void Set(Sprite sprite, Color color, bool isActive)
        {
            image.sprite = sprite;
            image.color = color;
            IsActive = isActive;
        }

        public override void Reset(Transform parent)
        {
            base.Reset(parent);
            image.color = Color.white;
        }

        private async UniTask Shake(float duration = .5f)
        {
            await transform.DOShakePosition(duration, 15f, randomness: 0f);
        }

        public async UniTask PlayLoseAnimation()
        {
            if (!IsActive)
            {
                return;
            }
            
            await Shake();
        }
    }
}