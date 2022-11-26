using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wasteland : ITile
{
    public GameObject TileObject { get; set; }
    public int xPos { get; set; }
    public int yPos { get; set; }
    [SerializeField] private int scoreWorth = 0;

    public Wasteland()
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
        return 'W';
    }
}
