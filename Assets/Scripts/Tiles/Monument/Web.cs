using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts;
using UnityEngine;

public class Web : Monument
{
    [SerializeField] new protected int scoreWorth = 0;
    public override string GetDescription()
    {
        return scoreWorth + " pts - " + "Destroy if adjacent to #Fire";
    }

    private Rule BurnWebs = new Rule("Burn webs", 100, () =>
    {
        GridManager.ForEach((int x, int y, Web web) =>
        {
            foreach (var dir in Directions.Cardinal)
            {
                ITile tile = GridManager.Instance.GetTile(dir.x + x, dir.y + y);
                if (tile.GetTags().Contains(Tag.Fire))
                {
                    TweenManager.Instance.Callout("Web burns!", new Vector2Int(web.xPos, web.yPos));
                    GameManager.Instance.DestroyTile(new Vector2Int(web.xPos, web.yPos));
                }
            }
        });
    });

    public override Tag[] GetTags()
    {
        return new[] { Tag.Monument };
    }

    public Web(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        GameManager.Instance.AddRule(BurnWebs);
    }
    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }
    
}
