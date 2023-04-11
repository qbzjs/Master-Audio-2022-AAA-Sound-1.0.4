using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Fountain : Monument
{

    public string Type = "Blood";

    public override string GetDescription()
    {
        return "<b><color=\"red\">x2</color></b> the score of each <b>Adjacent</b> tile";
    }

    public override Tag[] GetTags()
    {
        return new[] {Tag.Blood, Tag.Monument};
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
                    ITile bloodyTile = GridManager.Instance.GetTile(x + subdirection.x, y + subdirection.y);
                    if (bloodyTile.TileObject != null)
                    {
                        if (!bloodyTile.HasEffect(BloodMultiplier))
                        {
                            ParticleManager.Instance.InstantiateBloodParticles(bloodyTile.TileObject.transform);
                        }
                        bloodyTile.AddEffect(BloodMultiplier);
                    }
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
        BloodMultiplier = new Effect(
            "Blood multiplier", 20, 1, 1, (value) =>
            {
                return new Score(value.score * 2, value.explanation + " * 2");
            }
        );
        GameManager.Instance.AddRule(SetBloodMultiplier);
        ParticleManager.Instance.InstantiateBloodGlow(TileObject.transform);
    }
}
