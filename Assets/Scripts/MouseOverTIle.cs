using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;

public class MouseOverTile: MonoBehaviour
{
    [SerializeField] public ITile Tile;
    public void OnMouseDown()
    {
        if(!GameManager.Instance.dragging && GridManager.Instance.OverGrid(Tile.Position())){
            GameManager.Instance.Tooltip.Show(Tile);
        }
    }
    public void OnMouseUp()
    {
        GameManager.Instance.Tooltip.Hide();
    }
     public void OnMouseExit()
    {
        GameManager.Instance.Tooltip.Hide();
    }


}