using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Fountain : Wasteland
{
    new public bool Destructible()
    {
        return false;
    }

    public override string Type()
    {
        return "Fountain";
    }
    
    public string ShowCalculation()
    {
        var description = "Point Value: " + scoreWorth;
        return description;
    }
}
