using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITile
{
    public GameObject TileObject { get; set; }
    
    public Vector3 LocalPosition();

    public Vector3 Position();

    public bool Destructible();

    public void WhenPlaced();

    public void AddEffect(Effect toAdd);

    //nextToGraveYard
    //graves - count how many times been destroyed since next to graveyard

    public int xPos { get; set; }
    public int yPos { get; set; }
    public string Type();
    public Score CalculateScore();
}

/// <summary>
///     public int order;
///     public int stacks;
///     public int maxStacks;
///     public Func<Score, Score> modify;
/// </summary>
public struct Effect : IComparable<Effect>
{
    public string description;
    public int order;
    public int stacks;
    public int maxStacks;
    public Func<Score, Score> modify;

   /// <summary>
   /// 
   /// </summary>
   /// <param name="myOrder"></param>
   /// <param name="myStacks"></param>
   /// <param name="myMaxStacks"></param>
   /// <param name="myModify"></param>
    public Effect(string myDescription, int myOrder, int myStacks, int myMaxStacks, Func<Score, Score> myModify)
    {
        description = myDescription;
        order = myOrder;
        stacks = myStacks;
        maxStacks = myMaxStacks;
        modify = myModify;
    }
    
    public int CompareTo(Effect other)
    {
        return order.CompareTo(other.order);
    }
}

public struct Score
{
    public int score;
    public string explanation;

    public Score(int _score, string _explanation)
    {
        score = _score;
        explanation = _explanation;
    }
    
    public Score(int _score)
    {
        score = _score;
        explanation = _score.ToString();
    }
}
