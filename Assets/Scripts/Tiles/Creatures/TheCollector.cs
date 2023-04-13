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
        return $"Transform all non-Bauble tiles into a <b><link>Bauble</link></b> of equal worth";
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
        int rats = 0;
        GridManager.ForEach((int x, int y, ITile tile) =>
        {
            if (x == xPos && y == yPos) return; //if it's me, return
            if (tile.GetType().ToString() == "Wasteland") return; //don't collect wastelands
            
            int baubleScore = tile.CalculateScore().score;
            GameManager.Instance.TransformTile(new Vector2Int(x, y), "Bauble");
            ((Bauble)GridManager.Instance.GetTile(x, y)).SetScore(baubleScore);
        });
        
    }
}