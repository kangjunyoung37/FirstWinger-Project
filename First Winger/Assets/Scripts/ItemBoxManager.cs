using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoxManager : MonoBehaviour
{
    [SerializeField]
    PrefabCacheData[] ItemBoxFiles;

    void Start()
    {
        
    }
    Dictionary<string, GameObject> FileCache = new Dictionary<string, GameObject>();
   
    public GameObject Generate(int index, Vector3 position)
    {
        if (!((FWNetworkManager)FWNetworkManager.singleton).isServer)
        {
            return null;
        }
        if (index < 0 || index >= ItemBoxFiles.Length)
        {
            Debug.LogError("Generate error!" + index);
            return null;
        }
        string filePath = ItemBoxFiles[index].filePath;
        GameObject go = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().ItemBoxCacheSystem.Archive(filePath);
        ItemBox item = go.GetComponent<ItemBox>();
        item.RpcSetPosition(position);
        item.FilePath = filePath;

        return go;
    }
    public GameObject Load(string resourcePath)
    {

        GameObject go = null;
        if(FileCache.ContainsKey(resourcePath))
        {
            go = FileCache[resourcePath];
        }
        else
        {
            go = Resources.Load<GameObject>(resourcePath);
            if (!go)
            {
                Debug.LogError("Load Error" + resourcePath);
            }
            FileCache.Add(resourcePath, go);
        }
        return go;
    }

    public void Prepare()
    {
        if (!((FWNetworkManager)FWNetworkManager.singleton).isServer)
        {
            return;
        }
        for (int i = 0; i < ItemBoxFiles.Length; i++)
        {
            GameObject go = Load(ItemBoxFiles[i].filePath);
            SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().ItemBoxCacheSystem.GenerateCache(ItemBoxFiles[i].filePath, go, ItemBoxFiles[i].cacheCount);
        }
    }
    public bool Remove(ItemBox item)
    {
        if (!((FWNetworkManager)FWNetworkManager.singleton).isServer)
        {
            return true;
        }
        SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().ItemBoxCacheSystem.Restore(item.FilePath, item.gameObject);
        return true;
    }
}
