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

    public override Tag[] GetTags()
    {
        return new[] { Tag.Building };
    }

    public override string GetDescription()
    {
        return "If adjacent to #blood, turns into Blood River";
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
                Vector2Int location = new Vector2Int(xPos, yPos);
                GameManager.Instance.TransformTile(location, "BloodRiver");
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