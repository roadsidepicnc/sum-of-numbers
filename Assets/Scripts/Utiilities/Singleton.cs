using UnityEngine;

namespace Utilities
{
	public class Singleton<TInstance> : SingletonBase where TInstance : Singleton<TInstance>
	{
		public bool isPersistent;
		
		public static TInstance Instance;

		public virtual void Awake()
		{
			if (isPersistent)
			{
				if (!Instance || Instance.gameObject == null)
				{
					Instance = this as TInstance;
					DontDestroyOnLoad(gameObject);
				}
				else
				{
					DestroyImmediate(gameObject);
				}
			}
			else
			{
				if (!Instance || Instance.gameObject == null)
				{
					Instance = this as TInstance;
				}
			}
		}

		public override void ClearInstance()
		{
			// Debug.LogError("ClearInstance: "+instance);
			Instance = null;
		}
	}

	public abstract class SingletonBase : MonoBehaviour
	{
		public abstract void ClearInstance();
	}
}