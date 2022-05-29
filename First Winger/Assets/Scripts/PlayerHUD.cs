using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField]
    Gage HPGage;
    [SerializeField]
    Player OwnerPlayer;

    Transform OwnerTransform;
    Transform SelfTransform;

    void Start()
    {
        SelfTransform = transform;    
    }
    public void Intialize(Player player)
    {
        OwnerPlayer = player;
        OwnerTransform = OwnerPlayer.transform;
    }

    void Update()
    {
        UpdatePosition();
        UpdateHP();
    }
    void UpdatePosition()
    {
        if(OwnerPlayer != null)
        {
            SelfTransform.position = Camera.main.WorldToScreenPoint(OwnerTransform.position);
        }
    }
    void UpdateHP()
    {
        if(OwnerPlayer != null)
        {
            if(!OwnerPlayer.gameObject.activeSelf)
                gameObject.SetActive(OwnerPlayer.gameObject.activeSelf);

            HPGage.SetHP(OwnerPlayer.HPCurrent, OwnerPlayer.HPMax);
        }
    }
}
