using ObjectPoolingSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GridLine : PoolObject
    {
        [SerializeField] private Image lineImage;
        
        public void Set(float width, float height, Color color)
        {
            lineImage.color = color;
            lineImage.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        }
    }
}