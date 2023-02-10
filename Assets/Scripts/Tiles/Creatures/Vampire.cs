using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;

public class Vampire : Monster
{
    [SerializeField] protected int packSize = 5;

    private int timesFeasted = 0;

    protected new Tag[] tags = {Tag.Blood, Tag.Monster};
    
    
    public override string GetDescription()
    {
        return "3pts - In a pack of 5, blood rivers become x3";
    }
    
    public Vampire(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        Type = "Blood";
    }

    private static Rule PackOfVampires = new Rule("Pack Of Vampires", 9, () =>
    {
        GridManager.ForEach((int x, int y, ITile tile) => {
            if (tile is River river)
            {
                river.VampireBloodMultiplier();
            }
        });

    });

    public override void WhenPlaced()
    {
        CalculatePack();
        KillAdjacentHumans();
    }

    private void KillAdjacentHumans()
    {
        foreach (Vector2Int dir in Directions.Cardinal)
        {
            Vector2Int lookingAt = new Vector2Int(xPos + dir.x, yPos + dir.y);
            ITile potentialKill = GridManager.Instance.GetTile(lookingAt.x, lookingAt.y);
            if (potentialKill is Human)
            {
                GridManager.Instance.DestroyTile(lookingAt);
                timesFeasted++;
            }
        }
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
        return new Score(3);
    }
}
