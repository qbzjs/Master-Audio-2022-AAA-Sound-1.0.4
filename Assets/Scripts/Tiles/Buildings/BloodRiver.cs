using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts;
using Unity.VisualScripting;
using UnityEngine;

public class BloodRiver : Building
{
    private bool blood = false;
    public override bool HighlightPredicate(ITile otherTile)
    {
        return base.HighlightPredicate(otherTile) || otherTile.GetTags().Contains(Tag.Blood);
    }
    public override string GetDescription()
    {
        return "Adjacent tiles become Bloody (Double score)";
    }

    public override Tag[] GetTags()
    {
        return new []{Tag.Blood};
    }
    
    public BloodRiver(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }
}