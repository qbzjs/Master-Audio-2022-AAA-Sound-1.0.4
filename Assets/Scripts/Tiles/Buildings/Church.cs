using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using Scripts;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Church : Building
{
    [SerializeField] protected int scoreWorth = 3;

    public override Tag[] GetTags()
    {
        return new[] { Tag.Chaos, Tag.Building};
    }

    public override string GetDescription()
    {
        return "<i>A satanic church, worth 3 points</i>";
    }

    public Church(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }
}
