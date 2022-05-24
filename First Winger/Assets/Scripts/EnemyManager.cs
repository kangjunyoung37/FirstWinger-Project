using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    EnemyFactory enemyFactory;
    List<Enemy> enemies = new List<Enemy>();

    
    void Start()
    {
        Prepare();
    }
    [SerializeField]
    PrefabCacheData[] enemyFiles;
    
    void Update()
    {
    }
    public bool GenerateEnemy(EnemyGenerateData data)
    {

        string FilePath = SystemManager.Instance.s
        GameObject go = SystemManager.Instance.EnemyCacheSystem.Archive(data.FilePath);
        go.transform.position = data.GeneratePoint;
        Enemy enemy = go.GetComponent<Enemy>();
        enemy.FilePath = data.FilePath;
        enemy.Reset(data);
        return true;
    }

    public bool RemoveEnemy(Enemy enemy)
    {
        if (!enemies.Contains(enemy))
        {
            Debug.LogError("no exist Enemy");
            return false;
        }
        enemies.Remove(enemy);
        SystemManager.Instance.EnemyCacheSystem.Restore(enemy.FilePath, enemy.gameObject);
        return true;
    }
    public void Prepare()
    {
        for (int i = 0; i <enemyFiles.Length; i++)
        {
            GameObject go = enemyFactory.Load(enemyFiles[i].filePath);
            SystemManager.Instance.EnemyCacheSystem.GenerateCache(enemyFiles[i].filePath, go, enemyFiles[i].cacheCount);
        } 
    }
}
