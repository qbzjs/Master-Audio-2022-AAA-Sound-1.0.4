using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Ghost : Wasteland
{
    [SerializeField] new protected int scoreWorth = 6;
    protected new Tag[] tags = {Tag.Death};
    public override string GetDescription()
    {
        return scoreWorth + " pts";
    }
    public Ghost(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }
    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }
    
}
