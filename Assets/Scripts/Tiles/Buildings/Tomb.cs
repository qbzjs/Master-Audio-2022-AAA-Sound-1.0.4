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
        return "Starts open, will close once touches a pack of Vampires, grants 1 turn per vampire in pack";
    }

    public Tomb(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        Type = "Blood";
    }

    public override Tag[] GetTags()
    {
        return new[] { Tag.Blood };
    }

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
                GameManager.Instance.AddTurns(adjacentVampirePacks.Count);
            }

        }

        return new Score(0);
    }
}
