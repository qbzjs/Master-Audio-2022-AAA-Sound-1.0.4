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
        return "When Placed: +1 for each #Monster in largest pack";
    }

    public override Tag[] GetTags()
    {
        return new[] { Tag.Darkness, Tag.Building};
    }

    public Den(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        //GameManager.Instance.AddRule(BurnDen);
    }

    //private Rule BurnDen = new Rule("Den checks for #fire", 80, () =>
    //{
    //    GridManager.ForEach((int x, int y, Den den) =>
    //    {
    //        foreach (var dir in Directions.Cardinal)
    //        {
    //            if (GridManager.Instance.GetTile(x + dir.x, y + dir.y).GetTags().Contains(Tag.Fire))
    //            {
    //                GridManager.Instance.PlaceTile("HellFire", new Vector2Int(x, y));
    //            }
    //        }
    //    });
    //});

    public override void WhenPlaced()
    {
        base.WhenPlaced();
    }

    protected override Score CalculateBaseScore()
    {
        if (open)
        {
            List<Creature> monsterPacks = new();
            int maxPack = 0;
            GridManager.ForEach((int x, int y, ITile tile) =>
            {
                if (tile is Monster monster)
                {
                    List<Creature> pack = new();
                    pack.Add(monster);
                    maxPack = Mathf.Max(maxPack, monster.CountGroupCreatures(monster.GetType(), pack));
                    monsterPacks.AddRange(pack);
                }
            });
            
            scoreworth = maxPack;
            open = false;
        }

        return new Score(scoreworth);
    }
}
