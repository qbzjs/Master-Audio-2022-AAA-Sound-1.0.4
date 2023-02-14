using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;

public class MouseOverTile: MonoBehaviour
{
    [SerializeField] public ITile Tile;
    public void OnMouseOver()
    {
        if (Input.GetMouseButton(1))
        {
            GameManager.Instance.scoretip.Hide();
            GameManager.Instance.tooltip.Show(Tile);
        }
   /*     else if (Input.GetMouseButton(0))
        {
            Vector3 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (GridManager.Instance.OverGrid(temp)){
                Tile.TileObject.GetComponent<SpriteRenderer>().color = new Color(0f,181f/255f,1f);
            }
        } */
        else
        {
            GameManager.Instance.tooltip.Hide();
            GameManager.Instance.scoretip.Show(Tile);
      //      Tile.TileObject.GetComponent<SpriteRenderer>().color = new Color(255f,255f,255f);
        }
    }
    public void OnMouseExit()
    {
        GameManager.Instance.tooltip.Hide();
        GameManager.Instance.scoretip.Hide();
    }
}