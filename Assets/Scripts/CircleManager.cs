using Gameplay;
using ObjectPoolingSystem;
using UnityEngine;
using Zenject;

namespace UI
{
    public class CircleManager : Manager
    {
        [Inject] private ObjectPoolManager _objectPoolManager;

        [SerializeField] private float circleSizeMultiplier;
        [SerializeField] private Color circleColor;

        public override void Initialize()
        {
            base.Initialize();

            IsInitialized = true;
        }

        public Circle Create(Transform parent, float cellWidth, float cellHeight)
        {
            var circle = _objectPoolManager.GetObject(PoolObjectType.Circle, parent) as Circle;
            circle?.Set(circleColor, cellWidth * circleSizeMultiplier, cellWidth * circleSizeMultiplier);
            return circle;
        }

        public void Reset(Circle circle)
        {
            _objectPoolManager.ResetObject(circle);
        }
    }
}