using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using Scripts;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Wing : Building
{
    [SerializeField] protected int scoreWorth = 1;

    public override Tag[] GetTags()
    {
        return new[] { Tag.Chaos, Tag.Building };
    }

    public override string GetDescription()
    {
        return "<i>A church wing, worth 1 points and +1 for every adjacnet church and wing</i>";
    }

    public Wing(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }

    protected override Score CalculateBaseScore()
    {
        int adjacent = 0;

        foreach (Vector2Int dir in Directions.Cardinal)
        {
            ITile tile = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y);
            if (tile is Wing || tile is Church)
            {
                adjacent++;
            }
        }

        return new Score(1 + adjacent);
    }
}
