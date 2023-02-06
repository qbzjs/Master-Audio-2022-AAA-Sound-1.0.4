using System.Collections;
using System.Collections.Generic;
using Scripts;
using Unity.VisualScripting;
using UnityEngine;

public class River : Wasteland
{
    private bool blood = false;

    private static Effect BloodMultiplier = 
        new Effect(
            "Blood multiplier", 20, 1, 1, (value) =>
            {
                return new Score(value.score * 2, value.explanation + " * 2");
            }
        );

    private static Rule PropagateBlood = new Rule("Propagate Blood", 10, () =>
    {
        bool turnMore = true;
        while (turnMore)
        {
            turnMore = false;
            GridManager.ForEach((int x, int y, ITile tile) => { 
                if (tile is River)
                {
                    turnMore = turnMore || ((River) tile).CheckTurn();
                }
            });
        }
        
        GridManager.ForEach((int x, int y, ITile tile) => { 
            if (tile is Fountain || (tile is River river && river.blood))
            {
                
                foreach (Vector2Int subdirection in Directions.Cardinal)
                {
                    GridManager.Instance.GetTile(x + subdirection.x, y + subdirection.y).AddEffect(BloodMultiplier);
                }
            }
        });

    });
    
    public River(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }
    public override bool Destructible()
    {
        return !blood;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns>if it turned</returns>
    public bool CheckTurn()
    {
        foreach (Vector2Int dir in Directions.Cardinal)
        {
            if (blood) continue; //can't turn if already made of blood
            ITile tile = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y);
            if (tile is Fountain || (tile is River river && river.blood))
            {
                blood = true;
                TileObject.GetComponent<SpriteRenderer>().sprite = LoadArt("BloodRiver");
                return true;
            }
        }
        return false;
    }

    public bool IsBlood()
    {
        return blood;
    }

    public void VampireBloodMultiplier()
    {
        BloodMultiplier = new Effect(
            "Blood multiplier", 20, 1, 1, (value) =>
            {
                return new Score(value.score * 3, value.explanation + " * 3");
            }
        );
    }

    public override void WhenPlaced()
    {
        GameManager.Instance.AddRule(PropagateBlood);
    }
}