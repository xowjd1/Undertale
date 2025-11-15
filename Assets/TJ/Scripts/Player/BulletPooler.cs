using System.Collections.Generic;
using UnityEngine;

public class BulletPooler : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int poolSize = 20;
    
    private List<GameObject> bulletPool;
    private GameObject poolContainer;
    
    private void Awake()
    {
        bulletPool = new List<GameObject>();
        poolContainer = new GameObject($"Pool - {bulletPrefab.name}");
       
        CreatePooler();
    }

    private void CreatePooler()
    {
        for (int i = 0; i < poolSize; i++)
        {
            bulletPool.Add(CreateInstance());
        }
    }

    private GameObject CreateInstance()
    {
        GameObject newInstance = Instantiate(bulletPrefab);
        newInstance.transform.SetParent(poolContainer.transform);
        newInstance.SetActive(false);
        return newInstance;
    }

    public GameObject GetInstanceFromPool()
    {
        for (int i = 0; i < bulletPool.Count; i++)
        {
            if (!bulletPool[i].activeInHierarchy)
            {
                return bulletPool[i];
            }
        }

        GameObject newInstance = CreateInstance();
        bulletPool.Add(newInstance);
        
        return newInstance;
    }
    
}
