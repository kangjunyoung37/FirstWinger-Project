using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InGameSceneMain : BaseSceneMain
{
   
    public GameState CurrentGameState
    {
        get { return NetworkTransfer.CurrentGameState; }
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
            if(!player)
            {
                Debug.LogWarning("Main Player is not setted");
            }
            return player;
        }
        set
        {
            player = value;
        }
    }

    GamePointAccumlator gamePointAccumlator = new GamePointAccumlator();

    public GamePointAccumlator GamePointAccumlator
    {
        get { return gamePointAccumlator; }
    }

    PrefabCacheSystem damageCacheSystem = new PrefabCacheSystem();
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

    [SerializeField]
    DamageManager damageManager;
    public DamageManager DamageManager
    {
        get { return damageManager; }
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
    public PrefabCacheSystem DamageCacheSystem
    {
        get { return damageCacheSystem; }
    }
    [SerializeField]
    SquadronManager squadronManager;
    public SquadronManager SquadronManager
    {
        get { return squadronManager; }
    }



    [SerializeField]
    Transform mainBGQuadTransform;

    public Transform MainBGQuadTransform
    {
        get { return mainBGQuadTransform; }
    }
    //protected override void OnStart()
    //{
    //    SceneStartTime = Time.time;
    //}
    //protected override void UpdateScene()
    //{
    //    base.UpdateScene();

    //    float currentTime = Time.time;
    //    if(currentGameState == GameState.Ready)
    //    {
    //        if(currentTime-SceneStartTime > GameReadyIntaval)
    //        {
    //            //SquadronManager.StartGame();
    //            currentGameState = GameState.Running;
    //        }
    //    }
    //}

    [SerializeField]
    Transform playerStartTransform1;
    public Transform PlayerStartTransform1
    {
        get { return playerStartTransform1; }
    }

    [SerializeField]
    Transform playerStartTrasform2;
    public Transform PlayerStartTrasform2
    {
        get { return playerStartTrasform2; }

    }

    [SerializeField]
    InGameNetwrokTransfer inGameNetwrokTransfer;
    InGameNetwrokTransfer NetworkTransfer
    {
        get
        {
            return inGameNetwrokTransfer;
        }
    }
    [SerializeField]
    ActorManager actorManager = new ActorManager();
    public ActorManager ActorManager
    {
        get { return actorManager; }
    }
    public void GameStart()
    {
        NetworkTransfer.RpcGameStart();
    }
}
