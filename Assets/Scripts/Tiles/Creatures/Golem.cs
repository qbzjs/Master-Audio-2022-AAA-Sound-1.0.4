using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Scripts;
using Unity.VisualScripting;
using UnityEngine.SocialPlatforms.Impl;

public class Golem : Creature
{
    [SerializeField] protected int scoreWorth = 0;

    private List<Tag> newTags = new List<Tag>();
    
    public override string GetDescription()
    {
        return $"When Placed: Gains tags of all <b>Adjacent</b> tiles, <b><color=\"red\">+1</color></b> for each tag";
    }
    
    public override Tag[] GetTags()
    {
        return newTags.ToArray();
    }

    protected override Score CalculateBaseScore() 
    {
        return new Score(newTags.Count - 1);
    }

    public override void WhenPlaced()
    {
        foreach (Vector2Int dir in Directions.Cardinal)
        {
            ITile tile = GridManager.Instance.GetTile(xPos + dir.x, yPos + dir.y);
            if (!(tile.GetType() == typeof(Wasteland)))
            {
                Tag[] neighborTags = tile.GetTags();
                foreach(Tag tag in neighborTags){
                    if (!newTags.Contains(tag)){
                        TweenManager.Instance.Callout($"Golem gains {tag.ToString()}", Position());
                        newTags.Add(tag);
                    }
                }
            }
        }
    }

    public Golem(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {
        newTags = new[] { Tag.Null }.ToList();
    }

}
