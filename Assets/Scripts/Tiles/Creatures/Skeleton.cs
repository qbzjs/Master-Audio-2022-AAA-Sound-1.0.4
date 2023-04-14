using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Scripts;
using UnityEngine.SocialPlatforms.Impl;

public class Skeleton : Creature, IEffectOnDestroyed
{
    [SerializeField] protected int scoreWorth = 2;
    
    private static List<Vector2Int> toHaunt = new ();

    public override Tag[] GetTags()
    {
        return new[] {Tag.Death};
    }

    public override string GetDescription()
    {
        return "If <b>Destroyed</b>, <b>Spawns</b> a new <b><link=\"card\">Skeleton</link></b>.";
    }
    
    
    public Skeleton(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }

    public Action GetInvokeAfterDestroyed()
    {
        Vector2Int spot = new Vector2Int(xPos, yPos);
        return () =>
        {
            TweenManager.Instance.Callout("Reassembled!", spot);
            GameManager.Instance.PlaceTile("Skeleton", spot);
        };
    }
}