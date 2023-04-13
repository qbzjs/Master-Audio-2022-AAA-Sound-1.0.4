using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Tombstone : Wasteland
{
    [SerializeField] private int scoreWorthAdjacent = 3;
    public int adjacentDestroyed = 0;
    public override string GetDescription()
    {
        return "<b><color=\"red\">+3</color></b> when the tile directly above this is <b>Destroyed</b>.";
    }

    public override Tag[] GetTags()
    {
        return new[] {Tag.Death};
    }

    public Tombstone(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }

    protected override Score CalculateBaseScore()
    {
        return new Score(adjacentDestroyed * scoreWorthAdjacent,
            $"[{adjacentDestroyed}] * {scoreWorthAdjacent}");
    }

    public override void WhenAnyDestroyed(int x, int y, ITile aboutToBeDestroyed)
    {
        if (x == xPos && y == yPos + 1)
        {
            adjacentDestroyed++;
        }
    }
}