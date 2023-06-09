﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;
using System.Threading;
using UnityEngine.SocialPlatforms.Impl;

public class Werewolf : Monster
{
    private int monstersKilled = 0;

    public override string GetDescription()
    {
        return "<b>Absorbs</b> <b>Adjacent</b> #Monster. <b>Pack</b> of 5 worth <b><color=\"red\">double</color></b>.";
    }

    public Werewolf(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        Type = "Dark";
    }

    public override Tag[] GetTags()
    {
        return new[] { Tag.Darkness, Tag.Monster };
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
        List<Creature> adjacentMonsterPacks = new();
        foreach (Vector2Int dir in Directions.Cardinal)
        {
            ITile tile = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y);
            if (tile is Monster monster && !(tile is Werewolf))
            {
                List<Creature> pack = new();
                pack.Add(monster);
                monster.CountGroupCreatures(monster.GetType(), pack);
                adjacentMonsterPacks.AddRange(pack);
            }
        }

        foreach(Monster monster in adjacentMonsterPacks)
        {
            monstersKilled += monster.CalculateScore().score;
        }
        
        foreach(Monster monster in adjacentMonsterPacks)
        {
            GridManager.Instance.KillTile(new Vector2Int(monster.xPos, monster.yPos));
        }

        return new Score(monstersKilled);
    }
}
