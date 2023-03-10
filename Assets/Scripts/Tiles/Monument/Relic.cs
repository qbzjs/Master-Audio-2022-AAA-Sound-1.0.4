using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using Scripts;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Relic : Monument
{
    [SerializeField] protected int scoreWorth = 4;

    public Relic(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }
    public override Tag[] GetTags()
    {
        return new[] { Tag.Chaos };
    }


    public override string GetDescription()
    {
        return "A magic relic, +2 points for witch in adjacent witch packs.";
    }

    protected override Score CalculateBaseScore()
    {
        List<Creature> adjacentWitchPacks = new();
        foreach (Vector2Int dir in Directions.Cardinal)
        {
            ITile tile = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y);
            if (tile is Witch witch)
            {
                List<Creature> pack = new();
                pack.Add(witch);
                witch.CountGroupCreatures(witch.GetType(), pack);
                adjacentWitchPacks.AddRange(pack);
            }
        }

        return new Score(scoreWorth + adjacentWitchPacks.Count);
    }
}
