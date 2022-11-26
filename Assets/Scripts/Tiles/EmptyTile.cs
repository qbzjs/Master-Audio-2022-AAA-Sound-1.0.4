using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyTile : ITile
{
    public GameObject TileObject { get; set; }
    public int xPos { get; set; }
    public int yPos { get; set; }
    [SerializeField] private int scoreWorth = 0;

    public EmptyTile()
    {
    }

    public bool Destructible()
    {
        return true;
    }

    public int CalculateScore()
    {
        return scoreWorth;
    }

    public char Type()
    {
        return 'E';
    }
}
