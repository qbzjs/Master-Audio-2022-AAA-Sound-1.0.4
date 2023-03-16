using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Dwelling : Building
{
    [SerializeField] protected int scoreWorthAdjacent = 1;

    public override string GetDescription()
    {
        return "3 points, +1 for each adjacent dwelling";
    }

    public Dwelling(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }

    public override Tag[] GetTags()
    {
        return new[] { Tag.Building };
    }

    protected override Score CalculateBaseScore()
    {
        int adjacentDwelling = 0;

        foreach (Vector2Int dir in Directions.Cardinal)
        {
            ITile tile = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y);
            if (tile is Dwelling)
            {
                adjacentDwelling++;
            }
        }

        return new Score(3 + adjacentDwelling * scoreWorthAdjacent,
            $"3 + [{adjacentDwelling}] * {scoreWorthAdjacent}");
    }
}
