using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Scripts;
using UnityEngine.SocialPlatforms.Impl;

public class Cinders : Wasteland
{
    [SerializeField] protected int scoreWorth = 0;

    private Rule CheckCinders = new Rule("Cinders checking for fire", 92, () =>
    {
        GridManager.ForEach((int x, int y, Cinders cinders) =>
        {
            int numFire = 0;
            foreach (var dir in Directions.Cardinal)
            {
                if (GridManager.Instance.GetTile(x + dir.x, y + dir.y).GetTags().Contains(Tag.Fire))
                {
                    numFire++;
                }
            }

            if (numFire == 4)
            {
                GridManager.Instance.PlaceTile("CinderDemon", new Vector2Int(x, y));
            }
        });
    });

    public override bool HighlightPredicate(ITile otherTile)
    {
        return otherTile.GetTags().Contains(Tag.Fire);
    }

    public override string GetDescription()
    {
        return $"If surrounded by #fire, become Cinder Demon";
    }
    
    
    public Cinders(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        GameManager.Instance.AddRule(CheckCinders);
    }

    public override Tag[] GetTags()
    {
        return new[] {Tag.Fire};
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }
}
