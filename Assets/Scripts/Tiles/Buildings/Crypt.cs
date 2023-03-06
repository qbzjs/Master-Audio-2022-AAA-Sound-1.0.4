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
        return "Consumes all adjacent death, doubles score, adds turn for each tile consumed.";
    }

    public override void WhenPlaced()
    {
        foreach (Vector2Int dir in Directions.Cardinal)
        {
            ITile tile = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y);
            if (tile.GetTags().Contains(Tag.Death))
            {
                scoreWorth += (tile.CalculateScore().score * 2);
            }
            GameManager.Instance.Turns++;
        }
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }
}
