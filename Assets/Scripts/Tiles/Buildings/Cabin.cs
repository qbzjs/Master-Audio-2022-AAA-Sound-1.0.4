using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Cabin : Building
{
    [SerializeField] protected int scoreWorth = 0;
    public new string Type = "Chaos";

    public Cabin(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }

    public override string GetDescription()
    {
        return "<i>The haunted temple! Absorbs all ghosts and their points, with a base value of 6</i>";
    }

    public override void WhenPlaced()
    {
        GridManager.ForEach((int x, int y, Wasteland wasteland) =>
        {
            if (x == xPos && y == yPos) return; //if it's me, return
            ITile tile = GridManager.Instance.GetTile(x, y);
            if (tile is Wasteland)
            {
                scoreWorth++;
                return;
            }
        });
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }
}
