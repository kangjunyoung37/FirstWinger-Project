using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

[System.Serializable]
[StructLayout(LayoutKind.Sequential,CharSet = CharSet.Ansi)]
public struct ItemStruct
{
    public int index;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MarshalTableConstant.charBufferSize)]
    public string FilePath;
    public int Value;
}
public class ItemTable : TableLoader<ItemStruct>
{
   
    Dictionary<int, ItemStruct> tableDatas = new Dictionary<int, ItemStruct>();

    void Start()
    {
        Load();
    }

  
    void Update()
    {
        
    }
    protected override void AddData(ItemStruct data)
    {
        tableDatas.Add(data.index,data);
    }
    public ItemStruct GetItem(int index)
    {
        if(!tableDatas.ContainsKey(index))
        {
            Debug.LogError("GetItem Error" + index);
            return default(ItemStruct);
        }
        return tableDatas[index];
    }
}
