using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Temple : Monument
{
    [SerializeField] protected int scoreWorth = 6;
    [SerializeField] protected int ghostsAbsorbed = 0;

    public Temple(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }
    public override Tag[] GetTags()
    {
        return new[] { Tag.Chaos, Tag.Monument };
    }


    public override string GetDescription()
    {
        return "The haunted temple! Absorbs all ghosts and their points";
    }

    public override void WhenPlaced()
    {
        GridManager.ForEach((int x, int y, Ghost ghost) =>
        {
            if (x == xPos && y == yPos) return; //if it's me, return

            ghostsAbsorbed += ghost.CalculateScore().score;
            GridManager.Instance.DestroyTile(new Vector2Int(x, y));
        });
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth + ghostsAbsorbed);
    }
}
