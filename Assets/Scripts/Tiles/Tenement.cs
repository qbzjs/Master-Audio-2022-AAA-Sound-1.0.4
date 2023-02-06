using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Tenement : Wasteland
{
    [SerializeField] protected int scoreWorthAdjacent = 1;

    public override string GetDescription()
    {
        return "+1 for each adjacent tenement";
    }
    
    public Tenement(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }

    protected override Score CalculateBaseScore()
    {
        int adjacentTenements = 0;

        foreach (Vector2Int dir in Directions.Cardinal)
        {
            ITile tile = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y);
            if (tile is Tenement)
            {
                adjacentTenements++;
            }
        }

        return new Score(adjacentTenements * scoreWorthAdjacent, 
            $"[{adjacentTenements}] * {scoreWorthAdjacent}");
    }
}
