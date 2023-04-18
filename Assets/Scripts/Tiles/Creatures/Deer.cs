using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Deer : Animal
{
    [SerializeField] new protected int scoreWorth = 3;

    public override string GetDescription()
    {
        return $"{scoreWorth}pts - Just a normal \"Deer\", moves to a new empty tile every turn if able";
    }

    public override Tag[] GetTags()
    {
        return new[] { Tag.Darkness, Tag.Animal };
    }

    public static Rule MoveDeer = new Rule("Move Deer", 10, () =>
    {
        int deerCount = 0;
        List<Vector2Int> coordinatesList = new List<Vector2Int>();
        GridManager.ForEach((int x, int y, Deer deer) =>
        {
            deerCount++;
            GameManager.Instance.DestroyTile(new Vector2Int(x, y));
        });
        GridManager.ForEach((int x, int y, ITile tile) =>
        {
            if (tile.GetType() == typeof(Wasteland))
            {
                coordinatesList.Add(new Vector2Int(x, y));
            }
        });

        for(int i = 0; i < deerCount; i++)
        {
            Vector2Int coordinates = coordinatesList.PickRandom();
            GridManager.Instance.PlaceTile("Deer", coordinates);
            coordinatesList.Remove(coordinates);
        }
    });

    public Deer(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }

    protected override Score CalculateBaseScore()
    {
        return new Score(scoreWorth);
    }

    public override void WhenPlaced()
    {
        GameManager.Instance.AddRule(MoveDeer);
    }
}