using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Gargoyle : Monument
{
    [SerializeField] new protected int scoreWorth = 2;

    public override Tag[] GetTags()
    {
        return new[] {Tag.Darkness};
    }

    public override string GetDescription()
    {
        return scoreWorth + " pts";
    }
    
    public Gargoyle(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }
    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }
    
}
