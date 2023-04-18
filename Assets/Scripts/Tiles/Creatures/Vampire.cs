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
        return "In a <b>Pack</b> of <b><color=\"red\">5</color></b>, all <b>BloodRivers</b> <b><color=\"red\">x3</color></b>";
    }
    
    public override string GetCardRefName()
    {
        return "BloodRiver";
    }
    public override Tag[] GetTags()
    {
        return new []{Tag.Blood, Tag.Monster};
    }
    
    public Vampire(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        
    }

    private static Rule PackOfVampires = new Rule("Pack Of Vampires", 9, () =>
    {
        GridManager.ForEach((int x, int y, ITile tile) => {
            if (tile is Fountain fountain)
            {
                fountain.VampireBloodMultiplier();
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
        return new Score(2);
    }
}
