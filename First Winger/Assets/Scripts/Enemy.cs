using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Enemy : Actor
{
    
    public enum State : int
    {
        None = -1,
        Ready = 0,
        Appear,
        Battle,
        Dead,
        Disappear,
    }
    [SerializeField]
    [SyncVar]
    State CurrentState = State.None;
    // Start is called before the first frame update
    const float MaxSpeed = 10.0f;
    const float MaxSpeedTime = 0.5f;
    

    [SerializeField]
    [SyncVar]
    Vector3 TargetPosition;

    [SerializeField]
    [SyncVar]
    float CurrentSpeed;
    [SyncVar]
    Vector3 CurrenrVelocity;

    [SerializeField]
    Transform FireTransform;

    [SerializeField]
    [SyncVar]
    float BulletSpeed = 1;

    [SyncVar]
    float MoveStartTime = 0.0f;
    [SyncVar]
    float LastActionUpdateTime= 0.0f;

    [SerializeField]
    [SyncVar]
    int FireRemainCount = 1;

    [SerializeField]
    [SyncVar]
    int GamePoint = 10;

    [SyncVar]
    [SerializeField]
    string filePath;

    public string FilePath
    {
        get{ return filePath; }
        set{filePath = value; }
    }
    [SyncVar]
    Vector3 AppearPoint;
    [SyncVar]
    Vector3 DisappearPoint;

    [SerializeField]
    [SyncVar]
    float ItemDropRate;
    [SerializeField]
    [SyncVar]
    int ItemDropID;
    protected override void Initialize()
    {

        base.Initialize();
        Debug.Log("Enemy: Initialize");
        InGameSceneMain inGameSceneMain = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>();
        if (!((FWNetworkManager)FWNetworkManager.singleton).isServer)
        {
           
            transform.SetParent(inGameSceneMain.EnemyManager.transform);
            inGameSceneMain.EnemyCacheSystem.Add(FilePath, gameObject);
            gameObject.SetActive(false);
        }
        if(actorInstanceID != 0)
        {
            inGameSceneMain.ActorManager.Regist(actorInstanceID, this);
        }
    }
    protected override void UpdateActor()
    {
        switch (CurrentState)
        {
            case State.None:
                break;
            case State.Ready:
                UpdateReady();
                break;
            case State.Dead:
                break;
            case State.Appear:
            case State.Disappear:
                UpdateSpeed();
                UpdateMove();
                break;
            case State.Battle:
                UpdateBattle();
                break;
        }
        if (CurrentState == State.Appear || CurrentState == State.Disappear)
        {
            UpdateMove();
            UpdateSpeed();
        }
    }
    // Update is called once per frame


    void UpdateSpeed()
    {
       CurrentSpeed = Mathf.Lerp(CurrentSpeed, MaxSpeed, (Time.time)/MaxSpeedTime);
    }
    void UpdateMove()
    {
        float distance = Vector3.Distance(TargetPosition, transform.position);
        if (distance == 0)
        {
            Arrived();
            return;
        }
        CurrenrVelocity = (TargetPosition-transform.position).normalized * CurrentSpeed;
        transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref CurrenrVelocity, distance / CurrentSpeed, MaxSpeed);
    }
    void Arrived()
    {
        CurrentSpeed = 0.0f;
        if(CurrentState == State.Appear)
        {
            CurrentState = State.Battle;
            LastActionUpdateTime = Time.time;
        }
        else //if (CurrentState == State.Disappear)
        {
            CurrentState = State.None;
        }
    }
    public void Reset(SquadronStruct data)
    {
        //EnemyStruct enemyStruct = SystemManager.Instance.EnemyTable.GetEnemy(data.EnemyID);
        //CurrentHP = MaxHP = enemyStruct.MaxHP;
        //Damage = enemyStruct.Damage;
        //crashDamage = enemyStruct.CrashDamage;
        //BulletSpeed = enemyStruct.BulletSpeed;
        //FireRemainCount = enemyStruct.FireReaminCount;
        //GamePoint = enemyStruct.GamePoint;

        //AppearPoint = new Vector3(data.AppearPointX, data.AppearPointY,0);
        //DisappearPoint = new Vector3(data.DisappearPointX, data.DisappearPointY,0);


        //CurrentState = State.Ready;
        //LastActionUpdateTime = Time.time;
        if (isServer)
        {
            RpcReset(data);
        }
        else
        {
            CmdReset(data);
            if (isLocalPlayer)
                ResetData(data);
        }

        
    }

    void ResetData(SquadronStruct data)
    {
        EnemyStruct enemyStruct = SystemManager.Instance.EnemyTable.GetEnemy(data.EnemyID);
        Debug.Log("내 아이디는" + data.EnemyID);
        CurrentHP = MaxHP = enemyStruct.MaxHP;
        Damage = enemyStruct.Damage;
        crashDamage = enemyStruct.CrashDamage;
        BulletSpeed = enemyStruct.BulletSpeed;
        FireRemainCount = enemyStruct.FireReaminCount;
        GamePoint = enemyStruct.GamePoint;

        AppearPoint = new Vector3(data.AppearPointX, data.AppearPointY, 0);
        DisappearPoint = new Vector3(data.DisappearPointX, data.DisappearPointY, 0);
        ItemDropRate = enemyStruct.ItemDropRate;
        ItemDropID = enemyStruct.ItemDropID;

        CurrentState = State.Ready;
        LastActionUpdateTime = Time.time;
        isDead = false;
    }
    public void Appear(Vector3 targetPos)
    {
        TargetPosition = targetPos;
        CurrentSpeed = MaxSpeed;

        CurrentState = State.Appear;
        MoveStartTime = Time.time;
    }
    void Disappear(Vector3 targetPos)
    {
        TargetPosition = targetPos;
        CurrentSpeed = 0;
        CurrentState = State.Disappear;
        MoveStartTime = Time.time;
    }

    void UpdateBattle()
    {
        
        if(Time.time - LastActionUpdateTime > 1.0f)
        {
            if(FireRemainCount > 0)
            {
                Fire();
                FireRemainCount--;
            }
            else
            {
                Disappear(DisappearPoint);
            }
            
            LastActionUpdateTime = Time.time;
        }
    }
    void UpdateReady()
    {
        if(Time.time - LastActionUpdateTime > 1.0f)
        {
            Appear(AppearPoint);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
        Player player = other.GetComponentInParent<Player>();
        if (player)
        {
            if (!player.IsDead)
            {
                BoxCollider boxCollider = (BoxCollider)other;
                Vector3 crashPos = player.transform.position + boxCollider.center;
                crashPos.x += boxCollider.size.x * 0.5f;
                player.OnCrash(CrashDamage, crashPos);
            }          
            
        }
           
    }
    public override void OnCrash(int damage, Vector3 crashPos)
    {
       base.OnCrash(damage, crashPos);

    }
    public void Fire()
    { 
        Bullet bullet = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().BulletManager.Generate(BulletManager.EnemyBulletIndex);
        bullet.Fire(actorInstanceID, FireTransform.position, -FireTransform.right, BulletSpeed,Damage);
    }

    protected override void OnDead()
    {
        base.OnDead();
        InGameSceneMain inGameSceneMain = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>();
        inGameSceneMain.GamePointAccumlator.Accumulate(GamePoint);
        inGameSceneMain.EnemyManager.RemoveEnemy(this);
        GenerateItem();


        CurrentState = State.Dead;
       
    }
    protected override void DecreaseHP(int value, Vector3 damagePos)
    {
        base.DecreaseHP(value,damagePos);
        Vector3 damagePoint = damagePos + Random.insideUnitSphere * 0.5f;
        SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().DamageManager.Generate(DamageManager.EnemyDamageIndex, damagePoint* 0.5f, value,Color.magenta);
    }
    [Command]
    public void CmdReset(SquadronStruct data)
    {
        ResetData(data);
        base.SetDirtyBit(1);
    }
    [ClientRpc]
    public void RpcReset(SquadronStruct data)
    {
        ResetData(data);
        base.SetDirtyBit(1);
    }
    void GenerateItem()
    {
        if (!isServer) 
            return;
        float ItemGen = Random.Range(0.0f, 100.0f);
        if (ItemGen > ItemDropRate)
            return;
        ItemDropTable itemDropTable = SystemManager.Instance.ItemDropTable;
        ItemDropStruct dropStruct = itemDropTable.GetDropData(ItemDropID);

        ItemGen = Random.Range(0, dropStruct.Rate1 + dropStruct.Rate2 + dropStruct.Rate3);
        int ItemIndex = -1;
        if (ItemGen <= dropStruct.Rate1)
            ItemIndex = dropStruct.ItemID1;
        else if(ItemGen <= dropStruct.Rate1+dropStruct.Rate2)
        {
            ItemIndex = dropStruct.ItemID2;
        }
        else if(ItemGen <= dropStruct.Rate1+dropStruct.Rate2+dropStruct.Rate3)
        {
            ItemIndex = dropStruct.ItemID3;
        }
        InGameSceneMain inGameSceneMain = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>();
        inGameSceneMain.ItemBoxManager.Generate(ItemIndex, transform.position);
    }
}
