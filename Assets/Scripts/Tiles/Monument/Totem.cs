using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using Scripts;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Totem : Monument
{
    [SerializeField] new protected int scoreWorth = 2;
    public override string GetDescription()
    {
        return "+2 for each <b>Adjacent</b> #Monster";
    }

    public override Tag[] GetTags()
    {
        return new[] { Tag.Darkness, Tag.Monument };
    }

    public Totem(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }
    protected override Score CalculateBaseScore()
    {
        int numMonsters = 0;
        foreach (Vector2Int dir in Directions.Cardinal)
        {
            ITile tile = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y);
            if (tile is Monster monster)
            {
                numMonsters++;
            }
        }
        return new Score(scoreWorth * numMonsters);
    }

}
