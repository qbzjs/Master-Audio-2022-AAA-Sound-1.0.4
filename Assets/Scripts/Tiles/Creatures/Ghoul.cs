using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;

public class Ghoul : Monster
{
    [SerializeField] protected int packSize = 5;

    public Ghoul(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }

    private static Effect GhoulTenement =
        new Effect(
            "Ghoul tenement", 30, 1, 1, (value) =>
            {
                return new Score(value.score * 2, value.explanation + " * 2");
            }
        );

    private static Effect GhoulMansion =
        new Effect(
            "Ghoul manion", 30, 1, 1, (value) =>
            {
                return new Score(value.score / 2, value.explanation + " / 2");
            }
        );

    private static Rule PackOfGhouls = new Rule("Pack Of Ghouls", 15, () =>
    {
        GridManager.ForEach((int x, int y, ITile tile) => {
            if (tile is Tenement)
            {
                tile.AddEffect(GhoulTenement);
            }
            if (tile is Mansion)
            {
                tile.AddEffect(GhoulMansion);
            }
        });

    });

    public override void WhenPlaced()
    {
        List<Creature> pack = new();
        pack.Add(this);
        int packCount = CountGroupCreatures(this.GetType(), pack);

        if (packCount >= packSize)
        {
            GameManager.Instance.AddRule(PackOfGhouls);
        }
    }
}
