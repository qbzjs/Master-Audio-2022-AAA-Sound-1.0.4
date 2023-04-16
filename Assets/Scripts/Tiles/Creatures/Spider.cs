using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Spider : Animal
{
    [SerializeField] new protected int scoreWorth = 6;

    public override string GetDescription()
    {
        return $"When Placed: <b>Spawns</b> <b>Web</b> at each <b>Adjacent</b> empty tile.";
    }
    
    public override string GetCardRefName()
    {
        return "Web";
    }
    public Spider(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }

    public override Tag[] GetTags()
    {
        return new[] { Tag.Animal };
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }

    public override void WhenPlaced()
    {
        foreach (var dir in Directions.Cardinal)
        {
            Vector2Int gridPos = new Vector2Int(dir.x + xPos, dir.y + yPos);
            ITile tile = GridManager.Instance.GetTile(gridPos.x, gridPos.y);
            if ((tile.GetType() == typeof(Wasteland)) && GridManager.Instance.OverGrid(gridPos))
            {
                GameManager.Instance.PlaceTile("Web", gridPos);
            }
        }
    }
}