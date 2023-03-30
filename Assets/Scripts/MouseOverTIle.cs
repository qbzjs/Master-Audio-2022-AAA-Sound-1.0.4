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
    }

}