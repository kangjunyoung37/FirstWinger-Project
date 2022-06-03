using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    static SystemManager intance = null;


    [SerializeField]
    NetworkConnectionifo networkConnectionifo = new NetworkConnectionifo();

    public NetworkConnectionifo Connectioninfo
    {
        get { return networkConnectionifo; }
    }
    public static SystemManager Instance
    {
        get { return intance; }
    }

    [SerializeField]
    EnemyTable enemyTable;
    public EnemyTable EnemyTable
    {
        get
        {
           return enemyTable;
        }
    }
    BaseSceneMain currentSceneMain;
    public BaseSceneMain CurrentSceneMain
    {
        set { currentSceneMain = value; }
    }
    [SerializeField]
    ItemTable itemTable;
    public ItemTable ItemTable
    {
        get { return itemTable; }
    }
    [SerializeField]
    ItemDropTable itemDropTable;
    public ItemDropTable ItemDropTable
    {
        get { return itemDropTable; }
    }
    private void Awake()
    {
        if(intance != null)
        {
            Debug.LogError("SystemManager error!");
            Destroy(gameObject);
            return;
        }
        intance = this;
        DontDestroyOnLoad(gameObject); 

    }
   
    void Start()
    {
        BaseSceneMain baseSceneMain = GameObject.FindObjectOfType<BaseSceneMain>();

        SystemManager.Instance.CurrentSceneMain = baseSceneMain;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public T GetCurrentSceneMain<T>() where T : BaseSceneMain
    {
        return currentSceneMain as T;
    }
}
