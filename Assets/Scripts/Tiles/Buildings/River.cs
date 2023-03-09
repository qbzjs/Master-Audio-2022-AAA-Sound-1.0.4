using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts;
using Unity.VisualScripting;
using UnityEngine;

public class River : Building
{
    public override bool HighlightPredicate(ITile otherTile)
    {
        return base.HighlightPredicate(otherTile) || otherTile.GetTags().Contains(Tag.Blood);
    }

    public override string GetDescription()
    {
        return "Becomes a blood river if next to a tile with #blood.<br>Place a tile next to a blood river to double it's score.";
    }

    private static Rule PropagateBlood = new Rule("Propagate Blood", 10, () =>
    {
        bool turnMore = true;
        while (turnMore)
        {
            turnMore = false;
            GridManager.ForEach((int x, int y, ITile tile) => { 
                if (tile is River river)
                {
                    turnMore = turnMore || river.CheckTurn();
                }
            });
        }
    });
    
    public River(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns>if it turned</returns>
    public bool CheckTurn()
    {
        foreach (Vector2Int dir in Directions.Cardinal)
        {
            ITile tile = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y);
            if (tile.GetTags().Contains(Tag.Blood))
            {
                //Debug.Log("going to turn: " + tile);
                TweenManager.Instance.Callout("Blood River Flows!", Position());
                GridManager.Instance.PlaceTile("BloodRiver", new Vector2Int(xPos, yPos));
                return true;
            }
        }
        return false;
    }

    public override void WhenPlaced()
    {
        GameManager.Instance.AddRule(PropagateBlood);
    }
}