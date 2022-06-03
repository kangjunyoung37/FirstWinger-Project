using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class PrefabCacheData
{
    public string filePath;
    public int cacheCount;
}
public class PrefabCacheSystem 
{
  
    Dictionary<string, Queue<GameObject>> Cache = new Dictionary<string, Queue<GameObject>> ();

    public void GenerateCache(string filePath, GameObject gameObject, int cacheCount, Transform parentTransform = null)
    {
        if (Cache.ContainsKey(filePath))
        {
            Debug.LogWarning("Already cache generated! filePath = " + filePath);
            return;
        }

        else
        {
            Queue<GameObject> queue = new Queue<GameObject> ();
            for ( int i = 0; i < cacheCount; i++)
            {
                GameObject go = Object.Instantiate<GameObject>(gameObject, parentTransform);
                go.SetActive(false);
                queue.Enqueue(go);
                Enemy enemy = go.GetComponent<Enemy>();
                if(enemy != null)
                {
                    enemy.FilePath = filePath;
                    NetworkServer.Spawn(go);
                }

                Bullet bullet = go.GetComponent<Bullet>();
                if(bullet != null)
                {
                    bullet.FilePath = filePath;
                    NetworkServer.Spawn(go);
                }
                ItemBox box = go.GetComponent<ItemBox>();
                if(box != null)
                {
                    box.FilePath = filePath;
                    NetworkServer.Spawn(go);
                }
            }
            Cache.Add(filePath, queue);
        }



    }

    public GameObject Archive(string filePath, Vector3 position)
    {
        if (!Cache.ContainsKey(filePath))
        {
            Debug.LogError("Archive Error! no Cache generated! filePath" + filePath);
        }
        if(Cache[filePath].Count == 0)
        {
            Debug.LogWarning("Archive problem! not enough Count");
            return null;
        }

        GameObject go = Cache[filePath].Dequeue();
        go.SetActive(true);
        go.transform.position = position;
        if(((FWNetworkManager)FWNetworkManager.singleton).isServer)
        {
            Enemy enemy = go.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.SetPosition(position);
                enemy.RpcSetActive(true);
            }
            Bullet bullet = go.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.SetPosition(position);   
                bullet.RpcSetActive(true);
            }
            ItemBox box = go.GetComponent<ItemBox>();
            if (box != null)
            {   
                box.RpcSetPosition(position);
                box.RpcSetActive(true);
            }

        }
        return go;
    }

    public bool Restore(string filePath, GameObject gameObject)
    {
        if(!Cache.ContainsKey(filePath))
        {
            Debug.LogError("Archive Error! no Cache generated! filePath" + filePath);
        }
        gameObject.SetActive(false);
        if (((FWNetworkManager)FWNetworkManager.singleton).isServer)
        {
            Enemy enemy = gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.RpcSetActive(false);
            }
            Bullet bullet = gameObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.RpcSetActive(false);
            }
            ItemBox box = gameObject.GetComponent<ItemBox>();
            if (box != null)
            {
                box.RpcSetActive(false) ;
            }

        }
        Cache[filePath].Enqueue(gameObject);
        return true;
    }
    public void Add(string filePath, GameObject gameObject)
    {
        Queue<GameObject> queue;
        if(Cache.ContainsKey(filePath))
        {
            queue = Cache[filePath];
        }
        else
        {
            queue = new Queue<GameObject>();
            Cache.Add(filePath, queue);
        }
        queue.Enqueue(gameObject);
    }

}
