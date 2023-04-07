using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Legion : Creature
{
    [SerializeField] new protected int scoreWorth = 0;

    public override string GetDescription()
    {
        return $"A random <b>Adjacent</b> tile BECOMES LEGION";
    }
    
    public Legion(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }

    public override void WhenPlaced()
    {
        string copyName = "Web"; //default value if no adjacent
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
        }
        
        GridManager.ForEach((int x, int y, ITile tile) =>
        {
            if (tile.GetType().Name == "Wasteland")
            {
                GridManager.Instance.PlaceTile(copyName, new Vector2Int(x, y));
            }
        });
    }
}