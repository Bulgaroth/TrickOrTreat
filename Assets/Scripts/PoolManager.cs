using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
	[Serializable]
	public class Pool
	{
		public string tag;
		public GameObject prefab;
		public int size;
	}

	public List<Pool> pools;
	public Dictionary<string, Queue<GameObject>> poolDictionnary = new();

    #region Singleton

	public static PoolManager instance;

	#endregion

    private void Awake()
	{
		instance = this;

		foreach (var pool in pools)
		{
			var objectPool = new Queue<GameObject>();
			
			for (int i = 0; i < pool.size; ++i)
			{
				var obj = Instantiate(pool.prefab, transform);
				objectPool.Enqueue(obj);
				obj.SetActive(false);
			}
			poolDictionnary.Add(pool.tag, objectPool);
		}
	}

	public GameObject GetElement(string tag)
	{
		var obj = poolDictionnary[tag].Dequeue();
		//obj.SetActive(true);
        poolDictionnary[tag].Enqueue(obj);
        return obj;
	}
}
