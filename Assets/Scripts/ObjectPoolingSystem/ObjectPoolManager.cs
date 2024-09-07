using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using Zenject;

namespace ObjectPoolManagement
{
    public class ObjectPoolManager : BaseManager
    {
        [SerializeField] private int initialObjectCount;
        [SerializeField] private Transform pooledObjectsParent;
        [SerializeField] private List<PoolObject> poolItemPrefabs;

        private readonly Dictionary<PoolObjectType, ObjectPool> _poolTypeTpPoolItemDictionary = new();

        private ObjectPool.Factory _objectPoolFactory;

        [Inject]
        public void InstallDependencies(ObjectPool.Factory objectPoolFactory)
        {
            _objectPoolFactory = objectPoolFactory;
        }

        public override void Initialize()
        {
            CreatePools(initialObjectCount);
            IsInitialized = true;
        }

        private void CreatePools(int initialObjectCount)
        {
            foreach (var poolObject in poolItemPrefabs)
            {
                var objectPool = _objectPoolFactory.Create(); 
                objectPool.Initialize(pooledObjectsParent, poolObject, initialObjectCount);
                _poolTypeTpPoolItemDictionary.Add(poolObject.poolObjectType, objectPool);
            }
        }

        public PoolObject GetObject(PoolObjectType poolObjectType, Transform parent = null)
        {
            return _poolTypeTpPoolItemDictionary[poolObjectType].GetPoolObject(parent);
        }

        public void ResetObject(PoolObject poolObject, Transform parent = null)
        {
            var pool = _poolTypeTpPoolItemDictionary[poolObject.poolObjectType];
            pool.ResetPoolObject(poolObject, parent == null ? pooledObjectsParent : parent);
        }

        public void ResetPools(Transform parent = null)
        {
            foreach (var poolTypeToPoolItem in _poolTypeTpPoolItemDictionary)
            {
                poolTypeToPoolItem.Value.Reset(parent == null ? pooledObjectsParent : parent);
            }
        }

        public int GetActiveObjectCountOfPool(PoolObjectType poolObjectType) => _poolTypeTpPoolItemDictionary[poolObjectType].ActiveObjectCount;
    }
}