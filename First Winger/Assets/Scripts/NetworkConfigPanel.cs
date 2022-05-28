using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkConfigPanel : BasePanel
{
    const string DefaultIPAdress = "localhost";
    const string DefaultPortAdress = "7777";

    [SerializeField]
    InputField IPAdressInputField;

    [SerializeField]
    InputField PortInputField;

    public override void InitailizePanel()//초기화 오버라이드
    {
        base.InitailizePanel();
        IPAdressInputField.text = DefaultIPAdress;
        PortInputField.text = DefaultPortAdress;
        Close();
    }

    public void OnHostButton()//호스트 버튼을 눌렀을때 
    {
        SystemManager.Instance.Connectioninfo.host = true;
        TitleSceneMain sceneMain = SystemManager.Instance.GetCurrentSceneMain<TitleSceneMain>();
        sceneMain.GotoNextScene();
    }

    public void OnClientButton()//클라이언트 버튼을 눌렀을때

    {
        SystemManager.Instance.Connectioninfo.host = false;
        TitleSceneMain sceneMain = SystemManager.Instance.GetCurrentSceneMain<TitleSceneMain>();

        if(!string.IsNullOrEmpty(IPAdressInputField.text) || IPAdressInputField.text != DefaultIPAdress)
        {
            SystemManager.Instance.Connectioninfo.IPAdress = IPAdressInputField.text;
        }
        if(!string .IsNullOrEmpty(PortInputField.text) || PortInputField.text != DefaultPortAdress)
        {
            int port = 0;
            if(int.TryParse(PortInputField.text, out port))
            {
                SystemManager.Instance.Connectioninfo.port= port;
            }
            else
            {
                Debug.LogError("OnClientButton error port" + PortInputField.text);
                return;
            }
        }
        sceneMain.GotoNextScene();
    }

}
