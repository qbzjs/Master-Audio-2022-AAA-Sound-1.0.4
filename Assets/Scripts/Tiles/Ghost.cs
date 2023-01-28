using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Ghost : Wasteland
{
    [SerializeField] new protected int scoreWorth = 10;
    
    public Ghost(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }
    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }
    
}
