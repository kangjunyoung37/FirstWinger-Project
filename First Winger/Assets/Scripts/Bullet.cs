using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OwnerSide : int
{
    Player = 0,
    Enemy
}
public class Bullet : MonoBehaviour
{
    const float LifeTime = 15.0f;
    // Start is called before the first frame update
    OwnerSide ownerSide = OwnerSide.Player;

    [SerializeField]
    Vector3 MoveDirection = Vector3.zero;

    [SerializeField]
    float Speed = 0.0f;

    bool NeddMove = false;

    bool Hited = false;

    float FiredTime;

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
    public void Fire(OwnerSide fireownerSide , Vector3 firePosition,Vector3 direction,float speed)
    {
        ownerSide = fireownerSide;
        transform.position = firePosition;
        MoveDirection = direction;
        Speed = speed; 

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
        Collider myCollider = GetComponentInChildren<Collider>();
        myCollider.enabled = false;
        Hited = true;
        NeddMove = false;
        if (ownerSide == OwnerSide.Player)
        {
            Enemy enemy = collider.GetComponentInParent<Enemy>();

        }
        else
        {
            Player player = collider.GetComponentInParent<Player>();
        }
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
        Destroy(gameObject);
    }
}
