using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Castle : Building
{
    [SerializeField] protected int scoreWorthAdjacent = 2;

    private List<Tag> newTags = new List<Tag>();

    public override string GetDescription()
    {
        return "+2 point for each adjacent castle. If next to a monster, score is 6 but cannot be increased (eat the rich).";
    }

    public override Tag[] GetTags()
    {
        return newTags.ToArray();
    }


    public Castle(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }

    protected override Score CalculateBaseScore()
    {
        int adjacentCastles = 0;

        bool adjacentMonster = false;

        foreach (Vector2Int dir in Directions.Cardinal)
        {
            ITile tile = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y);
            if (tile is Castle)
            {
                adjacentCastles++;
            }
            if (tile.GetTags().Contains(Tag.Monster))
            {
                adjacentMonster = true;
                foreach (Tag tag in tile.GetTags())
                {
                    if (!newTags.Contains(tag) && tag != Tag.Monster)
                    {
                        newTags.Add(tag);
                    }
                }
            }
        }

        if (adjacentMonster)
        {
            return new Score(6);
        }

        return new Score(2 + adjacentCastles * scoreWorthAdjacent);
    }
}
