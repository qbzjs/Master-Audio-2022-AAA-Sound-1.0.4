using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts;
using UnityEngine;

public class Cave : Building
{
    [SerializeField] protected int scoreWorthAdjacent = 2;

    public override string GetDescription()
    {
        return $"+{scoreWorthAdjacent} for each adjacent #darkness";
    }

    public override Tag[] GetTags()
    {
        return new[] {Tag.Darkness, Tag.Building};
    }

    public Cave(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }

    protected override Score CalculateBaseScore()
    {
        int adjacentDarkness = 0;
        

        foreach (Vector2Int dir in Directions.Cardinal)
        {
            ITile tile = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y);
            if (tile.GetTags().Contains(Tag.Darkness))
            {
                adjacentDarkness++;
            }
        }

        return new Score(adjacentDarkness * scoreWorthAdjacent, 
            $"[{adjacentDarkness}] * {scoreWorthAdjacent}");
    }
}
