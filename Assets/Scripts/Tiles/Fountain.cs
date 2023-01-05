using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fountain : Wasteland
{
    public bool Destructible()
    {
        return false;
    }

    public string Type()
    {
        return "FO";
    }
}
