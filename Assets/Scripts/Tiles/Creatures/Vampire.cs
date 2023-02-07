using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;

public class Vampire : Monster
{
    [SerializeField] protected int packSize = 5;

    [SerializeField] protected int scoreWorth = 0;


    public new string Type = "Blood";

    public Vampire(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }

    private static Rule PackOfVampires = new Rule("Pack Of Vampires", 15, () =>
    {
        GridManager.ForEach((int x, int y, ITile tile) => {
            if (tile is River river)
            {
                river.VampireBloodMultiplier();
                river.WhenPlaced();
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
            GameManager.Instance.AddRule(PackOfVampires);
        }
    }


    protected override Score CalculateBaseScore()
    {
        Debug.Log("calculating score");
        return new Score(scoreWorth);
    }
}
