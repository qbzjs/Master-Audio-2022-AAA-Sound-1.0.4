using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class RatKing : Rat
{
    [SerializeField] new protected int scoreWorth = 0;
    protected bool justPlaced = true;

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
        int rats = 0;
        GridManager.ForEach((int x, int y, Rat rat) =>
        {
            if (x == xPos && y == yPos) return; //if it's me, return

            if (rat is RatKing)
            {
                return;
            }
            rats++;
            scoreWorth += rat.CalculateScore().score;
            GridManager.Instance.DestroyTile(new Vector2Int(x, y));
            justPlaced = false;
        });
        TweenManager.Instance.Callout($"Rat King eats {rats} rats!", Position());
    }
}