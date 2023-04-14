using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Bauble : Monument
{
    [SerializeField] new protected int scoreWorth = 0;

    public override Tag[] GetTags()
    {
        return new[] { Tag.Null};
    }

    public override string GetDescription()
    {
        return " ";
    }

    public void SetScore(int newScore)
    {
        scoreWorth = newScore;

            TweenManager.Instance.Callout($"Bauble worth {scoreWorth}pts", Position());
    }
    
    public Bauble(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }
    
    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }
    
}