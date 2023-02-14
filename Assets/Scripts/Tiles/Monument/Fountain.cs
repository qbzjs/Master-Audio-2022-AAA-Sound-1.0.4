using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Fountain : Monument
{

    public string Type = "Blood";

    public override string GetDescription()
    {
        return "My blood spilleth over";
    }

    public override Tag[] GetTags()
    {
        return new[] {Tag.Blood};
    }


    private static Effect BloodMultiplier = 
        new Effect(
            "Blood multiplier", 20, 1, 1, (value) =>
            {
                return new Score(value.score * 2, value.explanation + " * 2");
            }
        );

    private Rule SetBloodMultiplier = new Rule("Setting Blood Multiplier", 11, () =>
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

    public void VampireBloodMultiplier()
    {
        BloodMultiplier = new Effect(
            "Blood multiplier", 20, 1, 1, (value) =>
            {
                return new Score(value.score * 3, value.explanation + " * 3");
            }
        );
    }
    
    public Fountain(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        GameManager.Instance.AddRule(SetBloodMultiplier);
    }
}
