using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using Scripts;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Totem : Monument
{
    [SerializeField] new protected int scoreWorth = 8;
    public override string GetDescription()
    {
        return scoreWorth + " pts";
    }

    public override Tag[] GetTags()
    {
        return new[] { Tag.Darkness };
    }

    public Totem(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }
    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }

}
