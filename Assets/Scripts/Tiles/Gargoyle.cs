using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Gargoyle : Wasteland
{
    [SerializeField] new protected int scoreWorth = 2;
    
    public Gargoyle(Transform parentTransform, Vector3 pos)
    {
        ConstructorHelper(parentTransform, pos, "Gargoyle");
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }

    public override string Type()
    {
        return "Gargoyle";
    }
    
}
