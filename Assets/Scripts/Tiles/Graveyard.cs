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

    public override int CalculateScore()
    {
        return scoreWorth + adjacentDestroyed * scoreWorthAdjacent;
    }

    public override string Type()
    {
        return "Graveyard";
    }
    public string ShowCalculation()
    {
        var description = "Point Value: " + scoreWorth + " Points from Destroyed Adjacent Tiles: " + (scoreWorthAdjacent * adjacentDestroyed);
        return description;
    }

}
