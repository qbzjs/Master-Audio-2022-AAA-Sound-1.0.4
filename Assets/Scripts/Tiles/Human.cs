using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Human : Wasteland
{
    [SerializeField] new protected int scoreWorth = 1;

    private static List<Vector2Int> toHaunt = new ();

    private Rule Haunting = new Rule("A human died here", 1000, () =>
    {
        foreach (var spot in toHaunt)
        {
            if (GridManager.Instance.GetTile(spot.x, spot.y) is Wasteland)
            {
                GridManager.Instance.PlaceTile("Ghost", new Vector2Int(spot.x, spot.y));
            }   
        }
        toHaunt.Clear();
    });
    
    public Human(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        GameManager.Instance.AddRule(Haunting);
    }

    public override void Observe(DefaultEvent e) //Should be called when a tile is destroyed
    {
        if (xPos != e.xPos || yPos != e.yPos)
        {
            //If we're not referencing this object, don't do anything
            return;
        }

        toHaunt.Add(new Vector2Int(xPos, yPos));
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }
    
}
