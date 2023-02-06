using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;

public class Ghoul : Monster
{
    [SerializeField] protected int packSize = 5;

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
            ghoulStrength = 5;
        } else
        {
            ghoulStrength = packCount;
        }

        return new Score(ghoulStrength,
            $"[{ghoulStrength}]");
    }
}
