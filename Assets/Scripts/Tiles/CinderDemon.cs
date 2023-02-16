using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class CinderDemon : Wasteland
{
    [SerializeField] new protected int scoreWorth = 12;

    public override Tag[] GetTags()
    {
        return new[] {Tag.Fire};
    }
    
    public override string GetDescription()
    {
        return scoreWorth + " pts";
    }
    public CinderDemon(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }
    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }
    
}
