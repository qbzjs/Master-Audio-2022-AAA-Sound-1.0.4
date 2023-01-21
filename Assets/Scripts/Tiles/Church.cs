﻿using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Church : Wasteland
{
    [SerializeField] protected int scoreWorthAdjacent = 3;

    public Church(Transform parentTransform, Vector3 pos)
    {
        ConstructorHelper(parentTransform, pos, "Church");
    }

    protected override Score CalculateBaseScore()
    {
        int adjacentWings = 0;

        if (GridManager.Instance.GetTile(xPos + 1, yPos).Type() == "ChurchWing")
        {
            adjacentWings++;
        }

        if (GridManager.Instance.GetTile(xPos, yPos - 1).Type() == "ChurchWing")
        {
            adjacentWings++;
        }

        if (GridManager.Instance.GetTile(xPos - 1, yPos).Type() == "ChurchWing")
        {
            adjacentWings++;
        }

        if (GridManager.Instance.GetTile(xPos, yPos + 1).Type() == "ChurchWing")
        {
            adjacentWings++;
        }

        
        return new Score(adjacentWings * scoreWorthAdjacent, 
            $"[{adjacentWings}] * {scoreWorthAdjacent}");
    }

    public override string Type()
    {
        return "Church";
    }
}
