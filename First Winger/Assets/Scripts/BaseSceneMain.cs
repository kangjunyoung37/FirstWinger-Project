using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSceneMain : MonoBehaviour
{


    private void Awake()
    {
        OnAwake();
    }
    void Start()
    {
        OnStart();
    }


    void Update()
    {
        UpdateScene();
    }

    void OnDestroy()
    {
        Terminate();
    }

    protected virtual void OnAwake()
    {

    }

    protected virtual void OnStart()
    {


    }

    protected virtual void Initialize()
    {

    }

    protected virtual void UpdateScene()
    {


    }

    protected virtual void Terminate()
    {



    }
}



