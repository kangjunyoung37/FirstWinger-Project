using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
[System.Serializable]
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct ItemDropStruct
{
    public int index;
    public int ItemID1;
    public int Rate1;
    public int ItemID2;
    public int Rate2;
    public int ItemID3;
    public int Rate3;
}
public class ItemDropTable : TableLoader<ItemDropStruct>
{
    Dictionary<int, ItemDropStruct> tableDatas = new Dictionary<int, ItemDropStruct>();
    void Start()
    {
        Load();
    }

 
    void Update()
    {
        
    }
    protected override void AddData(ItemDropStruct data)
    {
        tableDatas.Add(data.index, data);
    }
    public ItemDropStruct GetDropData(int index)
    {
        if (!tableDatas.ContainsKey(index))
        {
            Debug.LogError("GetItem Error" + index);
            return default(ItemDropStruct);
        }
        return tableDatas[index];
    }
}
