using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Scripts;
using UnityEngine.SocialPlatforms.Impl;

public class Human : Creature
{
    [SerializeField] protected int scoreWorth = 2;
    
    private static List<Vector2Int> toHaunt = new ();

    private Rule CheckForMonsters = new Rule("Humans checking for monsters", 2, () =>
    {
        GridManager.ForEach((int x, int y, ITile tile) =>
        {
            if (tile is Human human)
            {
                foreach (var dir in Directions.Cardinal)
                {
                    if (GridManager.Instance.GetTile(x + dir.x, y + dir.y).GetTags().Contains(Tag.Monster))
                    {
                        toHaunt.Add(new Vector2Int(x, y));
                    }
                }
            }
        });
    });
    
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
        return $"{scoreWorth}pts - On death: spawn a ghost worth 6 pts" +
                "<br> On Turn: dies if next to a tile with #monster";
    }
    
    
    public Human(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        GameManager.Instance.AddRule(CheckForMonsters);
        GameManager.Instance.AddRule(Haunting);
    }

    /*
    public override void WhenAnyDestroyed(int x, int y, ITile aboutToBeDestroyed)
    {
        if (aboutToBeDestroyed != this)
        {
            //If we're not referencing this object, don't do anything
            return;
        }

        toHaunt.Add(new Vector2Int(xPos, yPos));
    }*/

    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }
}
