using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Graveyard : Wasteland
{
    [SerializeField] private int scoreWorthAdjacent = 1;
    public int adjacentDestroyed = 0;

    public Graveyard(Transform parentTransform, Vector3 pos)
    {
        ConstructorHelper(parentTransform, pos, "Graveyard");
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(adjacentDestroyed * scoreWorthAdjacent,
            $"[{adjacentDestroyed}] * {scoreWorthAdjacent}");
    }

    public override string Type()
    {
        return "Graveyard";
    }
}
