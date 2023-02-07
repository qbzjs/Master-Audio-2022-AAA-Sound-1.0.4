using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;

public class Creature : Wasteland
{
    [SerializeField] protected int packSize = 5;

    public Creature(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
    
    }

    public int CountGroupCreatures(Type type, List<Creature> pack)
    {
        foreach (Vector2Int dir in Directions.Cardinal)
        {
            ITile tile = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y);
            if (tile.GetType() == type && !pack.Contains((Creature)tile))
            {
                pack.Add((Creature)tile);
                ((Creature)tile).CountGroupCreatures(type, pack);
            }
        }

        return pack.Count;
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(0);
    }

}

