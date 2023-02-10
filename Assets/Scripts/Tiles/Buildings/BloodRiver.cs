using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts;
using Unity.VisualScripting;
using UnityEngine;

public class BloodRiver : Building
{
    private bool blood = false;

    public override string GetDescription()
    {
        return "Adjacent tiles become Bloody (Double score)";
    }

    public override Tag[] GetTags()
    {
        return new []{Tag.Blood};
    }

    private static Effect BloodMultiplier = 
        new Effect(
            "Blood multiplier", 20, 1, 1, (value) =>
            {
                return new Score(value.score * 2, value.explanation + " * 2");
            }
        );

    private static Rule SetBloodMultiplier = new Rule("Setting Blood Multiplier", 11, () =>
    {
        GridManager.ForEach((int x, int y, ITile tile) => { 
            if (tile is Fountain || tile is BloodRiver)
            {
                foreach (Vector2Int subdirection in Directions.Cardinal)
                {
                    GridManager.Instance.GetTile(x + subdirection.x, y + subdirection.y).AddEffect(BloodMultiplier);
                }
            }
        });
    });
    
    public BloodRiver(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }

    public void VampireBloodMultiplier()
    {
        BloodMultiplier = new Effect(
            "Blood multiplier", 20, 1, 1, (value) =>
            {
                return new Score(value.score * 3, value.explanation + " * 3");
            }
        );
    }

    public override void WhenPlaced()
    {
        GameManager.Instance.AddRule(SetBloodMultiplier);
    }
}