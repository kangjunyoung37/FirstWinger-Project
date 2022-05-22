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
                enemy.OnCrash(this, CrashDamage);
            }
        }
           
    }
    public override void OnCrash(Actor attacker, int damege)
    {
        
        base.OnCrash(attacker, damege);
    }
    public void Fire()
    {     
        if (IsDead)
            return;

        Bullet bullet = SystemManager.Instance.BulletManager.Generate(BulletManager.PlayerBulletIndex);
        bullet.Fire(OwnerSide.Player, FireTransform.position, FireTransform.right, BulletSpeed,Damage);

    }
    protected override void OnDead(Actor killer)
    {
        base.OnDead(killer);
        gameObject.SetActive(false);
    }
}
