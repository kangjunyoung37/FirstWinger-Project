using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] effectPrefabs;


    void Start()
    {
        
    }

    void Update()
    {
        
        
    }

    public GameObject GenerateEffect(int index, Vector3 position)
    {
        if(index < 0 || index >= effectPrefabs.Length)
        {
            Debug.LogError("GenerateEffect error" + index);
        }

        GameObject go = Instantiate<GameObject>(effectPrefabs[index],position,Quaternion.identity);

        return go;
    }
}
