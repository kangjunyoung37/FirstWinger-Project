using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public const int PlayerBulletIndex = 0;
    public const int EnemyBulletIndex = 1;

    [SerializeField]
    PrefabCacheData[] bulletFiles;

    Dictionary<string , GameObject> FileCache = new Dictionary<string, GameObject>();

     void Start()
    {
        Prepare();
    }
     void Update()
    {
        
    }

    public GameObject Load(string resourcePath)
    {
        GameObject go = null;
        if (FileCache.ContainsKey(resourcePath))
        {
            go = FileCache[resourcePath];
        }
        else
        {
            go = Resources.Load<GameObject>(resourcePath);
            if (!go)
            {
                Debug.LogError("Load error! path = " + resourcePath);
                return null;
            }
            FileCache.Add(resourcePath, go);
        }
        return go;
    }
    public void Prepare()
    {
        for(int i = 0; i < bulletFiles.Length; i++)
        {
            GameObject go = Load(bulletFiles[i].filePath);
            SystemManager.Instance.BulletCacheSystem.GenerateCache(bulletFiles[i].filePath, go, bulletFiles[i].cacheCount);

        }
    }
    public Bullet Generate(int index)
    {
        string filePath = bulletFiles[index].filePath;
        GameObject go = SystemManager.Instance.BulletCacheSystem.Archive(filePath);

        Bullet bullet = go.GetComponent<Bullet>();
        bullet.FilePath = filePath;

        return bullet;
    }
    public bool Remove(Bullet bullet)
    {
        SystemManager.Instance.BulletCacheSystem.Restore(bullet.FilePath, bullet.gameObject);
        return true;
    }
}
