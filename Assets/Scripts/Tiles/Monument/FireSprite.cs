using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts;
using UnityEngine;

public class FireSprite : Monument
{
    [SerializeField] new protected int scoreWorth = 8;
    
    private Rule CheckSprites = new Rule("FireSprite checks for #fire", 200, () =>
    {
        GridManager.ForEach((int x, int y, FireSprite fireSprite) =>
        {
            bool hasFire = false;
            foreach (var dir in Directions.Cardinal)
            {
                if (GridManager.Instance.GetTile(x + dir.x, y + dir.y).GetTags().Contains(Tag.Fire))
                {
                    hasFire = true;
                }
            }

            if (!hasFire)
            {
                TweenManager.Instance.Callout("FireSprite went out :(", new Vector2Int(x, y));
                GridManager.Instance.DestroyTile(new Vector2Int(x, y));
            }
        });
    });
    
    public override string GetDescription()
    {
        return  $"{scoreWorth} pts - destroy if not adjacent to #fire";
    }

    public override Tag[] GetTags()
    {
        return new[] {Tag.Fire, Tag.Monument};
    }

    public FireSprite(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }

    public override void WhenPlaced()
    {
        GameManager.Instance.AddRule(CheckSprites);
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }
    
}
