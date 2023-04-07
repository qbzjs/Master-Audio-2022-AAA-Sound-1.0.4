using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Doppelganger : Creature
{
    [SerializeField] new protected int scoreWorth = 0;

    public override string GetDescription()
    {
        return $"<b>Spawn</b> a copy of a random <b>Adjacent</b> tile";
    }
    
    public Doppelganger(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }

    public override void WhenPlaced()
    {
        string copyName = "Doppelganger"; //default value if no adjacent
        List<string> copyList = new();
        foreach (Vector2Int dir in Directions.Cardinal)
        {
            ITile adjacentTile = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y);
            if (adjacentTile.GetType().Name != "Wasteland")
            {
                copyList.Add(adjacentTile.GetType().Name);
            }
        }

        if (copyList.Count > 0)
        {
            copyName = copyList.PickRandom();
            GameManager.Instance.TransformTile(new Vector2Int(xPos, yPos), copyName);
        }
        
    }
}