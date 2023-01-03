using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fountain : Wasteland
{
    public Fountain(int x, int y) : base(x, y) { }
 
    public bool Destructible()
    {
        return false;
    }

    public string Type()
    {
        return "FO";
    }
}
