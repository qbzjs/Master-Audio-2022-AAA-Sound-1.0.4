using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;
using System.Threading;
using UnityEngine.SocialPlatforms.Impl;

public class Werewolf : Monster
{
    private int vampiresKilled = 0;

    public override string GetDescription()
    {
        return "<i>Kills adjacent vampires, in pack of 5 all werewolf scores double</i>";
    }

    public Werewolf(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        Type = "Dark";
    }

    public override Tag[] GetTags()
    {
        return new[] { Tag.Darkness };
    }


    private static Effect WolfMultiplier =
        new Effect(
            "Wolf multiplier", 20, 1, 1, (value) =>
            {
                return new Score(value.score * 2, value.explanation + " * 2");
            }
        );

    private static Rule PackOfWolves = new Rule("Pack Of Wolves", 9, () =>
    {
        GridManager.ForEach((int x, int y, ITile tile) => {
            if (tile is Werewolf werewolf)
            {
                werewolf.AddEffect(WolfMultiplier);
            }
        });

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
            GameManager.Instance.AddRule(PackOfWolves);
        }
    }

    protected override Score CalculateBaseScore()
    {
        List<Creature> adjacentVampirePacks = new();
        foreach (Vector2Int dir in Directions.Cardinal)
        {
            ITile tile = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y);
            if (tile is Vampire vampire)
            {
                List<Creature> pack = new();
                pack.Add(vampire);
                vampire.CountGroupCreatures(vampire.GetType(), pack);
                adjacentVampirePacks.AddRange(pack);
            }
        }

        vampiresKilled += adjacentVampirePacks.Count;

        foreach(Vampire vampire in adjacentVampirePacks)
        {
            GridManager.Instance.KillTile(new Vector2Int(vampire.xPos, vampire.yPos));
        }

        return new Score(vampiresKilled);
    }
}
