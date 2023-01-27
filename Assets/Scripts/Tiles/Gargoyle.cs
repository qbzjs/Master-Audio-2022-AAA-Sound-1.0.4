using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Gargoyle : Wasteland
{
    [SerializeField] new protected int scoreWorth = 2;
    
    public Gargoyle(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }
    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }
    
}
