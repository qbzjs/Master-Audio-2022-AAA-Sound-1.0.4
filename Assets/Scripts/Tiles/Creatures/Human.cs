using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;
using UnityEngine.SocialPlatforms.Impl;

public class Human : Creature
{
    [SerializeField] protected int scoreWorth = 0;
    
    private static List<Vector2Int> toHaunt = new ();

    private Rule Haunting = new Rule("A human died here", 1000, () =>
    {
        for (var index = 0; index < Human.toHaunt.Count; index++)
        {
            var spot = Human.toHaunt[index];
            if (GridManager.Instance.GetTile(spot.x, spot.y) is Wasteland)
            {
                GridManager.Instance.PlaceTile("Ghost", new Vector2Int(spot.x, spot.y));
            }
        }

        Human.toHaunt.Clear();
    });
    
    public override string GetDescription()
    {
        return "On death: spawn a ghost";
    }
    
    
    public Human(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        GameManager.Instance.AddRule(Haunting);
    }

    public override void WhenAnyDestroyed(int x, int y, ITile aboutToBeDestroyed)
    {
        if (aboutToBeDestroyed != this)
        {
            //If we're not referencing this object, don't do anything
            return;
        }

        toHaunt.Add(new Vector2Int(xPos, yPos));
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(0);
    }
}
