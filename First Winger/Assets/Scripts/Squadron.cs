using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class EnemyGenerateData
{
    public string FilePath;
    public int MaxHP;
    public int Damage;
    public int CrashDamage;
    public int BulletSpeed;
    public int FireRemainCount;
    public int GamePoint;

    public Vector3 GeneratePoint;
    public Vector3 AppearPoint;

    public Vector3 DisappearPoint;
}
public class Squadron : MonoBehaviour
{
    [SerializeField]
    EnemyGenerateData[] enemyGenerateDatas;


    void Start()
    {
        
    }

    
    void Update()
    {

    }
    public void GenerateAllData()
    {
        for(int i = 0; i < enemyGenerateDatas.Length; i++)
        {
            SystemManager.Instance.EnemyManager.GenerateEnemy(enemyGenerateDatas[i]);
        }
    }
}
