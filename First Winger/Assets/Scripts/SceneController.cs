using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneNameConstants
{
    public static string TitleScene = "TitleScene";
    public static string LoadingScene = "LoadingScene";
    public static string InGame = "InGame";

}
public class SceneController : MonoBehaviour
{
    private static SceneController instance;
    public static SceneController Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = GameObject.Find("Scenecontroller");
                if(go == null)
                {
                    go = new GameObject("SceneController");
                    SceneController controller = go.AddComponent<SceneController>();
                    return controller;
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Can't have two instance of singletone");
            DestroyImmediate(this);
            return;
        }
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        //변화에 따른 다른 이벤트 메소드 맵핑

        SceneManager.activeSceneChanged += OnActiveSceneChanaged;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void Update()
    {
        
    }


    //이전 스크린을 Unload하고 로딩
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName, LoadSceneMode.Single));//scene 하나만 해서 로딩
    }

    //이전 스크린을 언로드 없이 로딩
    public void LoadSceneAdditive(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName, LoadSceneMode.Additive));//scene을 여러개로 해서 로딩
    }

    IEnumerator LoadSceneAsync(string sceneName , LoadSceneMode loadSceneMode)//비동기 로드
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);

        while (!asyncOperation.isDone)
        {
            yield return null;  
        }
        Debug.Log("LozdSceneAsync is complete");
    }

    public void OnActiveSceneChanaged(Scene scene0 , Scene scene1)
    {
        Debug.Log("OnActiveSceneChagged is called! " + scene0.name + scene1.name);


    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        Debug.Log("OnSceneLoaded is called" + scene.name+ loadSceneMode.ToString());
        BaseSceneMain baseSceneMain = GameObject.FindObjectOfType<BaseSceneMain>();
        Debug.Log("OnSceneLoaded! " + baseSceneMain.name);
        SystemManager.Instance.CurrentSceneMain = baseSceneMain;
    }

    public void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("OnSceneUnloaded is called" + scene.name);
    }
    public void LoadSceneImmediate(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
