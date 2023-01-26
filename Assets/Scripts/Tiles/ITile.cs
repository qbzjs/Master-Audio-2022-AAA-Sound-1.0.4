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

    public void Observe(DefaultEvent e);

    /// <summary>
    /// GameManager calls this when this ITile is placed. A good place to add Rules.
    /// </summary>
    public void WhenPlaced();

    /// <summary>
    /// GridManager calls this when any ITile is destroyed. Note: this is NOT when THIS is destroyed, when ANY OTHER
    ///     ITile is destroyed. This is so, for instance, graveyards can know when their neighbors are destroyed.
    /// </summary>
    /// <param name="x">x position of the tile</param>
    /// <param name="y">y position of the tile</param>
    /// <param name="aboutToBeDestroyed">The tile about to be destroyed</param>
    public void WhenAnyDestroyed(int x, int y, ITile aboutToBeDestroyed);
    
    /// <summary>
    /// Responsible for keeping track of effects added, Wasteland implements the functionality
    /// </summary>
    /// <param name="toAdd">Effect to add</param>
    public void AddEffect(Effect toAdd);

    public int xPos { get; set; }
    public int yPos { get; set; }
    public string Type();
    public Score CalculateScore();
}

public struct Effect : IComparable<Effect>
{
    public string description; //Used to compare equality, and may be used in the future to surface to players
    public int order; //Order in which effects are implied, lower is sooner (1, 2, 3...)
    public int stacks; //How many stacks of the effect are applied. TODO MAKE THIS DO SOMETHING
    public int maxStacks; //How many stacks it can have maximum. If the effect doesn't stack (like the BloodRiver)
                          //    Then maxStacks should equal 0. 
    public Func<Score, Score> modify; //Function which modifies a score. Scores have an int and a string describing
                                      // how that int was calculated.
                                      
    public Effect(string myDescription, int myOrder, int myStacks, int myMaxStacks, Func<Score, Score> myModify)
    {
        description = myDescription;
        order = myOrder;
        stacks = myStacks;
        maxStacks = myMaxStacks;
        modify = myModify;
    }
    
    //This implements IComparable so Lists can sort Effects in order
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
