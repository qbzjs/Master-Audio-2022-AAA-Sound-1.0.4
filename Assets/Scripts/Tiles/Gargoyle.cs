using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class Gargoyle : Wasteland
{
    [SerializeField] new protected int scoreWorth = 2;
    public static string Description
    {
        get
        {
            return "A grotesque face in the dark";
        }
    }

    public static string PointDescription
    {
        get
        {
            return "2";
        }
    }
    public Gargoyle(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }
    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }
    
}
