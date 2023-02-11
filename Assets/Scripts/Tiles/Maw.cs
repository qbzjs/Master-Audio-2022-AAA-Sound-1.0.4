using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Maw : Monument
{
    public override string GetDescription()
    {
        return "MAW";
    }

    public override Tag[] GetTags()
    {
        return new[] {Tag.Maw};
    }

    public Maw(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }

    public override void WhenPlaced()
    {
        foreach (Vector2Int dir in Directions.Compass)
        {
            GridManager.Instance.DestroyTile(new Vector2Int(dir.x + xPos, dir.y + yPos));
        }
        GridManager.Instance.DestroyTile(new Vector2Int(xPos, yPos));
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(0, "MAW");
    }
    
}
