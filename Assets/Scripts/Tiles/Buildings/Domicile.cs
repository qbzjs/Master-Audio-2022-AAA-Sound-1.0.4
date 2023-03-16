using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Domicile : Building
{

    public override string GetDescription()
    {
        return "2 points, if adjacent to other domicile +2";
    }

    public Domicile(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }

    public override Tag[] GetTags()
    {
        return new[] { Tag.Building, Tag.Darkness };
    }

    protected override Score CalculateBaseScore()
    {

        foreach (Vector2Int dir in Directions.Cardinal)
        {
            ITile tile = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y);
            if (tile is Domicile)
            {
                return new Score(4);
            }
        }

        return new Score(2);
    }
}
