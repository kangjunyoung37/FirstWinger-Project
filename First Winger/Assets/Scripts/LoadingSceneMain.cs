using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoadingSceneMain : BaseSceneMain
{
    const float NextSceneIntaval = 1f;
    const float TextUpdateIntaval = 0.15f;
    const string LoadingTextValue = "Loading...";

    [SerializeField]
    Text LoadingText;

    int TextIndex = 0;
    float LastUpdateTime;
    float ScreenStartTime;
    bool NextSceneCall = false;

    protected override void OnStart()
    {
        ScreenStartTime = Time.time;
    }
    protected override void UpdateScene()
    {
        base.UpdateScene();

        float currentTime = Time.time;
        if(currentTime - LastUpdateTime > TextUpdateIntaval)
        {
            LoadingText.text = LoadingTextValue.Substring(0, TextIndex + 1);

            TextIndex++;
            if(TextIndex >= LoadingTextValue.Length)
            {
                TextIndex = 0;
            }

            LastUpdateTime = currentTime;
        }
        if(currentTime - ScreenStartTime > NextSceneIntaval)
        {
            if(!NextSceneCall)
            {
                GotoNextScene();
            }
        }
    }
    void GotoNextScene()
    {

        NetworkConnectionifo info = SystemManager.Instance.Connectioninfo;
        if (info.host)//호스트로 시작
        {

            FWNetworkManager.singleton.StartHost();
        }

        else//클라이언트로 시작
        {


            if (!string.IsNullOrEmpty(info.IPAdress))
            {
                FWNetworkManager.singleton.networkAddress = info.IPAdress;
            }

            if(info.port != FWNetworkManager.singleton.networkPort)
            {
                FWNetworkManager.singleton.networkPort = info.port;
            }

            FWNetworkManager.singleton.StartClient();
        }
        //SceneController.Instance.LoadScene(SceneNameConstants.InGame);
        //호스트로 동작을 함
        NextSceneCall = true;
    }
}
