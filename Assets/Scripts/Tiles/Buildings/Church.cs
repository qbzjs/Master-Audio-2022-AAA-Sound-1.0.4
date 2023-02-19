using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Church : Building
{
    [SerializeField] protected int scoreWorth = 3;
    public new string Type = "Chaos";

    public override string GetDescription()
    {
        return "<i>A satanic church, worth 3 points</i>";
    }

    public Church(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }
}
