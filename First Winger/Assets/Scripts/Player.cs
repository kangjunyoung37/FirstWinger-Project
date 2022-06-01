using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : Actor
{
    const string PlayerHUDPath = "Prefabs/PlayerHUD";
    [SerializeField]
    [SyncVar]
    Vector3 MoveVector = Vector3.zero;
   
    [SerializeField]
    NetworkIdentity NetworkIdentity = null;

    [SerializeField]
    float Speed;

    [SerializeField]
    BoxCollider boxCollider;

    [SerializeField]
    Transform FireTransform;



    [SerializeField]
    float BulletSpeed = 1;

    InputController controller = new InputController();

    [SerializeField]
    [SyncVar]
    bool Host = false;
    [SerializeField]
    [SyncVar]
    int UsableItemCount = 0;

    public int ItemCount
    {
        get { return UsableItemCount; }
    }

    [SerializeField]
    Material ClientPlayerMaterial;
    protected override void Initialize()
    {
        base.Initialize();
        InGameSceneMain inGameSceneMain = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>();
        if (isLocalPlayer)
            inGameSceneMain.Hero = this;
       if(isServer && isLocalPlayer)
        {
            Host = true;
            RpcSetHost();
        }
        
        Transform startTrasnform;
        if (Host)
        {
            startTrasnform = inGameSceneMain.PlayerStartTransform1;
        }
        else
        {
            startTrasnform = inGameSceneMain.PlayerStartTrasform2;
            MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();
            meshRenderer.material = ClientPlayerMaterial;
        }
        SetPosition(startTrasnform.position);
        if(actorInstanceID != 0)
        {
            inGameSceneMain.ActorManager.Regist(ActorInstanceID, this);
        }
        InitaializePlayerHUD();
    }
   
    void InitaializePlayerHUD()
    {
        InGameSceneMain inGameSceneMain = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>();
        GameObject go = Resources.Load<GameObject>(PlayerHUDPath);
        GameObject goInstance = Instantiate<GameObject>(go, inGameSceneMain.DamageManager.CanvasTransform);
        PlayerHUD playerHUD = goInstance.GetComponent<PlayerHUD>();
        playerHUD.Intialize(this);
    }
    protected override void UpdateActor()
    {
        if (!isLocalPlayer)//로컬 플레이어가 아니면
            return;
        UpdateInput();
        UpdateMove();
    }

    [ClientCallback]//자신의 클라이언트에서만 실행함
    public void UpdateInput()
    {
        controller.UpdateInput();
    }
    void UpdateMove()
    {
        if (MoveVector.sqrMagnitude == 0)
            return;

        if(isServer)
        {
            RpcMove(MoveVector); //Host플레이어일 경우  RPC로 보냄
        }
        else
        {
            CmdMove(MoveVector);//Client일 Cmd로 보냄
            if (isLocalPlayer)
            {
                transform.position += AdjustMoveVector(MoveVector);
            }
        }

    }
    [ClientRpc]
    public void RpcMove(Vector3 moveVector)
    {
        this.MoveVector = moveVector;
        transform.position += AdjustMoveVector(this.MoveVector);
        base.SetDirtyBit(1);
        this.MoveVector = Vector3.zero; //다른 플레이어가 보내면 Update를 통해 초기화가 안되므로 바로 초기화를 시켜줌
    }
    [Command]
    public void CmdMove(Vector3 moveVector)
    {
        this.MoveVector = moveVector;
        transform.position += AdjustMoveVector(this.MoveVector);
        base.SetDirtyBit(1);//서버가 알 수 있도록 통보
        this.MoveVector = Vector3.zero;//다른 플레이어가 보내면 Update를 통해 초기화가 안되므로 바로 초기화를 시켜줌

    }
    public void ProcessInput(Vector3 moveDirection)//
    {
        MoveVector = moveDirection * Speed * Time.deltaTime;

    }
    Vector3 AdjustMoveVector(Vector3 moveVector)
    {
        Transform mainBGQuadTransform = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().MainBGQuadTransform;
        Vector3 result = Vector3.zero;

        result = boxCollider.transform.position + boxCollider.center + moveVector;
        if (result.x - boxCollider.size.x * 0.5f < -mainBGQuadTransform.localScale.x * 0.5f)
            moveVector.x = 0;
        if (result.x + boxCollider.size.x * 0.5f > mainBGQuadTransform.localScale.x * 0.5f)
            moveVector.x = 0;
        if (result.y - boxCollider.size.y * 0.5f < -mainBGQuadTransform.localScale.y * 0.5f)
            moveVector.y = 0;
        if (result.y + boxCollider.size.y * 0.5f > mainBGQuadTransform.localScale.y * 0.5f)
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
                enemy.OnCrash(CrashDamage, crashPos);
            }
        }
           
    }
    public void Fire()
    {     

        if (IsDead)
            return;
        if (Host)
        {
            Bullet bullet = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().BulletManager.Generate(BulletManager.PlayerBulletIndex);
            bullet.Fire(actorInstanceID, FireTransform.position, FireTransform.right, BulletSpeed, Damage);
        }
        else
        {
            CmdFire(actorInstanceID, FireTransform.position, FireTransform.right, BulletSpeed, Damage);
        }
    }
    public void FireBomb()
    {
        if (UsableItemCount <= 0)
            return;
        if(Host)
        {
            Bullet bullet = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().BulletManager.Generate(BulletManager.PlayerBombIndex);
            bullet.Fire(actorInstanceID, FireTransform.position, FireTransform.right, BulletSpeed, Damage);

        }
        else
        {
            CmdFireBomb(actorInstanceID, FireTransform.position, FireTransform.right, BulletSpeed, Damage);
        }
        DecreaseUsableItemCount();
    }
    void DecreaseUsableItemCount()
    {
        if(isServer)
        {
            RpcDecreaseUsableItemCount();
        }
        else
        {
            CmdDecreaseUsableItemCount();
            if (isLocalPlayer)
                UsableItemCount--;
        }
    }
    [Command]
    void CmdDecreaseUsableItemCount()
    {
        UsableItemCount--;
        base.SetDirtyBit(1);
    }

    void RpcDecreaseUsableItemCount()
    {
        UsableItemCount--;
        base.SetDirtyBit(1);
    }
    [Command]
    public void CmdFireBomb(int ownerIntanceId, Vector3 firePosition, Vector3 direction, float speed, int damage)

    {
        Bullet bullet = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().BulletManager.Generate(BulletManager.PlayerBombIndex);
        bullet.Fire(ownerIntanceId, firePosition, direction, speed, damage);
        base.SetDirtyBit(1);
    }
    [Command]
    public void CmdFire(int ownerIntanceId, Vector3 firePosition, Vector3 direction, float speed, int damage)
    {
        Bullet bullet = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().BulletManager.Generate(BulletManager.PlayerBulletIndex);
        bullet.Fire(ownerIntanceId, firePosition, direction, speed, damage);
        base.SetDirtyBit(1);
    }
    protected override void OnDead()
    {
        base.OnDead();
        gameObject.SetActive(false);
    }

    protected override void DecreaseHP(int value, Vector3 damagePos)
    {
        base.DecreaseHP(value, damagePos);
        Vector3 damagePoint = damagePos + Random.insideUnitSphere * 0.5f;
        SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().DamageManager.Generate(DamageManager.PlayerDamageIndex, damagePoint * 0.5f, value,Color.red);
    }
    [ClientRpc]
    public void RpcSetHost()
    {
        Host = true;
        base.SetDirtyBit(1);
    }
    protected virtual void InternalIncreaseHP(int value)
    {
        if(isDead)
        {
            return;
        }
        CurrentHP += value;
        if(CurrentHP > MaxHP)
            CurrentHP = MaxHP;

    }
    public virtual void IncreaseHP(int value)
    {
        if(isDead)
        { 
            return;
        }
        CmdIncreaseHP(value);
    }
    [Command]
    public void CmdIncreaseHP(int value)
    {
        InternalIncreaseHP(value);
        base.SetDirtyBit(1);
    }
    public virtual void IncreaseUsableItem(int value = 1)
    {
        if(isDead)
        {
            return;
        }
        CmdIncreaseUsableItem(value);
    }
    [Command]
    public void CmdIncreaseUsableItem(int value)
    {
        UsableItemCount += value;
        base.SetDirtyBit(1);
    }
}
