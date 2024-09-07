using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ObjectPool
{
    public class ObjectPool
    {
        private readonly Stack<PoolObject> _pooledObjects = new();
        private readonly HashSet<PoolObject> _activeObjects = new();
        private readonly PoolObject _prefab;
        private readonly int _initialObjectCount;
        
        private DiContainer _diContainer;
        
        public int PooledObjectCount => _pooledObjects.Count;
        public int ActiveObjectCount => _activeObjects.Count;

        public ObjectPool(PoolObject prefab, int initialObjectCount, DiContainer diContainer)
        {
            _prefab = prefab;
            _initialObjectCount = initialObjectCount;
            _diContainer = diContainer;
        }

        public ObjectPool Initialize(Transform parent)
        {
            for (var i = 0; i < _initialObjectCount; i++)
            {
                var poolObject = GameObject.Instantiate(_prefab);
                ResetPoolObject(poolObject, parent);
            }

            return this;
        }

        public PoolObject GetPoolObject(Transform parent, Action<PoolObject> getPoolObjectAction = null, Action<PoolObject> resetPoolObjectAction = null)
        {
            var poolObject = PooledObjectCount > 0 ? _pooledObjects.Pop() : GameObject.Instantiate(_prefab);
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
    }
}

public enum PoolObjectType
{
    Cell = 1
}