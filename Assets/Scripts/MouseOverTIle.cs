using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;

public class MouseOverTile: MonoBehaviour
{
    [SerializeField] public ITile Tile;
    public void OnMouseOver()
    {
        if (GameManager.Instance.dragging)
        {
            GameManager.Instance.tooltip.Hide();
        }
        else 
        {
            GameManager.Instance.tooltip.Show(Tile);
        }
    }
    public void OnMouseExit()
    {
        GameManager.Instance.tooltip.Hide();
        turnWhite();
    }
    public void OnMouseUp()
    {
        turnWhite();
    }

    private void turnGrey()
    {
        Color grey = new Color(0.5f, 0.5f, 0.5f, 1f);
        GridManager.ForEach((int x, int y, ITile tile) =>
        {
            if (tile.GetType() == typeof(Wasteland)) return;
            if (!(Tile.HighlightPredicate(tile) || tile.HighlightPredicate(Tile)))
            {
                LeanTween.color(tile.TileObject, grey, 0f);
            }
        });
    }
    private void turnWhite()
    {
        GridManager.ForEach((int x, int y, ITile tile) => {
            if (tile.GetType().Name != "Wasteland")
            {
                LeanTween.color(tile.TileObject, Color.white, 0f);
            }
        });
    }

}