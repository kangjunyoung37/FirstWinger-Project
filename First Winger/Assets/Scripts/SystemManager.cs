using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    static SystemManager intance = null;

    public static SystemManager Instance
    {
        get { return intance; }
    }

    [SerializeField]
    EffectManager effectManager; 

    public EffectManager EffectManager
    {
        get { return effectManager; }
    }

    [SerializeField]
    Player player;

    public Player Hero
    {
        get
        {
            return player;
        }
    }

    GamePointAccumlator gamePointAccumlator = new GamePointAccumlator();

    public GamePointAccumlator GamePointAccumlator
    {
        get { return gamePointAccumlator; }
    }


    private void Awake()
    {
        if(intance != null)
        {
            Debug.LogError("SystemManager error!");
            Destroy(gameObject);
            return;
        }
        intance = this;
        DontDestroyOnLoad(gameObject); 

    }
    PrefabCacheSystem enemyCacheSystem = new PrefabCacheSystem();
    PrefabCacheSystem bulletCacheSystem = new PrefabCacheSystem();
    PrefabCacheSystem effectCacheSystem = new PrefabCacheSystem();
    [SerializeField]
    EnemyManager enemyManager;

    public EnemyManager EnemyManager
    {
        get { return enemyManager; }

    }

    [SerializeField]
    BulletManager bulletManager;
    public BulletManager BulletManager
    {
        get { return bulletManager; }
    }



    public PrefabCacheSystem EnemyCacheSystem
    {
        get { return enemyCacheSystem; }
    }
    public PrefabCacheSystem BulletCacheSystem
    {
        get { return bulletCacheSystem; }
    }

    public PrefabCacheSystem EffectCacheSystem
    {
        get { return effectCacheSystem; }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
