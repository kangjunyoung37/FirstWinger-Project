using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class Actor :  NetworkBehaviour
{

    [SerializeField]
    [SyncVar]
    protected int MaxHP = 100;
    public int HPMax
    {
        get { return MaxHP; }
    }

    [SerializeField]
    [SyncVar]
    protected int CurrentHP;
    public int HPCurrent
    {
        get { return CurrentHP; }
    }

    [SerializeField]
    [SyncVar]
    protected int Damage = 1;

    [SerializeField]
    [SyncVar]
    protected bool isDead = false;

    [SerializeField]
    protected int crashDamage = 100;


    protected int CrashDamage
    {
        get { return crashDamage; }
    }
    public bool IsDead
    {
        get { return isDead; }
    }
    [SyncVar]
    protected int actorInstanceID = 0;

    public int ActorInstanceID
    {
        get
        {
            return actorInstanceID;
        }
    }

    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateActor();
    }

    protected virtual void Initialize()
    {
        CurrentHP = MaxHP;
        if(isServer)
        {
            actorInstanceID = GetInstanceID();//게임오브젝트마다 ID가 있는데 그걸 반환함
            RpcSetActorInstanceID(actorInstanceID);

        }
    }

    protected virtual void UpdateActor()
    {

    }
    public virtual void OnBulletHited(int damage,Vector3 hitPos)
    {
        Debug.Log("OnBulletHited" + damage);
        DecreaseHP(damage, hitPos);
    }
    public virtual void OnCrash(int damage, Vector3 hitPos)
    {
        Debug.Log("OnCrash damage" + damage);
        DecreaseHP(damage, hitPos);
    }
    protected virtual void DecreaseHP( int value,Vector3 damagePos)
    {
        if (isDead)
            return;
        if (isServer)
        {
            RpcDecreaseHP(value, damagePos);
        }
     

    }
    protected virtual void InternalDecreaseHP(int value, Vector3 damagePos)
    {
        if (isDead)
            return;
        CurrentHP -= value;
        if(CurrentHP < 0)
            CurrentHP = 0;
        if(CurrentHP == 0)
        {
            OnDead();
        }
    }
    protected virtual void OnDead()
    {
        Debug.Log(name + "OnDead");
        isDead = true;

        SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().EffectManager.GenerateEffect(EffectManager.ActorDeadFxIndex, transform.position);
    }
    public void SetPosition(Vector3 position)
    {
        if (isServer)
        {
            RpcSetPosition(position);//Host인경우 Rpc로 보내고
        }
        else
        {
            CmdSetPosition(position);//Client일경우 Cmd로 보냄
            if (isLocalPlayer)
            {
                transform.position = position;
            }
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
        this.SetDirtyBit(1);
    }
    [ClientRpc]
    public void RpcSetActive(bool value)
    {
        this.gameObject.SetActive(value);
        base.SetDirtyBit(1);
    }
    //public void updatenetworkactor()
    //{
    //    if(isserver)
    //    {
    //        rpcupdatenetworkactor();
    //    }
    //    else
    //    {
    //        cmdupdatenetworkactor();
    //    }
    //}
    [Command]
    public void CmdUpdateNetworkActor()
    {
        base.SetDirtyBit(1);
    }
    [ClientRpc]
    public void RpcUpdateNetworkActor()
    {
        base.SetDirtyBit(1);
    }
    [ClientRpc]
    public void RpcSetActorInstanceID(int instID)
    {
        this.actorInstanceID = instID;
        if(this.actorInstanceID != 0)
        {
            SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().ActorManager.Regist(this.actorInstanceID, this);
           
        }
        base.SetDirtyBit(1);
    }
    [Command]
    public void CmdDecreaseHP(int value, Vector3 damagePos)
    {
        InternalDecreaseHP(value, damagePos);
        base.SetDirtyBit(1);
    }
    [ClientRpc]
    public void RpcDecreaseHP(int value, Vector3 damagePos)
    {
        InternalDecreaseHP(value, damagePos);
        base.SetDirtyBit(1);
    }
}
