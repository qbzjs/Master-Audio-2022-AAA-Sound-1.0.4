﻿using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Homestead : Building
{
    [SerializeField] protected int scoreWorthAdjacent = 2;

    public override string GetDescription()
    {
        return "<color=\"red\"><b>+2</b></color> for each <b>Adjacent</b> <b>Homestead</b>";
    }

    public Homestead(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }

    public override Tag[] GetTags()
    {
        return new[] { Tag.Building };
    }

    protected override Score CalculateBaseScore()
    {
        int adjacentHomes = 0;

        foreach (Vector2Int dir in Directions.Cardinal)
        {
            ITile tile = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y);
            if (tile is Homestead)
            {
                adjacentHomes++;
            }
        }

        return new Score(1 + adjacentHomes * scoreWorthAdjacent,
            $"[{adjacentHomes}] * {scoreWorthAdjacent}");
    }
}
