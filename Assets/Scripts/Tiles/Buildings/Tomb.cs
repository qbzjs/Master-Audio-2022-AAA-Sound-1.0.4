using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Tomb : Building
{
    private bool open = true;

    public override string GetDescription()
    {
        return "Starts open, will close once touches a pack of Vampires, grants 1 turn per vampire in pack, harms churche once closed";
    }

    public Tomb(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }

    private static Effect ChurchSubtractor;

    private static Rule PropagateTomb = new Rule("Propagate Tomb", 10, () =>
    {
        GridManager.ForEach((int x, int y, ITile tile) => { 
            if (tile is Church)
            {
                tile.AddEffect(ChurchSubtractor);
            }
        });

    });

    protected override Score CalculateBaseScore()
    {
        if (open)
        {
            List<Creature> adjacentVampirePacks = new();
            foreach (Vector2Int dir in Directions.Cardinal)
            {
                ITile tile = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y);
                if (tile is Vampire vampire)
                {
                    List<Creature> pack = new();
                    pack.Add(vampire);
                    vampire.CountGroupCreatures(vampire.GetType(), pack);
                    adjacentVampirePacks.AddRange(pack);
                }
            }

            if (adjacentVampirePacks.Count > 0)
            {
                open = false;
                GameManager.Instance.ChangeTurns(adjacentVampirePacks.Count);
                int delta;
                if (adjacentVampirePacks.Count >= 5)
                {
                    delta = 5;
                } else
                {
                    delta = adjacentVampirePacks.Count;
                }
                
                ChurchSubtractor =
                    new Effect(
                        "Church subtractor", 10, 1, 1, (value) =>
                        {
                            return new Score(value.score - delta, value.explanation + " - " + delta);
                        }
                    );

                GameManager.Instance.AddRule(PropagateTomb);
            }

        }

        return new Score(0);
    }
}
