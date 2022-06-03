using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{

    static Dictionary<Type, BasePanel> Panels = new Dictionary<Type, BasePanel>();
    
    void Start()
    {
        
    }

   
    void Update()
    {
        
    }

    public static bool RegistPanel(Type PanelClassType, BasePanel basePanel)
    {
        if(Panels.ContainsKey(PanelClassType))
        {
            Debug.LogError("RegistPanel Error! Already exist Type! PanelClassType" + PanelClassType.ToString());
        }

        Panels.Add(PanelClassType, basePanel);
        return true;
    }

    public static bool UnRegisPanel(Type PanelClassType)
    {
        if (!Panels.ContainsKey(PanelClassType))
        {
            Debug.Log("UnRegistPanel Error");
            return false;
        }
        Panels.Remove(PanelClassType);
        return true;
    }
    public static BasePanel GetPanel(Type PanelClassType)
    {
        if(!Panels.ContainsKey(PanelClassType))
        {
            Debug.Log("GetPanel Error Can't Find");
            return null;
        }

        return Panels[PanelClassType]; 
    }
}
