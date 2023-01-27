using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Church : Wasteland
{
    [SerializeField] protected int scoreWorthAdjacent = 3;

    public Church(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        
    }

    protected override Score CalculateBaseScore()
    {
        int adjacentWings = 0;

        if (GridManager.Instance.GetTile(xPos + 1, yPos) is Wing)
        {
            adjacentWings++;
        }

        if (GridManager.Instance.GetTile(xPos, yPos - 1) is Wing)
        {
            adjacentWings++;
        }

        if (GridManager.Instance.GetTile(xPos - 1, yPos) is Wing)
        {
            adjacentWings++;
        }

        if (GridManager.Instance.GetTile(xPos, yPos + 1) is Wing)
        {
            adjacentWings++;
        }

        
        return new Score(adjacentWings * scoreWorthAdjacent, 
            $"[{adjacentWings}] * {scoreWorthAdjacent}");
    }
}
