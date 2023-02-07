using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;

public class Vampire : Monster
{
    [SerializeField] protected int packSize = 5;

    private int timesFeasted = 0;


    public override string GetDescription()
    {
        return "5pts - In a pack of 5, blood rivers become x3";
    }
    
    public override Tag[] GetTags()
    {
        return new []{Tag.Blood, Tag.Monster};
    }
    
    public Vampire(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        
    }

<<<<<<< HEAD
    private static Rule PackOfVampires = new Rule("Pack Of Vampires", 9, () =>
=======
    private static Rule PackOfVampires = new Rule("Pack Of Vampires", 15, () =>
    {
        GridManager.ForEach((int x, int y, ITile tile) => {
            if (tile is BloodRiver bloodRiver)
            {
                bloodRiver.VampireBloodMultiplier();
            }
        });

    });

    protected override Score CalculateBaseScore()
>>>>>>> 3381f9f (fixed creature summation)
    {
        GridManager.ForEach((int x, int y, ITile tile) => {
            if (tile is BloodRiver bloodRiver)
            {
                bloodRiver.VampireBloodMultiplier();
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
            GameManager.Instance.AddRule(PackOfVampires);
        }
    }
    
    protected override Score CalculateBaseScore()
    {
        return new Score(5);
    }
}
