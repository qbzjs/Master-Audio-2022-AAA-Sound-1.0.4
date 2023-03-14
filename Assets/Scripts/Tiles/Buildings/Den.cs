using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Den : Building
{
    private bool open = true;
    private int scoreworth = 0;

    public override string GetDescription()
    {
        return "Starts open, will close once touches a pack of Werewolfs, grants 1 points per werewolf. Can catch fire.";
    }

    public override Tag[] GetTags()
    {
        return new[] { Tag.Darkness, Tag.Building};
    }

    public Den(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        GameManager.Instance.AddRule(BurnDen);
    }

    private Rule BurnDen = new Rule("Den checks for #fire", 80, () =>
    {
        GridManager.ForEach((int x, int y, Den den) =>
        {
            foreach (var dir in Directions.Cardinal)
            {
                if (GridManager.Instance.GetTile(x + dir.x, y + dir.y).GetTags().Contains(Tag.Fire))
                {
                    GridManager.Instance.PlaceTile("HellFire", new Vector2Int(x, y));
                }
            }
        });
    });

    protected override Score CalculateBaseScore()
    {
        if (open)
        {
            List<Creature> adjacentWolfPacks = new();
            foreach (Vector2Int dir in Directions.Cardinal)
            {
                ITile tile = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y);
                if (tile is Werewolf werewolf)
                {
                    List<Creature> pack = new();
                    pack.Add(werewolf);
                    werewolf.CountGroupCreatures(werewolf.GetType(), pack);
                    adjacentWolfPacks.AddRange(pack);
                }
            }

            if (adjacentWolfPacks.Count > 0)
            {
                open = false;
                scoreworth = adjacentWolfPacks.Count;
            }

        }

        return new Score(scoreworth);
    }
}
