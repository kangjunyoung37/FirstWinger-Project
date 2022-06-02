using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SquadronManager : MonoBehaviour
{

    float GameStartTime;

    int SquadronIndex;

    [SerializeField]
    SquadronTable[] squadronDatas;

    [SerializeField]
    SquadronScheduleTable squadronScheduleTable;


    bool running = false;
    bool AllSquadronGenerated = false;
    bool ShowWarningUICalled = false;

    void Start()
    {
        squadronDatas = GetComponentsInChildren<SquadronTable>();
        for(int i = 0; i < squadronDatas.Length; i++)
        {
            squadronDatas[i].Load();

           
        }
        squadronScheduleTable.Load();
    }

 
    void Update()
    {
        if(!AllSquadronGenerated)
            CheckSquadronGenerateings();
        else if(!ShowWarningUICalled)
        {
            InGameSceneMain inGameSceneMain = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>();
            if (SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().EnemyManager.GetEnemyListCount() == 0)
            {
                inGameSceneMain.ShowWaringUI();
                ShowWarningUICalled = true;
            }
        }
      




    }
    public void StartGame()
    {
        GameStartTime = Time.time;
        SquadronIndex = 0;
        running = true;
        Debug.Log("Start");

    }
    void CheckSquadronGenerateings()
    {
        if (!running)
        {
            return;
        }
        SquadronScheduleDataStruct data = squadronScheduleTable.GetSquadronScheduleDataStruct(SquadronIndex);
        if(Time.time - GameStartTime >= data.GenerateTime)
        {
            
            GenerateSquadron(squadronDatas[data.SquadronID]);
            SquadronIndex++;
            
            if (SquadronIndex >= squadronScheduleTable.GetDataCount())
            {
                OnAllSquadronGenerated();
                return;
            }
        }
    }
    void GenerateSquadron(SquadronTable table)
    {
        Debug.Log("GenerateSquadron");
        for(int i = 0; i < table.GetCount(); i++)
        {
            SquadronStruct squardronMember = table.GetSquadron(i);
            SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().EnemyManager.GenerateEnemy(squardronMember);
        }
    }

    void OnAllSquadronGenerated()
    {
        Debug.Log("AllSqadronGenerateed");
        running = false;
        AllSquadronGenerated = true;
    }

}
