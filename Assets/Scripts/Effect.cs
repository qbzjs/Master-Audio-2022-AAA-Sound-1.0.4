using System;

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