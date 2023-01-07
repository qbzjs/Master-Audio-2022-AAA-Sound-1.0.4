using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Wasteland : ITile
{
    public GameObject TileObject { get; set; }
    public int xPos { get; set; }
    public int yPos { get; set; }
    [SerializeField] protected int scoreWorth = 0;

    public Vector3 LocalPosition()
    {
        return TileObject.transform.localPosition;
    }

    public Vector3 Position()
    {
        return TileObject.transform.position;
    }

    public bool Destructible()
    {
        return true;
    }

    public string Type()
    {
        return "WA";
    }

    public int CalculateScore()
    {
        return scoreWorth;
    }
}
