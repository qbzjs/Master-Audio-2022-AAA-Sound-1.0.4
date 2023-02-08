using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;
using System.Threading;
using UnityEngine.SocialPlatforms.Impl;

public class Werewolf : Monster
{
    private int vampiresKilled = 0;

    public override string GetDescription()
    {
        return "<i>Kills adjacent vampires</i>";
    }

    public Werewolf(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        Type = "Dark";
    }

    protected override Score CalculateBaseScore()
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

        vampiresKilled += adjacentVampirePacks.Count;

        foreach(Vampire vampire in adjacentVampirePacks)
        {
            GridManager.Instance.KillTile(new Vector2Int(vampire.xPos, vampire.yPos));
        }

        return new Score(vampiresKilled);
    }
}
