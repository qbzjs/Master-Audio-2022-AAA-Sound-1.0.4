using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITile
{
    public GameObject TileObject { get; set; }
    
    public Vector3 LocalPosition();

    public Vector3 Position();

    public bool Destructible();

    //nextToGraveYard
    //graves - count how many times been destroyed since next to graveyard

    public int xPos { get; set; }
    public int yPos { get; set; }
    public string Type();
    public int CalculateScore();
}
