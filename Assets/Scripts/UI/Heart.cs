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
        
        public void Set(Color color, bool isActive)
        {
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
            transform.DOShakePosition(duration, 15f, randomness: 0f);
            await UniTask.Delay((int) ((duration + .2f) * 1000));
            image.color = Color.white;
        }

        public async UniTask Lose()
        {
            if (!IsActive)
            {
                return;
            }
            
            await Shake();
            IsActive = false;
        }
    }
}