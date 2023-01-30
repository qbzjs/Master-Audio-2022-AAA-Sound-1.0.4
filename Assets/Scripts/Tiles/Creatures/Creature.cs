using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;

public class Creature : Wasteland
{
    public List<Creature> GroupedCreatures = new();
    [SerializeField] protected int packSize = 5;
    private bool packActivated = false;

    public Creature(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        GroupedCreatures.Add(this);
    }

    private int CountGroupCreatures()
    {
        foreach (Vector2Int dir in Directions.Cardinal)
        {
            ITile tile = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y);
            if (tile.GetType() == this.GetType())
            {
                foreach (Creature c in ((Creature)tile).GroupedCreatures)
                {
                    if (!GroupedCreatures.Contains(c))
                    {
                        GroupedCreatures.Add(c);
                    }
                }
            }
        }

        return GroupedCreatures.Count;
    }

}

