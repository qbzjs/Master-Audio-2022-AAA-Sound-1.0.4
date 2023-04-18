using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil.Cil;
using Scripts;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Church : Building
{
    [SerializeField] protected int scoreWorth = 2;

    public override Tag[] GetTags()
    {
        return new[] { Tag.Chaos, Tag.Building};
    }

    public override string GetDescription()
    {
        return "<color=\"red\"><b>+1</b></color> for each <b>Adjacent</b> #Chaos";
    }

    public Church(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        
    }

    protected override Score CalculateBaseScore()
    {
        int adjacentChaos = 0;

        foreach (Vector2Int dir in Directions.Cardinal)
        {
            ITile tile = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y);
            if (tile.GetTags().Contains(Tag.Chaos))
            {
                adjacentChaos++;
            }
        }

        return new Score(scoreWorth + adjacentChaos,
            $"2 + [{adjacentChaos}]");
    }
}
