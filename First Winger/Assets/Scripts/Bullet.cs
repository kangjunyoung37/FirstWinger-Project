using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    const float LifeTime = 15.0f;

    Actor Owner;

    [SerializeField]
    Vector3 MoveDirection = Vector3.zero;

    [SerializeField]
    float Speed = 0.0f;

    bool NeddMove = false;

    bool Hited = false;

    float FiredTime;

    [SerializeField]
    int Damage = 1;

    public string FilePath
    {
        get;
        set;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ProcessDisappearCondition())
            return;
    
        UpdateMove();
    }
    void UpdateMove()
    {
        if (!NeddMove)
            return;

        Vector3 moveVector = MoveDirection.normalized * Speed * Time.deltaTime;
        moveVector = AdjustMove(moveVector);
        transform.position += moveVector;


    }
    public void Fire(Actor Onwer , Vector3 firePosition,Vector3 direction,float speed,int damage)
    {
        Owner = Onwer;
        transform.position = firePosition;
        MoveDirection = direction;
        Speed = speed; 
        Damage = damage;
        NeddMove = true;
        FiredTime = Time.time;

    }
    Vector3 AdjustMove(Vector3 moveVector)
    {
        RaycastHit hitInfo;
       if( Physics.Linecast(transform.position,transform.position+moveVector ,out hitInfo ))
       {
            moveVector = hitInfo.point-transform.position;
            OnBulletCollision(hitInfo.collider);
       }
       return moveVector;
    }

    void OnBulletCollision(Collider collider)
    {
        if (Hited)
            return;
        
        if(collider.gameObject.layer == LayerMask.NameToLayer("EnemyBullet")||collider.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            return;
        }
        Actor actor = collider.GetComponentInParent<Actor>();
        if (actor && actor.IsDead || actor.gameObject.layer == Owner.gameObject.layer)
        {
            return;
        }

        actor.OnBulletHited(Owner, Damage, transform.position);
       

        Collider myCollider = GetComponentInChildren<Collider>();
        myCollider.enabled = false;
        Hited = true;
        NeddMove = false;
        GameObject go = SystemManager.Instance.EffectManager.GenerateEffect(EffectManager.BulletDisappearFxIndex, transform.position);
        go.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        Disapper();
    }

    private void OnTriggerEnter(Collider other)
    {
        OnBulletCollision(other);   
    }
    bool ProcessDisappearCondition()
    {
        if(transform.position.x > 15.0f || transform.position.x < -15.0f|| transform.position.y > 15.0f || transform.position.y < -15.0f)
        {
            Disapper();
            return true;
        }
        else if(Time.time - FiredTime > LifeTime)
        {
            Disapper();
            return true;
        }
            return false;
    }
    void Disapper()
    {
        SystemManager.Instance.BulletManager.Remove(this);
    }
}
