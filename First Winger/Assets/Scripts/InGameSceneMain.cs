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
    ItemBoxManager itemBoxManager;
    public ItemBoxManager ItemBoxManager
    {
        get { return itemBoxManager; }
    }
    [SerializeField]
    Player player;

    public Player Hero
    {
        get
        {

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
    PrefabCacheSystem itemBoxCacheSystem = new PrefabCacheSystem();

    public PrefabCacheSystem ItemBoxCacheSystem
    {
        get { return itemBoxCacheSystem; }
    }

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

    [SerializeField]
    int BossEnemyID;

    [SerializeField]
    Vector3 BossGeneratePos;

    [SerializeField]
    Vector3 BossAppearPos;

    public void GameStart()
    {
        NetworkTransfer.RpcGameStart();
    }
    public void ShowWaringUI()
    {
        NetworkTransfer.RpcShowWarningUI();
    }
    public void SetRunningState()
    {
        NetworkTransfer.RpcSetRunningState();
    }
    public void GenerateBoss()
    {
        SquadronStruct data = new SquadronStruct();
        data.EnemyID = BossEnemyID;
        data.GeneratePointX = BossGeneratePos.x;
        data.GeneratePointY = BossGeneratePos.y;
        data.AppearPointY = BossAppearPos.y;
        data.AppearPointX = BossAppearPos.x;
        data.DisappearPointX = -15.0f;
        data.DisappearPointY = 0.0f;

        EnemyManager.GenerateEnemy(data);
    }
}
