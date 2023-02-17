using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;

public class MouseOverTile: MonoBehaviour
{
    [SerializeField] public ITile Tile;
    public void OnMouseOver()
    {
        turnGrey();
        if (Input.GetMouseButton(1))
        {
            GameManager.Instance.scoretip.Hide();
            GameManager.Instance.tooltip.Show(Tile);
        }
        else
        {
            GameManager.Instance.tooltip.Hide();
            GameManager.Instance.scoretip.Show(Tile);
        }
    }
    public void OnMouseExit()
    {
        GameManager.Instance.tooltip.Hide();
        GameManager.Instance.scoretip.Hide();
        turnWhite();
    }

    private void turnGrey()
    {
        Color grey = new Color(0.5f, 0.5f, 0.5f, 1f);
        GridManager.ForEach((int x, int y, ITile tile) => {
            if (tile.GetType().Name == Tile.GetType().Name && tile.GetType().Name != "Wasteland")
            {
                LeanTween.color(tile.TileObject, grey, 0f);
            }
        });
    }
    private void turnWhite()
    {
        GridManager.ForEach((int x, int y, ITile tile) => {
            if (tile.GetType().Name == Tile.GetType().Name && tile.GetType().Name != "Wasteland")
            {
                LeanTween.color(tile.TileObject, Color.white, 0f);
            }
        });
    }

}