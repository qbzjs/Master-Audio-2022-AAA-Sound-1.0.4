using System;
using UnityEngine;

/// <summary>
/// This class allows colliders to communicate with their parent object
/// </summary>
public class DragChild : MonoBehaviour
{
    public IDragParent parent;

    private void OnMouseUp() { parent.OnMouseUp(); }
    private void OnMouseDown() { parent.OnMouseDown(); }
  //  private void OnMouseDrag() { parent.OnMouseDrag(); }
}