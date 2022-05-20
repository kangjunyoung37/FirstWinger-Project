using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    public const string EnemyPath = "Prefabs/Enemy";
    Dictionary<string, GameObject> EnemyFileCashe = new Dictionary<string, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject Load(string resourcePath)
    {
        GameObject go = null;
        if(EnemyFileCashe.ContainsKey(resourcePath))
        {
            go = EnemyFileCashe[resourcePath];
        }
        else
        {
            go = Resources.Load<GameObject>(resourcePath);
            if (!go)
            {
                Debug.LogError("Load error! path" + resourcePath);
                return null;

            }
            EnemyFileCashe.Add(resourcePath, go);
        }
        GameObject instancedGO = Instantiate(go);
        return instancedGO;
    }
}
