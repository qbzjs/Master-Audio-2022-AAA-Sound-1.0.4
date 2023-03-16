using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts;
//using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Chamber : Building
{
    public override string GetDescription()
    {
        return "2 points, worth 4 if next to a monster";
    }

    public Chamber(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }

    public override Tag[] GetTags()
    {
        return new[] { Tag.Building };
    }

    protected override Score CalculateBaseScore()
    {
        foreach (Vector2Int dir in Directions.Cardinal)
        {
            ITile tile = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y);
            if (tile.GetTags().Contains(Tag.Monster))
            {
                return new Score(4);
            }
        }

        return new Score(2);
    }
}
