using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Scripts;
using UnityEngine.SocialPlatforms.Impl;

public class Human : Creature, IEffectOnDestroyed
{
    [SerializeField] protected int scoreWorth = 3;
    
    private static List<Vector2Int> toHaunt = new ();

    private Rule CheckForMonsters = new Rule("Humans checking for monsters", 2, () =>
    {
        GridManager.ForEach((int x, int y, Human human) =>
        {
            foreach (var dir in Directions.Cardinal)
            {
                if (GridManager.Instance.GetTile(x + dir.x, y + dir.y).GetTags().Contains(Tag.Monster))
                {
                    GameManager.Instance.DestroyTile(new Vector2Int(x, y));
                    break;
                    //toHaunt.Add(new Vector2Int(x, y));
                }
            }
        });
    });
    
    
    public override string GetDescription()
    {
        return "If <b>Adjacent</b> to #Monster, is <b>Destroyed</b> and <b>Spawns</b> a <b>Ghost</b>.";
    }

    public override string GetCardRefName()
    {
        return "Ghost";
    }
    
    
    public Human(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        GameManager.Instance.AddRule(CheckForMonsters);
        //GameManager.Instance.AddRule(Haunting);
    }

    
    public override void WhenAnyDestroyed(int x, int y, ITile aboutToBeDestroyed)
    {
        if (aboutToBeDestroyed != this)
        {
            //If we're not referencing this object, don't do anything
            return;
        }


        //toHaunt.Add(new Vector2Int(xPos, yPos));
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }

    public Action GetInvokeAfterDestroyed()
    {
        Vector2Int spot = new Vector2Int(xPos, yPos);
        return () =>
        {
            TweenManager.Instance.Callout("Haunted!", spot);
            GameManager.Instance.PlaceTile("Ghost", spot);
        };
    }
}

