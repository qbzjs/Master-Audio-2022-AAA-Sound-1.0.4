using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class RatKing : Rat
{
    [SerializeField] new protected int scoreWorth = 0;

    public override string GetDescription()
    {
        return $"Destroy all other rats and gain their points.";
    }
    
    public RatKing(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }

    public override void WhenPlaced()
    {
        GridManager.ForEach((int x, int y, Rat rat) =>
        {
            if (x == xPos && y == yPos) return; //if it's me, return

            scoreWorth += rat.CalculateScore().score;
            GridManager.Instance.DestroyTile(new Vector2Int(x, y));            
        });
    }
}