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
        return "When placed: gets +2 for each adjacent Wasteland";
    }

    public override void WhenPlaced()
    {
        foreach (var dir in Directions.Cardinal)
        {
            ITile tile = GridManager.Instance.GetTile(dir.x + xPos, dir.y + yPos);
            if (tile.GetType() == typeof(Wasteland))
            {
                scoreWorth += 2;
            }
        }
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }
}
