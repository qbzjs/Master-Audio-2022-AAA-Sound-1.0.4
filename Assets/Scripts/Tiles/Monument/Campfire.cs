using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;

public class Campfire : Wasteland
{
    public override string GetDescription()
    {
        return "-2 points each turn. Destroyed when it reaches 0.";
    }
    
    public override Tag[] GetTags()
    {
        return new []{Tag.Fire};
    }
    
    public Campfire(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        scoreWorth = 12;
    }

    private static Rule DecrementCampfires = new Rule("Campfires burn lower", 2, () =>
    {
        GridManager.ForEach((int x, int y, Campfire campfire) =>
        {
            campfire.scoreWorth = campfire.scoreWorth - 2;
            if (campfire.scoreWorth <= 0)
            {
                GridManager.Instance.DestroyTile(new Vector2Int(x, y));
            }
        });

    });

    public override void WhenPlaced()
    {
        GameManager.Instance.AddRule(DecrementCampfires);
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }
}
