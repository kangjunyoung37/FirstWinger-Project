using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ItemBox : NetworkBehaviour
{
    [SerializeField]
    Transform selfTransform;
    [SerializeField]
    Vector3 RotateAngle = new Vector3(0.0f, 0.5f, 0.0f);
    [SyncVar]
    [SerializeField]
    string filePath;
    public string FilePath
    {
        get { return filePath; }
        set { filePath = value; }
    }
    [SerializeField]
    Vector3 MoveVector = Vector3.zero;
    void Start()
    {
        if (!((FWNetworkManager)FWNetworkManager.singleton).isServer)
        {
            InGameSceneMain inGameSceneMain = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>();
            transform.SetParent(inGameSceneMain.ItemBoxManager.transform);
            inGameSceneMain.ItemBoxCacheSystem.Add(FilePath, gameObject);
            gameObject.SetActive(false);
        }
    }

    
    void Update()
    {
        UpdateMove();
        UpdateRotate();
    }
    void UpdateRotate()
    {
        Vector3 eulerAnlges = selfTransform.localRotation.eulerAngles;
        eulerAnlges += RotateAngle;
        selfTransform.Rotate(RotateAngle, Space.Self);
    }
    void UpdateMove()
    {
        selfTransform.position += MoveVector * Time.deltaTime;

    }

    [ClientRpc]
    public void RpcSetActive(bool value)
    {
        this.gameObject.SetActive(value);
        base.SetDirtyBit(1);
    }

    [ClientRpc]
    public void RpcSetPosition(Vector3 position)
    {
        this.transform.position = position;
        base.SetDirtyBit(1);
    }
}
