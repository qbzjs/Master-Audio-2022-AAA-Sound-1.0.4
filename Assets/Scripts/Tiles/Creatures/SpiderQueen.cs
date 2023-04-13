using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class SpiderQueen : Animal
{
    [SerializeField] new protected int scoreWorth = 8;

    public override string GetDescription()
    {
        return $"<b>Fills its Row and Column with </b> <b><link>Web</link>s</b>.";
    }
    
    public SpiderQueen(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
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
        List<Vector2Int> spots = new();
        for (int i = 0; i < GridManager.Instance.Size; i++)
        {
            spots.Add(new Vector2Int(xPos, i));
            spots.Add(new Vector2Int(i, yPos));
        }
        foreach (var dir in spots)
        {
            Vector2Int gridPos = new Vector2Int(dir.x, dir.y);
            ITile tile = GridManager.Instance.GetTile(gridPos.x, gridPos.y);
            if ((tile.GetType() == typeof(Wasteland)))
            {
                GameManager.Instance.PlaceTile("Web", gridPos);
            }
        }
    }
}