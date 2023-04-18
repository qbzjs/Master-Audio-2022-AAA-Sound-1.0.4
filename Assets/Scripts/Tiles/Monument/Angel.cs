using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Angel : Monument
{
    [SerializeField] new protected int scoreWorth = -3;

    public override string GetDescription()
    {
        return "If <b>Adjacent</b> to #Blood, becomes <b>CorruptedAngel</b>";
    }

    public override Tag[] GetTags()
    {
        return new[] { Tag.Monument };
    }

    public Angel(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        GameManager.Instance.AddRule(CorruptAngel);
    }

    private static Rule CorruptAngel = new Rule("Corrupt Blood", 5, () =>
    {
        GridManager.ForEach((int x, int y, Angel angel) =>
        {
            foreach (Vector2Int dir in Directions.Cardinal)
            {
                ITile tile = (Wasteland)GridManager.Instance.GetTile(x + dir.x, y + dir.y);
                if (tile.GetTags().Contains(Tag.Blood))
                {
                    Vector2Int location = new Vector2Int(x, y);
                    GameManager.Instance.TransformTile(location, "CorruptedAngel");
                }
            }
        });
    });

    //TODO turn this into a rule, and make a corrupted angel tile to go with it
    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }

}
