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

    new public string Type()
    {
        return "FO";
    }
    
    public string ShowCalculation()
    {
        var description = "Point Value: " + scoreWorth;
        return description;
    }
}
