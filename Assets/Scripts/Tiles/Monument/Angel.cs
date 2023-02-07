using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Angel : Monument
{
    bool blood = false;

    [SerializeField] new protected int scoreWorth = -3;

    public override string GetDescription()
    {
        return "<i>A holy object, don't get it bloody</i>";
    }

    public Angel(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }
    protected override Score CalculateBaseScore()
    {
        foreach (Vector2Int dir in Directions.Cardinal)
        {
            if (blood) continue; //can't turn if already made of blood
            Wasteland tile = (Wasteland)GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y);
            if (tile.Type == "Blood")
            {
                blood = true;
                Type = "Blood";
                //Art
                scoreWorth = 6;
            }
        }
        return new Score(scoreWorth);
    }

}
