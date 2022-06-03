using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GuideMissile : Bullet
{
    const float ChaseFector = 1.5f;//방향 전환 정도
    const float ChasingStartTime = 0.7f;//타켓 추적 시작시간
    const float ChasingEndTime = 4.5f;//타켓 추적 종료시간


    [SyncVar]
    [SerializeField]
    int TargetInstanceID;

    [SerializeField]
    Vector3 ChaseVector;
    [SerializeField]
    Vector3 rotateVector = Vector3.zero;

    [SerializeField]
    bool FlipDirection = true;

    bool needChase = true;

    public void FireChase(int targetIntanceID, int ownerInstanceID, Vector3 direction, float speed, int damage)
    {
        if (!isServer)
            return;
        RpcSetTargetInstanceID(targetIntanceID);//Host플레이어일 경우
        base.Fire(ownerInstanceID,direction,speed,damage);
    }
    [ClientRpc]
    public void RpcSetTargetInstanceID(int targetIntanceID)
    {
        TargetInstanceID = targetIntanceID;
        base.SetDirtyBit(1);

    }
    protected override void UpdateTransform()
    {

        UpdateMove();
        UpdateRotate();
    }

    protected override void UpdateMove()
    {
        if (!NeedMove)
            return;
        Vector3 moveVector = MoveDirection.normalized * Speed * Time.deltaTime;

        float deltaTime = Time.time - FiredTime;
        if(deltaTime > ChasingStartTime && deltaTime < ChasingEndTime)
        {
            Actor target = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().ActorManager.GetActor(TargetInstanceID);
            if(target != null)
            {
                Vector3 targetVector = target.transform.position - transform.position;//타켓까지의 벡터

                ChaseVector = Vector3.Lerp(moveVector.normalized, targetVector.normalized, ChaseFector * Time.deltaTime);//이동 벡터와 타켓 벡터 사이의 벡터 계산
                moveVector += ChaseVector.normalized;//이동벡터에 추적벡터를 더함
                moveVector = moveVector.normalized * Speed * Time.deltaTime;//스피드에 따른 길이를 다시 계산

                MoveDirection = moveVector.normalized;
            }
        }
        moveVector = AdjustMove(moveVector);
        transform.position += moveVector;

        //moveVector방향으로 회전시키기
        rotateVector.z = Vector2.SignedAngle(Vector2.right, moveVector);
        if (FlipDirection)
            rotateVector.z += 180.0f;


    }
    void UpdateRotate()
    {
        Quaternion quat = Quaternion.identity;
        quat.eulerAngles = rotateVector;
        transform.rotation = quat;
    }
}
