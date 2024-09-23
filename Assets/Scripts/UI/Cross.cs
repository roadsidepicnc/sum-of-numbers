using DG.Tweening;
using ObjectPoolingSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Cross : PoolObject
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Image image;
        [SerializeField] private float tweenDuration;
        
        
        public void Set(float width, float height)
        {
            rectTransform.sizeDelta = new Vector2(width, height);
            image.DOFade(.65f, tweenDuration).From(0f);
        }
    }
}