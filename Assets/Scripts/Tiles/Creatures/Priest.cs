using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;

public class Priest : Creature
{
    [SerializeField] protected int packSize = 5;

    private int timesFeasted = 0;


    public override string GetDescription()
    {
        return "5pts - In a pack of 5, churches and temples x2";
    }

    public override Tag[] GetTags()
    {
        return new[] { Tag.Chaos, Tag.Monster };
    }

    public Priest(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }

    private static Effect ChurchMultiplier =
        new Effect(
            "Church multiplier", 20, 1, 1, (value) =>
            {
                return new Score(value.score * 2, value.explanation + " * 2");
            }
        );

    private static Rule PackOfPriests = new Rule("Pack Of Priests", 9, () =>
    {
        GridManager.ForEach((int x, int y, ITile tile) => {
            if (tile is Church church)
            {
                church.AddEffect(ChurchMultiplier);
            }
            if (tile is Temple temple)
            {
                temple.AddEffect(ChurchMultiplier);
            }
        });

    });

    public override void WhenPlaced()
    {
        CalculatePack();
    }

    private void CalculatePack()
    {
        List<Creature> pack = new();
        pack.Add(this);
        int packCount = CountGroupCreatures(this.GetType(), pack);

        if (packCount >= packSize)
        {
            GameManager.Instance.AddRule(PackOfPriests);
        }
    }

    protected override Score CalculateBaseScore()
    {

        return new Score(5);
    }
}
