using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class SquadronData
{
    public float SquadronGenerateTime;
    public Squadron squadron;
}
public class SquadronManager : MonoBehaviour
{

    float GameStartTime;

    int SquadronIndex;

    [SerializeField]
    SquadronData[] squadronDatas;

    bool running = false;

    void Start()
    {
        
    }

 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartGame();
        }
        CheckSquadronGenerateings();
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
        if(Time.time - GameStartTime >= squadronDatas[SquadronIndex].SquadronGenerateTime)
        {
            GenerateSquadron(squadronDatas[SquadronIndex]);
            SquadronIndex += 1;
            if(SquadronIndex >= squadronDatas.Length)
            {
                AllSquadronGenerated();
                return;
            }
        }
    }
    void GenerateSquadron(SquadronData data)
    {
        Debug.Log("GenerateSquadron");
        data.squadron.GenerateAllData();
    }

    void AllSquadronGenerated()
    {
        Debug.Log("AllSqadronGenerateed");
        running = false;
    }

}
