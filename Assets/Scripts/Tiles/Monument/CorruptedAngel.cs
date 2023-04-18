using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class CorruptedAngel : Monument
{
    [SerializeField] new protected int scoreWorth = 3;

    public override string GetDescription()
    {
        return " ";
    }

    public override Tag[] GetTags()
    {
        return new[] { Tag.Blood, Tag.Monument };
    }

    public CorruptedAngel(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
    }

    //TODO turn this into a rule, and make a corrupted angel tile to go with it
    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }

}
