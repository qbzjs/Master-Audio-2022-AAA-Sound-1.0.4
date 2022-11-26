using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITile
{
    public GameObject TileObject { get; set; }
    public Vector3 LocalPosition()
    {
        return TileObject.transform.localPosition;
    }

    public Vector3 Position()
    {
        return TileObject.transform.position;
    }

    public bool Destructible();

    public int xPos { get; set; }
    public int yPos { get; set; }
    public char Type();
    public int CalculateScore();
}
