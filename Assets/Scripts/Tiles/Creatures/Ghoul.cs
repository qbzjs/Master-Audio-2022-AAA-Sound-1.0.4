using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;

public class Ghoul : Monster
{
    [SerializeField] protected int packSize = 3;

    public override Tag[] GetTags()
    {
        return new [] {Tag.Monster, Tag.Death};
    }

    public override string GetDescription()
    {
        return "<b><color=\"red\">+1</color></b> for each <b>Ghoul</b> in a <b>Pack</b> of <b><color=\"red\">max 3</color></b>";
    }
    public Ghoul(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }

    protected override Score CalculateBaseScore()
    {
        int ghoulStrength = 1;

        List<Creature> pack = new();
        pack.Add(this);
        int packCount = CountGroupCreatures(this.GetType(), pack);

        if (packCount >= packSize)
        {
            ghoulStrength = packSize;
        } else
        {
            ghoulStrength = packCount;
        }

        return new Score(ghoulStrength,
            $"[{ghoulStrength}]");
    }
}
