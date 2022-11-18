﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyTile : ITile
{
    public GameObject TileObject { get; set; }
    [SerializeField] private int scoreWorth = 0;

    public EmptyTile()
    {
    }

    public int CalculateScore()
    {
        return scoreWorth;
    }
}
