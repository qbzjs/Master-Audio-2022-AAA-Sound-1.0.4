using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;
using TMPro;

public class Tenement : Building
{
    [SerializeField] protected int scoreWorthAdjacent = 1;

    public override string GetDescription()
    {
        return "<color=\"red\"><b>+1</b></color> for each <b>Adjacent</b> <b>Tenement</b>";
    }
    
    public Tenement(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }

    public override Tag[] GetTags()
    {
        return new[] { Tag.Building };
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

        return new Score(1 + adjacentTenements * scoreWorthAdjacent, 
            $"1 + [{adjacentTenements}] * {scoreWorthAdjacent}");
    }
}
