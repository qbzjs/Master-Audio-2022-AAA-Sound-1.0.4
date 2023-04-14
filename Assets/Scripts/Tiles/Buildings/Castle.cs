using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil.Cil;
using Scripts;
using UnityEngine;

public class Castle : Monument
{
    public override string GetDescription()
    {
        return "<color=\"red\"><b>+2</b></color> to each <b>Adjacent</b> #Building when placed.";
    }

    private static Effect EdificeEffect = new Effect("Edifice", 3, 1, 4,
        (score) =>
        {
            return new Score(score.score + 2, $"{score.explanation} + 2");
        });

    /*
    private Rule Edificed = new Rule("Edificed", 80, () =>
    {
        GridManager.ForEach((int x, int y, Castle castle) =>
        {
            foreach (var dir in Directions.Cardinal)
            {
                if (GridManager.Instance.GetTile(x + dir.x, y + dir.y).GetTags().Contains(Tag.Building))
                {
                    ITile tile = GridManager.Instance.GetTile(x + dir.x, y + dir.y);
                    tile.AddEffect(EdificeEffect);
                }
            }
        });

    });*/

    public override Tag[] GetTags()
    {
        return new[] { Tag.Building };
    }

    public Castle(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        //GameManager.Instance.AddRule(Edificed);
    }
    protected override Score CalculateBaseScore()
    {
        return new Score(0);
    }

    public override void WhenPlaced()
    {
        foreach (var dir in Directions.Cardinal)
        {
            if (GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y).GetTags().Contains(Tag.Building))
            {
                ITile tile = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y);
                tile.AddEffect(EdificeEffect);
            }
        }
    }
}


/*Old castle code
 
     [SerializeField] protected int scoreWorthAdjacent = 2;

    private List<Tag> newTags = new List<Tag>();

    public override string GetDescription()
    {
        return "6 if adjacent to #monster. Else, +2 for each adjacent castle";
    }

    public override Tag[] GetTags()
    {
        return newTags.ToArray();
    }


    public Castle(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        newTags.Add(Tag.Building);
        newTags.Add(Tag.Royal);
    }

    protected override Score CalculateBaseScore()
    {
        int adjacentCastles = 0;

        bool adjacentMonster = false;

        foreach (Vector2Int dir in Directions.Cardinal)
        {
            ITile tile = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y);
            if (tile is Castle)
            {
                adjacentCastles++;
            }
            if (tile.GetTags().Contains(Tag.Monster))
            {
                adjacentMonster = true;
                foreach (Tag tag in tile.GetTags())
                {
                    if (!newTags.Contains(tag) && tag != Tag.Monster)
                    {
                        newTags.Add(tag);
                    }
                }
            }
        }

        if (adjacentMonster)
        {
            return new Score(6);
        }

        return new Score(2 + adjacentCastles * scoreWorthAdjacent);
    }
    */