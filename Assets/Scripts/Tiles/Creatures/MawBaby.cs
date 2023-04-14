using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts;
using UnityEngine;

public class MawBaby : Monument
{
    [SerializeField] protected int absorbed = 1;

    public MawBaby(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }
    public override Tag[] GetTags()
    {
        return new[] { Tag.Monster};
    }


    public override string GetDescription()
    {
        return "<b>Absorbs</b> a random adjacent tile when placed";
    }

    public override void WhenPlaced()
    {
        List<Vector2Int> absorbList = new();
        foreach (Vector2Int dir in Directions.Cardinal)
        {
            ITile adjacentTile = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y);
            if (adjacentTile.GetType().Name != "Wasteland")
            {
                absorbList.Add(new Vector2Int(xPos + dir.x, yPos + dir.y));
            }
        }

        if (absorbList.Count > 0)
        {
            Vector2Int copyPosition = absorbList.PickRandom();
            
            TweenManager.Instance.Callout("Devoured!", copyPosition);
            
            absorbed += GridManager.Instance.GetTile(copyPosition.x, copyPosition.y).CalculateScore().score;
            GameManager.Instance.DestroyTile(copyPosition);
        }
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth + absorbed);
    }
}