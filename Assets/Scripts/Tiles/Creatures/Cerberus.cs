using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;

public class Cerberus : Monster
{
    [SerializeField] protected int packSize = 3;

    private int bigScore = 10, smallScore = 0;
    private bool called = false;

    public override Tag[] GetTags()
    {
        return new [] {Tag.Monster};
    }

    public override string GetDescription()
    {
        return "Worth <b><color=\"red\">10</color></b> in a <b>Pack</b> of <b><color=\"red\">3</color></b>.";
    }
    public Cerberus(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }

    protected override Score CalculateBaseScore()
    {
        List<Creature> pack = new();
        pack.Add(this);
        int packCount = CountGroupCreatures(this.GetType(), pack);

        if (packCount == packSize)
        {
            scoreWorth = bigScore;
            if (!called)
            {
                called = true;
                TweenManager.Instance.Callout("Guard dog of hell!", Position());
            }
        } else
        {
            scoreWorth = smallScore;
        }

        return new Score(scoreWorth);
    }
}
