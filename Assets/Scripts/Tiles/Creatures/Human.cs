using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;
using UnityEngine.SocialPlatforms.Impl;

public class Human : Creature
{
    [SerializeField] protected int scoreWorth = 5;
    private bool ghost = false;

    public Human(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }

    protected override Score CalculateBaseScore()
    {
        bool adjacentMonsters = false;

        foreach (Vector2Int dir in Directions.Cardinal)
        {
            ITile tile = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y);
            if (tile is Monster)
            {
                adjacentMonsters = true;
            }
        }

        if (adjacentMonsters)
        {
            ghost = true;
            return new Score(scoreWorth, $"{scoreWorth}");
        }

        return new Score(0);
    }
}
