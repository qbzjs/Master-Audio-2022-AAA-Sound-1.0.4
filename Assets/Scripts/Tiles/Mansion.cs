﻿using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Mansion : Wasteland
{
    [SerializeField] new protected int scoreWorth = 6;
    [SerializeField] protected int scoreWorthAdjacent = 0;
    
    public Mansion(Transform parentTransform, Vector3 pos)
    {
        ConstructorHelper(parentTransform, pos, "Mansion");
    }

    protected override Score CalculateBaseScore()
    {
        int adjacentTenements = 0;
        foreach (Vector2Int dir in Directions.Compass)
        {
            string type = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y).Type();
            if (type == "Mansion" || type == "Tenement")
            {
                adjacentTenements++;
            }
        }

        if (adjacentTenements == 0)
        {
            return new Score(scoreWorth);
        }

        return new Score(scoreWorthAdjacent);;
    }

    public override string Type()
    {
        return "Mansion";
    }
}
