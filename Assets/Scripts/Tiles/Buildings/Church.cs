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
        return "3. A Satanic Church.";
    }

    public Church(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }
}
