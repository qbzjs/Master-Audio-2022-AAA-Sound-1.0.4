using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Fountain : Monument
{

    public string Type = "Blood";

    public override string GetDescription()
    {
        return "My blood spilleth over";
    }
    public override bool Destructible()
    {
        return false;
    }
    
    public Fountain(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
    }
    
    public string ShowCalculation()
    {
        var description = "Point Value: " + scoreWorth;
        return description;
    }
}
