using System;

public struct Rule : IComparable<Rule>
{
    public string description;
    public int order; // 1 happens first. then 2, 3, so on
    public Action action;
    
    public Rule(string myDescription, int myOrder, Action myAction)
    {
        description = myDescription;
        order = myOrder;
        action = myAction;
    }
    
    public int CompareTo(Rule other) //So it can be sorted in a list
    {
        return order.CompareTo(other.order);
    }
}