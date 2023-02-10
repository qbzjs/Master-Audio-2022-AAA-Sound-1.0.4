﻿using UnityEngine;

public class Clocktower : Wasteland
{
    public Clocktower(Transform parentTransform, Vector3 pos) : base(parentTransform, pos) { }

    public override string GetDescription()
    {
        return "While on the board, gives +1 turn";
    }

    public override void WhenPlaced()
    {
        GameManager.Instance.Turns++;
    }

    public override void WhenAnyDestroyed(int x, int y, ITile aboutToBeDestroyed)
    {
        if (this != aboutToBeDestroyed)
        {
            //If we're not referencing this object, don't do anything
            return;
        }
        
        GameManager.Instance.Turns--;
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(0);
    }
}