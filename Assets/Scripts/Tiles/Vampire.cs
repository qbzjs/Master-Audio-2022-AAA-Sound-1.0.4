using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Vampire : Wasteland
{
    [SerializeField] new protected int scoreWorth = 4;

    private int timesFeasted = 0;
    
    public Vampire(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }

    public override void Observe(DefaultEvent e)
    {
        if (xPos != e.xPos || yPos != e.yPos)
        {
            //If we're not referencing this object, don't do anything
            return;
        }

        foreach (Vector2Int dir in Directions.Cardinal)
        {
            Vector2Int lookingAt = new Vector2Int(xPos + dir.x, yPos + dir.y);
            ITile potentialKill = GridManager.Instance.GetTile(lookingAt.x, lookingAt.y);
            if (potentialKill is Human)
            {
                GridManager.Instance.DestroyTile(lookingAt);
                timesFeasted++;
            }
        }
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(4 + timesFeasted * 3, $"4 + [{timesFeasted}] * 3");
    }
    
}
