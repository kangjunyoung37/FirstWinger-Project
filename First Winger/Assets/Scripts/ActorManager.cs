using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager
{
    Dictionary<int,Actor> Actors = new Dictionary<int, Actor>();

    public bool Regist(int ActorInstanceID, Actor actor)
    {
        if(ActorInstanceID == 0)
        {
            Debug.LogError("Regist Error! ActtorInstanceId is not set!"+ActorInstanceID);
            return false;
        }

        if(Actors.ContainsKey(ActorInstanceID))
        {
            if(actor.GetInstanceID() != Actors[ActorInstanceID].GetInstanceID())
            {
                Debug.LogError("Regist Error already exist" + ActorInstanceID);
                return false;
            }
            Debug.Log(ActorInstanceID + "is already registed");
            return true;
        }
        Actors.Add(ActorInstanceID, actor);
       
        return true;
    }
    public Actor GetActor(int ActorIntaceID)
    {
        if(!Actors.ContainsKey(ActorIntaceID))
        {
            Debug.Log("키값이 존재 안함" + ActorIntaceID);

        }
        return Actors[ActorIntaceID];
    }
}
