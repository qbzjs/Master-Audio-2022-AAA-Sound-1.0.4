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
                TweenManager.Instance.Callout("Cinder Demon!", new Vector2Int(x, y));
                GameManager.Instance.TransformTile( new Vector2Int(x, y), "CinderDemon");
            }
        });
    });

    public override bool HighlightPredicate(ITile otherTile)
    {
        return otherTile.GetTags().Contains(Tag.Fire);
    }

    public override string GetDescription()
    {
        return $"If <b>Adjacent</b> to <b><color=\"red\">4</color></b> #fire becomes <b>CinderDemon</b>";
    }
    
    public override string GetCardRefName()
    {
        return "CinderDemon";
    }
    
    public Cinders(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
    }

    public override void WhenPlaced()
    {
        GameManager.Instance.AddRule(CheckCinders);
    }

    public override Tag[] GetTags()
    {
        return new[] {Tag.Fire, Tag.Monument};
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }
}
