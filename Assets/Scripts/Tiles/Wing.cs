using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;

public class Wing : Wasteland
{
    public Wing(Transform parentTransform, Vector3 pos)
    {
        ConstructorHelper(parentTransform, pos, "ChurchWing");
    }

    public override string Type()
    {
        return "ChurchWing";
    }
    
}
