using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wasteland : ITile
{
    public GameObject TileObject { get; set; }
    public int xPos { get; set; }
    public int yPos { get; set; }
    [SerializeField] protected int scoreWorth = 0;

    public Wasteland(int x, int y)
    {
        this.xPos = x;
        this.yPos = y;
    }

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

    //nextToGraveYard
    //graves - count how many times been destroyed since next to graveyard
    public string Type()
    {
        return "WA";
    }

    public int CalculateScore()
    {
        return scoreWorth;
    }
}
