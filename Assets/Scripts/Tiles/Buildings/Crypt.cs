using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts;
using UnityEngine;

public class Crypt : Building
{
    [SerializeField] protected int scoreWorth = 0;
    public Crypt(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }

    public override Tag[] GetTags()
    {
        return new[] { Tag.Death };
    }


    public override string GetDescription()
    {
        return "Consumes all adjacent death when placed, doubles score, adds turn for each tile consumed.";
    }

    public override void WhenPlaced()
    {
        foreach (Vector2Int dir in Directions.Cardinal)
        {
            ITile tile = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y);
            if (tile.GetTags().Contains(Tag.Death) && !(tile.GetType() == typeof(Crypt)))
            {
                scoreWorth += (tile.CalculateScore().score * 2);
                GridManager.Instance.DestroyTile(new Vector2Int(xPos + dir.x, yPos + dir.y));
            }
            GameManager.Instance.Turns++;
        }
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }
}
