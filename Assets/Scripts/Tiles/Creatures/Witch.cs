using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;
using System.Threading;
using UnityEngine.SocialPlatforms.Impl;

public class Witch : Monster
{
    [SerializeField] protected int scoreWorth = 2;
    private static int spellsCast = 0;

    public override string GetDescription()
    {
        return $"{scoreWorth} pts, in pack of 5 increases score of best tile on board.";
    }

    public Witch(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        Type = "Chaos";
        GameManager.Instance.AddRule(PackOfWitches);
    }

    public override Tag[] GetTags()
    {
        return new[] { Tag.Chaos, Tag.Monster };
    }


    private static Effect WitchMultiplier =
        new Effect(
            "Witch multiplier", 20, 1, 10, (value) =>
            {
                return new Score(value.score + 10, value.explanation + " + 10");
            }
        );

    private static Rule PackOfWitches = new Rule("Pack Of Witches", 9, () =>
    {
        List<List<Witch>> witchPacks = new();
        GridManager.ForEach((int x, int y, Witch witch) => {
            bool newWitch = true;
            foreach (List<Witch> pack in witchPacks)
            {
                if (pack.Contains(witch))
                {
                    newWitch = false;
                }
            }
            if (newWitch)
            {
                List<Witch> newPack = new();
                newPack.Add(witch);
                CountGroupedWitches(witch, newPack);

                witchPacks.Add(newPack);
            }
        });

        int numSpells = 0;
        foreach (List<Witch> pack in witchPacks)
        {
            if (pack.Count >= 5)
            {
                numSpells++;
            }
        }

        if (numSpells > spellsCast)
        {
            int toCast = numSpells - spellsCast;
            spellsCast = numSpells;
            for (int i = 0; i < toCast; i++)
            {
                castSpell();
            }
        } else
        {
            spellsCast = numSpells;
        }
    });

    private static void castSpell()
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
    }

    public static int CountGroupedWitches(Witch witch, List<Witch> pack)
    {
        foreach (Vector2Int dir in Directions.Cardinal)
        {
            ITile tile = GridManager.Instance.GetTile(witch.xPos + dir.x, witch.yPos + dir.y);
            if (tile.GetType() == typeof(Witch) && !pack.Contains((Witch)tile))
            {
                pack.Add((Witch)tile);
                CountGroupedWitches((Witch)tile, pack);
            }
        }

        return pack.Count;
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }
}
