using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil.Cil;
using Scripts;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Edifice : Monument
{
    public override string GetDescription()
    {
        return "Worth 6 points, increases adjacent buildings' score by 2";
    }

    private static Effect Edifice = new Effect("Edifice", 4, 1, 1,
        (score) =>
        {
            return new Score(score.score + 2, $"{score.explanation} + 2");
        });

    private Rule Edificed = new Rule("Edificed", 80, () =>
    {
        GridManager.ForEach((int x, int y, Edifice edifice) =>
        {
            foreach (var dir in Directions.Cardinal)
            {
                if (GridManager.Instance.GetTile(x + dir.x, y + dir.y).GetTags().Contains(Tag.Building))
                {
                    GridManager.Instance.GetTile(x + dir.x, y + dir.y).AddEffect(Edifice);
                }
            }
        });

    });

    public override Tag[] GetTags()
    {
        return new[] { Tag.Blood, Tag.Monument };
    }

    public Edifice(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }
    protected override Score CalculateBaseScore()
    {
        return new Score(6);
    }

}
