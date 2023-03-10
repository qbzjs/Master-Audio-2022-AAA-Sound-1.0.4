using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;
using System.Threading;
using UnityEngine.SocialPlatforms.Impl;
using NUnit.Framework.Internal;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;

public class Witch : Monster
{
    [SerializeField] protected int scoreWorth = 2;

    public override string GetDescription()
    {
        return "<i>Worth 2 points, in pack of 5 increases score of best tile on board.</i>";
    }

    public Witch(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        Type = "Chaos";
    }

    public override Tag[] GetTags()
    {
        return new[] { Tag.Chaos, Tag.Monster };
    }


    private static Effect WitchMultiplier =
        new Effect(
            "Witch multiplier", 20, 1, 5, (value) =>
            {
                return new Score(value.score + 10, value.explanation + " + 10");
            }
        );

    private static Rule PackOfWitches = new Rule("Pack Of Witches", 9, () =>
    {
        ITile bestTile = null;
        GridManager.ForEach((int x, int y, ITile tile) => {
            if (bestTile == null)
            {
                bestTile = tile;
            }
            else
            {
                if (tile.CalculateScore().score > bestTile.CalculateScore().score)
                {
                    bestTile = tile;
                }
            }
        });
        bestTile.AddEffect(WitchMultiplier);
    });

    public override void WhenPlaced()
    {
        CalculatePack();
    }

    private void CalculatePack()
    {
        List<Creature> pack = new();
        pack.Add(this);
        int packCount = CountGroupCreatures(this.GetType(), pack);

        if (packCount >= packSize)
        {
            GameManager.Instance.AddRule(PackOfWitches);
        }
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }
}
