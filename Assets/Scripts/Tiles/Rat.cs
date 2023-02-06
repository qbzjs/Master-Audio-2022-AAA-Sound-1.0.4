﻿using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Rat : Wasteland
{
    [SerializeField] new protected int scoreWorth = 1;

    public static string Description
    {
        get
        {
            return "Rats beget rats";
        }
    }

    public static string PointDescription
    {
        get
        {
            return "1";
        }
    }
    
    public static Rule PropagateRats = new Rule("Propagate Rats", 10, () =>
    {
        bool checkAgain = true;
        while (checkAgain)
        {
            checkAgain = false;
            GridManager.ForEach((int x, int y, ITile tile) =>
            {
                if (!(tile.GetType() == typeof(Wasteland))) return;
                float otherRats = 0;
                foreach (Vector2Int dir in Directions.Cardinal)
                {
                    if (GridManager.Instance.GetTile(x + dir.x, y + dir.y) is Rat)
                    {
                        otherRats++;
                    }
                }

                if (otherRats >= 2)
                {
                    GridManager.Instance.PlaceTile("Rat", new Vector2Int(x, y));
                    checkAgain = true;
                }
            });
        }
        
    });
    
    public Rat(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }

    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }

    public override void WhenPlaced()
    {
        GameManager.Instance.AddRule(PropagateRats);
    }
}