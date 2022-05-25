using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    State CurrentState = State.None;
    // Start is called before the first frame update
    const float MaxSpeed = 10.0f;
    const float MaxSpeedTime = 0.5f;
    

    [SerializeField]
    Vector3 TargetPosition;

    [SerializeField]
    float CurrentSpeed;

    Vector3 CurrenrVelocity;

    [SerializeField]
    Transform FireTransform;

    [SerializeField]
    float BulletSpeed = 1;

    float MoveStartTime = 0.0f;
    float LastActionUpdateTime= 0.0f;

    [SerializeField]
    int FireRemainCount = 1;

    [SerializeField]
    int GamePoint = 10;

    public string FilePath
    {
        get;
        set;
    }
    Vector3 AppearPoint;
    Vector3 DisappearPoint;
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
        EnemyStruct enemyStruct = SystemManager.Instance.EnemyTable.GetEnemy(data.EnemyID);
        CurrentHP = MaxHP = enemyStruct.MaxHP;
        Damage = enemyStruct.Damage;
        crashDamage = enemyStruct.CrashDamage;
        BulletSpeed = enemyStruct.BulletSpeed;
        FireRemainCount = enemyStruct.FireReaminCount;
        GamePoint = enemyStruct.GamePoint;

        AppearPoint = new Vector3(data.AppearPointX, data.AppearPointY,0);
        DisappearPoint = new Vector3(data.DisappearPointX, data.DisappearPointY,0);
        

        CurrentState = State.Ready;
        LastActionUpdateTime = Time.time;  
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
                player.OnCrash(this, CrashDamage, crashPos);
            }          
            
        }
           
    }
    public override void OnCrash(Actor attacker, int damage, Vector3 crashPos)
    {
       base.OnCrash(attacker, damage, crashPos);

    }
    public void Fire()
    { 
        Bullet bullet = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().BulletManager.Generate(BulletManager.EnemyBulletIndex);
        bullet.Fire(this, FireTransform.position, -FireTransform.right, BulletSpeed,Damage);
    }

    protected override void OnDead(Actor killer)
    {
        base.OnDead(killer);
        SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().GamePointAccumlator.Accumulate(GamePoint);
        SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().EnemyManager.RemoveEnemy(this);
        CurrentState = State.Dead;
       
    }
    protected override void DecreaseHP(Actor attacker, int value, Vector3 damagePos)
    {
        base.DecreaseHP(attacker, value,damagePos);
        Vector3 damagePoint = damagePos + Random.insideUnitSphere * 0.5f;
        SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().DamageManager.Generate(DamageManager.EnemyDamageIndex, damagePoint* 0.5f, value,Color.magenta);
    }
}
