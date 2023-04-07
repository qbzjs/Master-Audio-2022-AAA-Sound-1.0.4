using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class YagaHut : Creature
{
    [SerializeField] new protected int scoreWorth = 3;
    public override string GetDescription()
    {
        return " ";
    }

    public override Tag[] GetTags()
    {
        return new[] {Tag.Monster, Tag.Chaos};
    }

    public YagaHut(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }
    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }
    
}
