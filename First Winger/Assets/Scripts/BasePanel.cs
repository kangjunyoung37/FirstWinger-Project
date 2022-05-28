using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{

    public void Awake()
    {
        InitailizePanel();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePanel();
    }

    private void OnDestroy()
    { 
        DestroyPanel();
    }

   //private void OnGUI()
   // {
   //     if(GUILayout.Button("Close"))
   //     {
   //        Close();
   //     }
   //}
    public virtual void InitailizePanel()
    {
        PanelManager.RegistPanel(GetType(), this);
    }
    public virtual void DestroyPanel()
    {
        PanelManager.UnRegisPanel(GetType());  
    }
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
    public virtual void UpdatePanel()
    {

    }
}
