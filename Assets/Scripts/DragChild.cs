using System;
using UnityEngine;

/// <summary>
/// This class allows colliders to communicate with their parent object
/// </summary>
public class DragChild : MonoBehaviour
{
    public IDragParent parent;

    private void OnMouseUp()
    {
        GameManager.Instance.selected = gameObject;
        parent.OnMouseUp();
    }
    private void OnMouseDown() { 
        GameManager.Instance.selected = gameObject;
        parent.OnMouseDown(); }
    private void OnMouseDrag() { 
        GameManager.Instance.selected = gameObject;
        parent.OnMouseDrag(); }
    
    
}