using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameSceneMain : BaseSceneMain
{
    const float GameReadyIntaval = 3.0f;
    public enum GameState : int
    {
        Ready = 0,
        Running,
        End,
    }

    GameState currentGameState = GameState.Ready;
    public GameState CurrentGameState
    {
        get { return currentGameState; }
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

    float SceneStartTime;
    protected override void OnStart()
    {
        SceneStartTime = Time.time;
    }
    protected override void UpdateScene()
    {
        base.UpdateScene();

        float currentTime = Time.time;
        if(currentGameState == GameState.Ready)
        {
            if(currentTime-SceneStartTime > GameReadyIntaval)
            {
                SquadronManager.StartGame();
                currentGameState = GameState.Running;
            }
        }
    }
}
