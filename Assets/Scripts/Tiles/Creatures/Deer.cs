using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Deer : Animal
{
    [SerializeField] new protected int scoreWorth = 3;

    public override string GetDescription()
    {
        return $"Reassembles in an empty <b>Adjacent</b> space each turn if able";
    }

    public override Tag[] GetTags()
    {
        return new[] { Tag.Darkness, Tag.Animal };
    }

    public static Rule MoveDeer = new Rule("Move Deer", 10, () =>
    {
        List<Vector2Int> coordinatesList = new List<Vector2Int>();
        List<Vector2Int> interimList = new List<Vector2Int>();
        GridManager.ForEach((int x, int y, Deer deer) =>
        {
            interimList.Clear();
            foreach (Vector2Int dir in Directions.Cardinal)
            {

                ITile tile = GridManager.Instance.GetTile(x + dir.x, y + dir.y);
                if (tile.GetType() == typeof(Wasteland) && GridManager.Instance.Grid.InRange(x + dir.x, y + dir.y) && !coordinatesList.Contains(new Vector2Int(x + dir.x, y + dir.y)))
                {
                    interimList.Add(new Vector2Int(x + dir.x, y + dir.y));
                }

            }

            if (interimList.Count() != 0)
            {
                coordinatesList.Add(interimList.PickRandom());
                GameManager.Instance.DestroyTile(new Vector2Int(x, y));
            }
        });

        foreach (Vector2Int location in coordinatesList)
        {
            GameManager.Instance.PlaceTile("Deer", location);
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