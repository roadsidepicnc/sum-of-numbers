using System;
using UnityEngine;

namespace ObjectPool
{
    public interface IPoolable<T>
    {
        void Initialize(Transform parent, Action<PoolObject> resetAction);
        void Reset(Transform parent);
    }
}