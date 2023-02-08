using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOverTile: MonoBehaviour
{
    [SerializeField] public ITile Tile;
    public void OnMouseOver()
    {
        GameManager.Instance.tooltip.Show(Tile.GetDescription(), Tile.CalculateScore());   
    }
    public void OnMouseExit()
    {
        GameManager.Instance.tooltip.Hide();
    }
}
public class MouseOverTile: MonoBehaviour
{
    [SerializeField] public ITile Tile;
    public void OnMouseOver()
    {
        GameManager.Instance.tooltip.Show(Tile);   
    }
    public void OnMouseExit()
    {
        GameManager.Instance.tooltip.Hide();
    }
}