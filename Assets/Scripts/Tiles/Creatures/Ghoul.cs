using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;

public class Ghoul : Monster
{
    [SerializeField] protected int packSize = 3;
    public override string GetDescription()
    {
        return "+1 for each ghoul in its pack (up to 3)";
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
