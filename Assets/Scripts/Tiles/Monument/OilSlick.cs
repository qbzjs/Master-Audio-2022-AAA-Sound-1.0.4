using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Scripts;
using UnityEngine.SocialPlatforms.Impl;

public class OilSlick : Creature
{
    [SerializeField] protected int scoreWorth = 2;
    
    private static List<Vector2Int> toHaunt = new ();

    private static Effect Roaring = new Effect("Roaring", 3, 1, 5,
        (score) =>
        {
            return new Score(score.score + 2, $"{score.explanation} + 2");
        });
    
    private Rule BurnOil = new Rule("Oil checks for #fire", 80, () =>
    {
        bool goAgain = true;
        while (goAgain)
        {
            goAgain = false;
            GridManager.ForEach((int x, int y, OilSlick oil) =>
            {
                foreach (var dir in Directions.Cardinal)
                {
                    if (GridManager.Instance.GetTile(x + dir.x, y + dir.y).GetTags().Contains(Tag.Fire))
                    {
                        TweenManager.Instance.Callout("Oil to Hellfire!", new Vector2Int(x, y));
                        GridManager.Instance.PlaceTile("HellFire", new Vector2Int(x, y));
                        GridManager.Instance.GetTile(x, y).AddEffect(Roaring);
                        goAgain = true;
                    }
                }
            });           
        }

    });

    public override Tag[] GetTags()
    {
        return new[] {Tag.Darkness, Tag.Monument};
    }


    public override bool HighlightPredicate(ITile otherTile)
    {
        return otherTile.GetTags().Contains(Tag.Fire);
    }

    public override string GetDescription()
    {
        return $"{scoreWorth}pts - When adjacent to Fire, becomes a 5pt Hellfire.";
    }
    
    
    public OilSlick(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        GameManager.Instance.AddRule(BurnOil);
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }
}
