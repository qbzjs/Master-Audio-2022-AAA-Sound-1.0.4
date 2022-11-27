using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class River : ITile
{
    public GameObject TileObject { get; set; }
    public int xPos { get; set; }
    public int yPos { get; set; }
    [SerializeField] private int scoreWorth = 10;

    private bool blood = false;

    public River(int x, int y)
    {
        this.xPos = x;
        this.yPos = y;
    }

    public River(Sprite[] options, Transform parentTransform, Vector3 pos)
    {
        this.TileObject = new GameObject("Tile");
        int i = Random.Range(0, options.Length);
        this.TileObject.AddComponent<SpriteRenderer>();
        this.TileObject.transform.position = pos;
        this.TileObject.transform.rotation = Quaternion.identity;
        this.TileObject.transform.parent = parentTransform;
        this.TileObject.GetComponent<SpriteRenderer>().sprite = options[i];
    }

    public bool Destructible()
    {
        return true;
    }

    public int CalculateScore()
    {
        if (GridManager.Instance.GetTile(xPos + 1, yPos + 1).Type() == 'B' || GridManager.Instance.GetTile(xPos + 1, yPos + 1).Type() == 'F')
        {
            blood = true;
        }

        if (GridManager.Instance.GetTile(xPos + 1, yPos - 1).Type() == 'B' || GridManager.Instance.GetTile(xPos + 1, yPos - 1).Type() == 'F')
        {
            blood = true;
        }

        if (GridManager.Instance.GetTile(xPos - 1, yPos + 1).Type() == 'B' || GridManager.Instance.GetTile(xPos - 1, yPos + 1).Type() == 'F')
        {
            blood = true;
        }

        if (GridManager.Instance.GetTile(xPos - 1, yPos - 1).Type() == 'B' || GridManager.Instance.GetTile(xPos - 1, yPos - 1).Type() == 'F')
        {
            blood = true;
        }

        if (blood)
        {
            return scoreWorth;
        }

        return 0;
    }

    public char Type()
    {
        if (blood)
        {
            return 'B';
        }
        return 'R';
    }
}