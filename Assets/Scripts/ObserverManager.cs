using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Scripts;

public class ObserverManager : Singleton<ObserverManager>
{
    public List<ITile> GraveyardObservers = new();

    public void AddObserver(ITile tile)
    {
        if (tile.Type() == "Graveyard")
        {
            GraveyardObservers.Add((Wasteland)tile);
        }
    }

    public void RemoveObserver(ITile tile)
    {
        if (tile.Type() == "Graveyard")
        {
            GraveyardObservers.Remove((Wasteland)tile);
        }
    }

    public void ProcessEvent(DefaultEvent e)
    {
        List<ITile> Observers = new();

        switch (e)
        {
            case GraveyardEvent:
                Observers.AddRange(GraveyardObservers);
                break;

        }

        foreach (ITile o in Observers)
        {
            Debug.Log($"bug at {o}");
            o.Observe(e);
        }
    }
}
