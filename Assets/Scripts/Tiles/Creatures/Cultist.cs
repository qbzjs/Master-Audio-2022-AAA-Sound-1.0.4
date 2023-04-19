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
        return $"<b><color=\"red\">+{2}</color></b> to each <b>Adjacent</b> #Monument.";
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
        GridManager.ForEach((int x, int y, Monument monument) =>
        {
            if (monument.GetTags().Contains(Tag.Monument))
            {
                int numCultists = 0;
                foreach (var dir in Directions.Cardinal)
                {
                    ITile tile = GridManager.Instance.GetTile(x + dir.x, y + dir.y);
                    if (tile.GetType() == typeof(Cultist))
                    {
                        numCultists++;
                    }
                }
                //add effect to tile
                if (monument.ongoingEffects.Any((value) => { return value.description.Equals(Worshipped.description); }))
                {
                    Effect priorEffect = monument.ongoingEffects.FirstOrDefault((value) => { return value.description.Equals(Worshipped.description); });
                    monument.ongoingEffects.Remove(priorEffect);
                    priorEffect.stacks = numCultists;
                    monument.ongoingEffects.Add(priorEffect);
                } else
                {
                    for (int i = 0; i < numCultists; i++)
                    {
                        monument.AddEffect(Worshipped);
                    }
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
