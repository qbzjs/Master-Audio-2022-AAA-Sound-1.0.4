using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class TheCollector : Wasteland
{
    [SerializeField] new protected int scoreWorth = 0;
    protected bool justPlaced = true;

    public override string GetDescription()
    {
        return "When Placed: <b>Absorbs</b> all other tiles";
    }
    
    public TheCollector(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }

    public override Tag[] GetTags()
    {
        return new[] { Tag.Null};
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }

    public override void WhenPlaced()
    {
        GridManager.ForEach((int x, int y, ITile tile) =>
        {
            if (x == xPos && y == yPos) return; //if it's me, return
            if (tile.GetType().ToString() == "Wasteland") return; //don't collect wastelands
            
            int baubleScore = tile.CalculateScore().score;
            //GameManager.Instance.TransformTile(new Vector2Int(x, y), "Bauble");
            scoreWorth += baubleScore;
        });
        
        GridManager.ForEach((int x, int y, ITile tile) =>
        {
            if (x == xPos && y == yPos) return; //if it's me, return
            if (tile.GetType().ToString() == "Wasteland") return; //don't collect wastelands
            
            GameManager.Instance.DestroyTile(new Vector2Int(x, y));
        });
        
    }
}