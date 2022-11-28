using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fountain : ITile
{
    public GameObject TileObject { get; set; }
    public int xPos { get; set; }
    public int yPos { get; set; }

    [SerializeField] private int scoreWorth = 0;

    public Fountain()
    {
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
        return false;
    }

    public int CalculateScore()
    {
        return scoreWorth;
    }

    public char Type()
    {
        return 'F';
    }
}
