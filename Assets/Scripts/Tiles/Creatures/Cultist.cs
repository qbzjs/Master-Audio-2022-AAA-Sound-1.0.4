using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Cultist : Creature
{
    [SerializeField] new protected int scoreWorth = 2;
    public override string GetDescription()
    {
        return scoreWorth + " pts, gives adjacent Totems the worshipped effect (+2 pts, stacks).";
    }

    public override Tag[] GetTags()
    {
        return new[] { Tag.Darkness };
    }

    private static Effect Worshipped = new Effect("Worshipped", 3, 1, 4,
        (score) =>
        {
            return new Score(score.score + 2, $"{score.explanation} + 2");
        });

    private Rule Worship = new Rule("cultists check for totem to worship", 80, () =>
    {
        GridManager.ForEach((int x, int y, Cultist oil) =>
        {
            foreach (var dir in Directions.Cardinal)
            {
                ITile tile = GridManager.Instance.GetTile(x + dir.x, y + dir.y);
                if (tile.GetType() == typeof(Totem))
                {
                    tile.AddEffect(Worshipped);
                }
            }
        });
    });

    public Cultist(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        GameManager.Instance.AddRule(Worship);
    }
    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }

}
