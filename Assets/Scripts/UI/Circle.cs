using Cysharp.Threading.Tasks;
using DG.Tweening;
using ObjectPoolingSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Circle : PoolObject
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Image image;
        
        public void Set(Color color, float width, float height)
        {
            image.color = color;
            rectTransform.sizeDelta = new Vector2(width, height);
        }

        public override void Reset(Transform parent)
        {
            base.Reset(parent);
            image.color = Color.white;
        }

        public async UniTask PlayAnimation(float duration = .2f)
        {
            await transform.DOScale(Vector3.one, duration).From(Vector3.zero).SetEase(Ease.InOutBack);
        }
    }
}