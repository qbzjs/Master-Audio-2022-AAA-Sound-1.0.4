using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts;
using UnityEngine;

public class Temple : Monument
{
    [SerializeField] protected int scoreWorth = -5;
    [SerializeField] protected int deathAbsorbed = 0;

    public Temple(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }
    public override Tag[] GetTags()
    {
        return new[] { Tag.Chaos, Tag.Building };
    }


    public override string GetDescription()
    {
        return "<b>Absorbs</b> all #Death when placed";
    }

    public override void WhenPlaced()
    {
        GridManager.ForEach((int x, int y, ITile tile) =>
        {
            if (x == xPos && y == yPos) return; //if it's me, return

            if (tile.GetTags().Contains(Tag.Death))
            {
                deathAbsorbed += tile.CalculateScore().score;
                GameManager.Instance.DestroyTile(new Vector2Int(x, y));
            }
        });
    }

    protected override Score CalculateBaseScore()
    {
        foreach (Vector2Int dir in Directions.Cardinal)
        {
            ITile tile = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y);
            if (tile is BloodRiver bloodRiver)
            {
                scoreWorth = 0;
            }
        }
        return new Score(scoreWorth + deathAbsorbed);
    }
}
