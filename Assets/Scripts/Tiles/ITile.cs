using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITile
{
    public GameObject TileObject { get; set; }
    
    public Vector3 LocalPosition();

    public Vector3 Position();

    public bool Destructible();

    public int xPos { get; set; }
    public int yPos { get; set; }
    public char Type();
    public int CalculateScore();
}
