using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossVisual : MonoBehaviour
{
    public Vector3 normal2D;
    public Vector3 dir;

    public float lineThickness;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(Vector2.zero, normal2D);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector2.zero, dir);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector2.zero, Vector3.Cross(dir, normal2D) * lineThickness);
       
    }

    public Vector3 GetCurrentMousePos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane + 1f;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
