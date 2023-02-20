using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Scripts;
using UnityEngine.SocialPlatforms.Impl;

public class Golem : Creature
{
    [SerializeField] protected int scoreWorth = 0;

    private List<Tag> newTags = new List<Tag>();
    
    public override string GetDescription()
    {
        return $"{scoreWorth}pts - When Placed: Gains Tags of all adjacent Tiles";
    }
    
    public override Tag[] GetTags()
    {
        return newTags.ToArray();
    }

    protected override Score CalculateBaseScore() 
    {
        foreach (Vector2Int dir in Directions.Cardinal)
        {
            ITile tile = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y);
            if (!(tile.GetType() == typeof(Wasteland)))
            {
                Tag[] neighborTags = tile.GetTags();
                foreach(Tag tag in neighborTags){
                    if (!newTags.Contains(tag)){
                        newTags.Add(tag);
                    }
                }
            }
        }
        return new Score(scoreWorth);
    }
    public Golem(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }

}
