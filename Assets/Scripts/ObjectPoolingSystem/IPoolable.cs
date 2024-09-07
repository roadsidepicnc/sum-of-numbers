using System;
using ObjectPoolManagement;
using UnityEngine;

namespace ObjectPoolManagement
{
    public interface IPoolable<T>
    {
        void Initialize(Transform parent, Action<PoolObject> resetAction);
        void Reset(Transform parent);
    }
}