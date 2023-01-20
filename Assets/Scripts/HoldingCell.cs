using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingCell : Singleton<HoldingCell>
{
    public bool over, holding;


    private void OnMouseOver()
    {
        over = true;
    }

    private void OnMouseExit()
    {
        over = false;
    }
}
