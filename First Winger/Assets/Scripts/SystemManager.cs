using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    static SystemManager intance = null;

    public static SystemManager Instance
    {
        get { return intance; }
    }

    [SerializeField]
    EffectManager effectManager; 

    public EffectManager EffectManager
    {
        get { return effectManager; }
    }

    [SerializeField]
    Player player;

    public Player Hero
    {
        get
        {
            return player;
        }
    }

    GamePointAccumlator gamePointAccumlator = new GamePointAccumlator();

    public GamePointAccumlator GamePointAccumlator
    {
        get { return gamePointAccumlator; }
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
