using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//읽어온 테이블을 로드 시키는 스크립트.
public class TableLoader<TMarshalStruct> : MonoBehaviour
{
    [SerializeField]
    protected string FilePath;

    TableRecordParser<TMarshalStruct> tableRecordParser = new TableRecordParser<TMarshalStruct> ();

    public bool Load()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(FilePath);
        if(textAsset == null)
        {
            Debug.LogError("Load Failed" + FilePath);
            return false;
        }
        ParseTable(textAsset.text);

        return true;
    }

    void ParseTable(string text)
    {
        StringReader reader = new StringReader(text); //Systme.IO의 StringReader

        string line = null;
        bool fieldRead = false;
        while((line = reader.ReadLine()) != null) // 파일끝까지 읽으면서 파싱
        {
            
            if (!fieldRead)
            {
                fieldRead = true;
                continue;
            }
            TMarshalStruct data = tableRecordParser.ParseRecordLine(line);//처음의 한 줄씩 읽음
            
            AddData(data);
        }
    }

    protected virtual void AddData(TMarshalStruct data)
    {

    }
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
