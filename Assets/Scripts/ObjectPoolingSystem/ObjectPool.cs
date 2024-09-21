using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace ObjectPoolManagement
{
    public class ObjectPool
    {
        private readonly Stack<PoolObject> _pooledObjects = new();
        private readonly HashSet<PoolObject> _activeObjects = new();
        private PoolObject _prefab;
        private int _initialObjectCount;
        
        [Inject] private DiContainer _diContainer;
        
        public int PooledObjectCount => _pooledObjects.Count;
        public int ActiveObjectCount => _activeObjects.Count;
        
        
        public void Initialize(Transform parent, PoolObject prefab, int initialObjectCount)
        {
            _prefab = prefab;
            _initialObjectCount = initialObjectCount;
            
            for (var i = 0; i < _initialObjectCount; i++)
            {
                var poolObject = Object.Instantiate(_prefab);
                ResetPoolObject(poolObject, parent);
            }
        }

        public PoolObject GetPoolObject(Transform parent, Action<PoolObject> getPoolObjectAction = null, Action<PoolObject> resetPoolObjectAction = null)
        {
            var poolObject = PooledObjectCount > 0 ? _pooledObjects.Pop() : Object.Instantiate(_prefab);
            _activeObjects.Add(poolObject);
            _diContainer.InjectGameObject(poolObject.gameObject);
            poolObject.Initialize(parent, resetPoolObjectAction);
            getPoolObjectAction?.Invoke(poolObject);
            return poolObject;
        }

        public void ResetPoolObject(PoolObject poolObject, Transform parent)
        {
            _pooledObjects.Push(poolObject);
            _activeObjects.Remove(poolObject);
            poolObject.Reset(parent);
        }

        public void Reset(Transform parent)
        {
            foreach (var activeObject in _activeObjects)
            {
                ResetPoolObject(activeObject, parent);
            }
        }
        
        public class Factory : PlaceholderFactory<ObjectPool>
        {
        }
    }
}

public enum PoolObjectType
{
    Cell = 1,
    TargetScoreText = 2,
    GridLine = 3,
    Cross = 4
}