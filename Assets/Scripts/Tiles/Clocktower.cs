using System;
using UnityEngine;

public class Clocktower : Wasteland, IEffectOnDestroyed
{
    public Clocktower(Transform parentTransform, Vector3 pos) : base(parentTransform, pos) { }

    public override string GetDescription()
    {
        return "While on the board, gives <b><color=\"red\">+1</color></b> turn";
    }

    public override Tag[] GetTags()
    {
        return new Tag[] {Tag.Building};
    }

    public override void WhenPlaced()
    {
        TweenManager.Instance.Callout("Extra Turn!", Position());
        GameManager.Instance.Turns++;
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(0);
    }

    public Action GetInvokeAfterDestroyed()
    {
        Vector3 position = Position();
        return () =>
        {
            TweenManager.Instance.Callout("Lost Turn!", position);
            GameManager.Instance.Turns--;
        };
    }
}