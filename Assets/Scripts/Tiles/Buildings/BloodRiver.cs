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
        return "x2 each adjacent tile";
    }

    public override Tag[] GetTags()
    {
        return new []{Tag.Blood, Tag.Building};
    }

    public override void WhenPlaced()
    {
    }

    public BloodRiver(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        ParticleManager.Instance.InstantiateBloodGlow(TileObject.transform);
    }
}