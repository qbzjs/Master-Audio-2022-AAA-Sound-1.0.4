using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Scripts;

public class ObserverManager : Singleton<ObserverManager>
{
    public List<ITile> GraveyardObservers = new();
    public List<ITile> PlacedTileObservers = new();
    

    public void AddObserver(ITile tile)
    {
        if (tile is Graveyard)
        {
            GraveyardObservers.Add((Wasteland)tile);
        }
        
        if (tile is Human)
        {
            GraveyardObservers.Add((Wasteland)tile);
        }

        if (tile is Vampire)
        {
            PlacedTileObservers.Add(tile);
        }

        if (tile is Clocktower)
        {
            GraveyardObservers.Add((Wasteland)tile);
            PlacedTileObservers.Add((Wasteland)tile);
        }
    }

    public void RemoveObserver(ITile tile)
    {
        if (tile is Graveyard)
        {
            GraveyardObservers.Remove((Wasteland)tile);
        }
        
        if (tile is Human)
        {
            GraveyardObservers.Remove((Wasteland)tile);
        }
        
        if (tile is Vampire)
        {
            PlacedTileObservers.Remove(tile);
        }
        
        if (tile is Clocktower)
        {
            GraveyardObservers.Remove((Wasteland)tile);
            PlacedTileObservers.Remove((Wasteland)tile);
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
            case PlacedEvent:
                Observers.AddRange(PlacedTileObservers);
                break;
        }

        foreach (ITile o in Observers)
        {
            o.Observe(e);
        }
    }
}
