using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Graveyard : Wasteland
{
    [SerializeField] private int scoreWorthAdjacent = 1;
    public int adjacentDestroyed = 0;
    public override string GetDescription()
    {
        return "+1 when an adjacent tile is destroyed";
    }

    public override Tag[] GetTags()
    {
        return new[] {Tag.Death};
    }

    public Graveyard(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }

    protected override Score CalculateBaseScore()
    {
        return new Score(adjacentDestroyed * scoreWorthAdjacent,
            $"[{adjacentDestroyed}] * {scoreWorthAdjacent}");
    }

    public override void Observe(DefaultEvent e)
    {
        foreach (Vector2Int dir in Directions.Cardinal)
        {
            if (e.xPos == xPos + dir.x && e.yPos == yPos + dir.y)
            {
                adjacentDestroyed++;
            }
        }
    }
}
