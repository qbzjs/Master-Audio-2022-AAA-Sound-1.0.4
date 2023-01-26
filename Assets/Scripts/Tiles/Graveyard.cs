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

    //public override void WhenAnyDestroyed(int x, int y, ITile aboutToBeDestroyed)
    //{
    //    foreach (Vector2Int dir in Directions.Cardinal)
    //    {
    //        if (x == xPos + dir.x && x == yPos + dir.y)
    //        {
    //            adjacentDestroyed++;
    //        }
    //    }
    //}


    public override string Type()
    {
        return "Graveyard";
    }

    public override void Observe(DefaultEvent e)
    {
        Debug.Log($"bug B");
        foreach (Vector2Int dir in Directions.Cardinal)
        {
            if (e.xPos == xPos + dir.x && e.yPos == yPos + dir.y)
            {
                Debug.Log($"bug A");
                adjacentDestroyed++;
            }
        }
    }
}
