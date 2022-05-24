using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    [SerializeField]
    Vector3 MoveVector = Vector3.zero;

    [SerializeField]
    float Speed;

    [SerializeField]
    BoxCollider boxCollider;

    [SerializeField]
    Transform MainBGQuadTransform;

    [SerializeField]
    Transform FireTransform;

    [SerializeField]
    float BulletSpeed = 1;



    protected override void Initialize()
    {
        base.Initialize();
        PlayerStatePanel playerStatePanel = PanelManager.GetPanel(typeof(PlayerStatePanel)) as PlayerStatePanel;
        playerStatePanel.SetHP(CurrentHP, MaxHP);
    }
    // Update is called once per frame
    protected override void UpdateActor()
    {
        UpdateMove();
    }

    void UpdateMove()
    {
        if (MoveVector.sqrMagnitude == 0)
            return;

        MoveVector = AdjustMoveVector(MoveVector);

        transform.position += MoveVector;   
    }
    public void ProcessInput(Vector3 moveDirection)//
    {
        MoveVector = moveDirection * Speed * Time.deltaTime;

    }
    Vector3 AdjustMoveVector(Vector3 moveVector)
    {
        Vector3 result = Vector3.zero;

        result = boxCollider.transform.position + boxCollider.center + moveVector;
        if (result.x - boxCollider.size.x * 0.5f < -MainBGQuadTransform.lossyScale.x * 0.5f)
            moveVector.x = 0;
        if (result.x + boxCollider.size.x * 0.5f > MainBGQuadTransform.lossyScale.x * 0.5f)
            moveVector.x = 0;
        if (result.y - boxCollider.size.y * 0.5f < -MainBGQuadTransform.lossyScale.y * 0.5f)
            moveVector.y = 0;
        if (result.y + boxCollider.size.y * 0.5f > MainBGQuadTransform.lossyScale.y * 0.5f)
            moveVector.y = 0;

        return moveVector;
    }
    private void OnTriggerEnter(Collider other)
    {
       
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy)
        {
            if (!enemy.IsDead)
            {
                BoxCollider boxCollider = (BoxCollider)other;
                Vector3 crashPos = enemy.transform.position + boxCollider.center;
                crashPos.x += boxCollider.size.x * 0.5f;
                enemy.OnCrash(this, CrashDamage, crashPos);
            }
        }
           
    }
    public override void OnCrash(Actor attacker, int damege, Vector3 crashPos)
    {
        base.OnCrash(attacker, damege, crashPos);
    }
    public void Fire()
    {     
        if (IsDead)
            return;

        Bullet bullet = SystemManager.Instance.BulletManager.Generate(BulletManager.PlayerBulletIndex);
        bullet.Fire(this, FireTransform.position, FireTransform.right, BulletSpeed,Damage);

    }
    protected override void OnDead(Actor killer)
    {
        base.OnDead(killer);
        gameObject.SetActive(false);
    }

    protected override void DecreaseHP(Actor attacker, int value, Vector3 damagePos)
    {
        base.DecreaseHP(attacker, value, damagePos);
        PlayerStatePanel playerStatePanel = PanelManager.GetPanel(typeof(PlayerStatePanel)) as PlayerStatePanel;
        playerStatePanel.SetHP(CurrentHP, MaxHP);
        Vector3 damagePoint = damagePos + Random.insideUnitSphere * 0.5f;
        SystemManager.Instance.DamageManager.Generate(DamageManager.PlayerDamageIndex, damagePoint * 0.5f, value,Color.red);
    }

}
