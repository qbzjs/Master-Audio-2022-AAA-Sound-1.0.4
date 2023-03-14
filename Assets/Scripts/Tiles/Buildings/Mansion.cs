using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Mansion : Building
{
    [SerializeField] new protected int scoreWorth = 6;
    [SerializeField] protected int scoreWorthAdjacent = 0;
    
    public Mansion(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }

    public override Tag[] GetTags()
    {
        return new[] { Tag.Building };
    }

    protected override Score CalculateBaseScore()
    {
        int adjacentTenements = 0;
        foreach (Vector2Int dir in Directions.Compass)
        {
            ITile tile = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y);
            if (tile is Mansion || tile is Tenement)
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
}
