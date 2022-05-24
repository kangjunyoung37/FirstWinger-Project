using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

[System.Serializable]
[StructLayout(LayoutKind.Sequential,CharSet = CharSet.Ansi)]
public struct SquadronScheduleDataStruct
{
    public int index;
    public float GenerateTime;
    public int SquadronID;
}
public class SquadronScheduleTable :TableLoader<SquadronScheduleDataStruct>
{
    List<SquadronScheduleDataStruct> SquadronScheduledata = new List<SquadronScheduleDataStruct>();



    protected override void AddData(SquadronScheduleDataStruct data)
    {
        SquadronScheduledata.Add(data);
    }


    public SquadronScheduleDataStruct GetSquadronScheduleDataStruct(int index)
    {
        if(index < 0 || index >= SquadronScheduledata.Count)
        {
            Debug.LogError("Squadronschedule Error" + index);
            return default(SquadronScheduleDataStruct);
        }

        return SquadronScheduledata[index]; 
    }
}
