using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;

public class Vampire : Monster
{
    [SerializeField] protected int packSize = 5;

    public Vampire(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }

    protected override Score CalculateBaseScore()
    {
        return new Score(0);
    }

    public override void WhenPlaced()
    {
        int packCount = CountGroupCreatures();

        if (packCount >= packSize)
        {
            ObserverManager.Instance.ProcessEvent(new VampireEvent());
        }
    }
}
