using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FWNetworkManager : NetworkManager
{

    public const int WatingCountPlayerCount = 2;
    int PlayerCount = 0;
    public bool isServer
    {
        get;
        private set;
    }
    #region SERVER SIDE EVENT
    public override void OnServerConnect(NetworkConnection conn)
    {
        Debug.Log("OnServerConnect Call" + conn.address + " , " + conn.connectionId);
        base.OnServerConnect(conn);
    }
    public override void OnServerSceneChanged(string sceneName)
    {
        Debug.Log("OnServerSceneChanged:" + sceneName);
        base.OnServerSceneChanged(sceneName);
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        PlayerCount++;
        base.OnServerReady(conn);
        Debug.Log("OnServerReady" + conn.address);
        if(PlayerCount >= WatingCountPlayerCount)
        {
            InGameSceneMain inGameSceneMain = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>();
            inGameSceneMain.GameStart();
        }
       
    }

    public override void OnServerError(NetworkConnection conn, int errorCode)
    {
        Debug.Log("OnserverError" + errorCode);
        base.OnServerError(conn, errorCode);
    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        Debug.Log("OnServerDisconnect" + conn.address);
        base.OnServerDisconnect(conn);
    }
    public override void OnStartServer()//서버가 시작하면
    {
        Debug.Log("OnStartServer");
        base.OnStartServer();
        isServer = true;
    }
    #endregion SERVER SIDE EVENT

    #region CLIENT SIDE EVENT
    public override void OnStartClient(NetworkClient client)
    {
        Debug.Log("OnStartClient" + client.serverIp);
        base.OnStartClient(client);
    }
    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("OnClientConnect" + conn.connectionId + "" + conn.hostId);
        base.OnClientConnect(conn);
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        Debug.Log("OnClientSceneChanged" + conn.hostId);
        base.OnClientSceneChanged(conn);
    }
    public override void OnClientError(NetworkConnection conn, int errorCode)
    {
        Debug.Log("OnClientError" + errorCode);
        base.OnClientError(conn, errorCode);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        if(!isServer)
        {
            InGameSceneMain inGameScene = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>();
            if(inGameScene.CurrentGameState == GameState.End)
            {
                inGameScene.GotoTitleScene();
                return;
            }
        }
        base.OnClientDisconnect(conn);
    }

    public override void OnClientNotReady(NetworkConnection conn)
    {
        Debug.Log("OnClientNotReady" + conn.hostId);
        base.OnClientNotReady(conn);
    }

    public override void OnDropConnection(bool success, string extendedInfo)
    {
        Debug.Log("OnDropConnection" + extendedInfo);
        base.OnDropConnection(success, extendedInfo);
    }
    #endregion CLIENT SIDE EVENT
}
