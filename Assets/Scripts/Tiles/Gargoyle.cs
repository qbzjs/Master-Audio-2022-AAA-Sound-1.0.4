using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Gargoyle : Wasteland
{
    [SerializeField] new protected int scoreWorth = 2;
    
    public Gargoyle(Transform parentTransform, Vector3 pos)
    {
        ConstructorHelper(parentTransform, pos, "Gargoyle");
    }

    public override int CalculateScore()
    {
        return scoreWorth;
    }

    public override string Type()
    {
        return "Gargoyle";
    }

    public string ShowCalculation()
    {
        var description = "Point Value: " + scoreWorth;
        return description;
    }
    
}
