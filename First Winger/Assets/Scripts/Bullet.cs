using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour
{
    const float LifeTime = 15.0f;

    [SyncVar]
    [SerializeField]
    int OwnerInstanceID;

    [SyncVar]
    [SerializeField]
    Vector3 MoveDirection = Vector3.zero;

    [SyncVar]
    [SerializeField]
    float Speed = 0.0f;
    [SyncVar]
    bool NeddMove = false;
   
    [SyncVar]
    bool Hited = false;
    
    [SyncVar]
    float FiredTime;
    
    [SyncVar]
    [SerializeField]
    int Damage = 1;

    [SyncVar]
    [SerializeField]
    string filePath;

    public string FilePath
    {
        get { return filePath; }
        set { filePath = value; }
    }
    void Start()
    {
        if (!((FWNetworkManager)FWNetworkManager.singleton).isServer)
        {
            InGameSceneMain inGameSceneMain = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>();
            transform.SetParent(inGameSceneMain.BulletManager.transform);
            inGameSceneMain.BulletCacheSystem.Add(FilePath, gameObject);
            gameObject.SetActive(false);

        }
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
    public void Fire(int ownerIntanceID , Vector3 firePosition,Vector3 direction,float speed,int damage)
    {

        OwnerInstanceID = ownerIntanceID;
        SetPosition(firePosition);
        transform.position = firePosition;
        MoveDirection = direction;
        Speed = speed; 
        Damage = damage;
        NeddMove = true;
        FiredTime = Time.time;
        UpdateNetworkBullet();
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
        Actor owner = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().ActorManager.GetActor(OwnerInstanceID);
        Actor actor = collider.GetComponentInParent<Actor>();
        if (actor && actor.IsDead || actor.gameObject.layer == owner.gameObject.layer)
        {
            return;
        }

        actor.OnBulletHited(Damage, transform.position);
       

        Collider myCollider = GetComponentInChildren<Collider>();
        myCollider.enabled = false;
        Hited = true;
        NeddMove = false;
        GameObject go = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().EffectManager.GenerateEffect(EffectManager.BulletDisappearFxIndex, transform.position);
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
        SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().BulletManager.Remove(this);
    }
    [ClientRpc]
    public void RpcSetActive(bool value)
    {
        this.gameObject.SetActive(value);
        base.SetDirtyBit(1);
    }
    public void SetPosition(Vector3 position)
    {
        if (isServer)
        {
            RpcSetPosition(position);
        }
        else
        {
            CmdSetPosition(position);
            if(isLocalPlayer)
                transform.position = position;
        }
    }
    [Command]
    public void CmdSetPosition(Vector3 position)
    {
        this.transform.position = position;
        base.SetDirtyBit(1);
    }
    [ClientRpc]
    public void RpcSetPosition(Vector3 position)
    {
        this.transform.position = position;
        base.SetDirtyBit(1);
    }
    public void UpdateNetworkBullet()
    {
        if(isServer)
        {
            RpcUpdateNetworkBullet();
        }
        else
        {
            CmdUpdateNetworkBullet();
        }
    }
    [Command]
    public void CmdUpdateNetworkBullet()
    {
        base.SetDirtyBit(1);
    }

    [ClientRpc]
    public void RpcUpdateNetworkBullet()
    {
        base.SetDirtyBit(1);
    }
}
