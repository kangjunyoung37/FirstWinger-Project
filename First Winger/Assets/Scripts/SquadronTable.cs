using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

[System.Serializable]
[StructLayout(LayoutKind.Sequential,CharSet = CharSet.Ansi)]
public struct SquadronStruct
{
    public int index;
    public int EnemyID;
    public float GeneratePointX;
    public float GeneratePointY;
    public float AppearPointX;
    public float AppearPointY;
    public float DisappearPointX;
    public float DisappearPointY;
   

}
public class SquadronTable : TableLoader<SquadronStruct>
{
    List<SquadronStruct> tableDatas = new List<SquadronStruct> ();

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
    protected override void AddData(SquadronStruct data)
    {
        tableDatas.Add(data);
    }

    public SquadronStruct GetSquadron(int index)
    {
        if(index <0 || index >= tableDatas.Count)
        {
            Debug.LogError("GetSquadron" + index);
            return default(SquadronStruct);//Struct값이 모두 0인걸 반환
        }
        return tableDatas[index]; 
    }
    public int GetCount()
    {
        return tableDatas.Count;
    }


}
