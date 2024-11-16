using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using Utilities.Signals;
using Zenject;

namespace ObjectPoolingSystem
{ 
    public class ObjectPoolManager : Manager
    {
        [Inject] private ObjectPool.Factory _objectPoolFactory;
        [Inject] private ObjectPoolContainer _objectPoolContainer;
        [Inject] private SignalBus _signalBus;
        
        [SerializeField] private int initialObjectCount;
        [SerializeField] private List<PoolObject> poolItemPrefabs;
        
        private readonly Dictionary<PoolObjectType, ObjectPool> _poolTypeTpPoolItemDictionary = new();
        
        private Transform PooledObjectsParent => _objectPoolContainer.transform;
        
        public override void Initialize()
        {
            base.Initialize();
            CreatePools(initialObjectCount);
            
            IsInitialized = true;
        }

        public override void Subscribe()
        {
            _signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
        }

        public override void Unsubscribe()
        {
            _signalBus.Unsubscribe<GameStateChangedSignal>(OnGameStateChanged);
        }

        private void CreatePools(int initialCount)
        {
            foreach (var poolObject in poolItemPrefabs)
            {
                var objectPool = _objectPoolFactory.Create();
                objectPool.Initialize(PooledObjectsParent, poolObject, initialCount);
                _poolTypeTpPoolItemDictionary.Add(poolObject.poolObjectType, objectPool);
            }
            
            _objectPoolContainer.PoolObjects.Clear();
        }

        public PoolObject GetObject(PoolObjectType poolObjectType, Transform parent = null)
        {
            return _poolTypeTpPoolItemDictionary[poolObjectType].GetPoolObject(parent);
        }

        public int GetActiveObjectCountOfPool(PoolObjectType poolObjectType) => _poolTypeTpPoolItemDictionary[poolObjectType].ActiveObjectCount;
        
        public void ResetObject(PoolObject poolObject, Transform parent = null)
        {
            var pool = _poolTypeTpPoolItemDictionary[poolObject.poolObjectType];
            pool.ResetPoolObject(parent == null ? PooledObjectsParent : parent, poolObject);
        }

        public void ResetPools()
        {
            foreach (var poolTypeToPoolItem in _poolTypeTpPoolItemDictionary)
            {
                poolTypeToPoolItem.Value.StoreObjects();
                poolTypeToPoolItem.Value.Reset(_objectPoolContainer.transform);
            }

            foreach (var poolObject in _objectPoolContainer.PoolObjects)
            {
                poolObject.transform.SetParent(PooledObjectsParent);
            }
        }

        private void OnGameStateChanged(GameStateChangedSignal gameStateChangedSignal)
        {
            if (gameStateChangedSignal.GameState is GameState.SceneIsChanged or GameState.SceneIsReloaded)
            {
                ResetPools();
            }
        }
    }
}