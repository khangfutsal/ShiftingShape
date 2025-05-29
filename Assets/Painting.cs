using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Painting : MonoBehaviour
{
    public static Painting _ins;
    private Mesh mesh;
    public float minDistance = 1f;
    public float lineThickness;
    Vector3 lastMousePosition;
    private void Awake()
    {
        _ins = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            mesh = new Mesh();
            Vector3[] vertices = new Vector3[4];
            Vector2[] uv = new Vector2[4];
            int[] triangles = new int[6];

            vertices[0] = GetCurrentMousePos();
            vertices[1] = GetCurrentMousePos();
            vertices[2] = GetCurrentMousePos();
            vertices[3] = GetCurrentMousePos();

            uv[0] = Vector2.zero;
            uv[1] = Vector2.zero;
            uv[2] = Vector2.zero;
            uv[3] = Vector2.zero;

            triangles[0] = 0;
            triangles[1] = 3;
            triangles[2] = 1;

            triangles[3] = 1;
            triangles[4] = 3;
            triangles[5] = 2;

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;
            mesh.MarkDynamic();

            GetComponent<MeshFilter>().mesh = mesh;

            lastMousePosition = GetCurrentMousePos();
        }
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            
            if(Vector3.Distance(GetCurrentMousePos(), lastMousePosition) > minDistance )
            {
                Vector3[] vertices = new Vector3[mesh.vertices.Length + 2];
                Vector2[] uv = new Vector2[mesh.uv.Length + 2];
                int[] triangles = new int[mesh.triangles.Length + 6];

                mesh.vertices.CopyTo(vertices, 0);
                mesh.uv.CopyTo(uv, 0);
                mesh.triangles.CopyTo(triangles, 0);

                int vIndex = vertices.Length - 4;
                int vIndex0 = vIndex + 0;
                int vIndex1 = vIndex + 1;
                int vIndex2 = vIndex + 2;
                int vIndex3 = vIndex + 3;

                Vector3 mouseForwardVector = (GetCurrentMousePos() - lastMousePosition).normalized;
                Vector3 normal2D = new Vector3(0, 0, -1f);
                Vector3 newVerxUp = GetCurrentMousePos() + Vector3.Cross(mouseForwardVector, normal2D) * lineThickness;
                Vector3 newVerxDown = GetCurrentMousePos() + Vector3.Cross(mouseForwardVector, normal2D * -1f) * lineThickness;

                vertices[vIndex2] = newVerxUp;
                vertices[vIndex3] = newVerxDown;

                uv[vIndex2] = Vector2.zero;
                uv[vIndex3] = Vector2.zero;

                int tIndex = triangles.Length - 6;

                triangles[tIndex + 0] = vIndex0;
                triangles[tIndex + 1] = vIndex2;
                triangles[tIndex + 2] = vIndex1;

                triangles[tIndex + 3] = vIndex1;
                triangles[tIndex + 4] = vIndex2;
                triangles[tIndex + 5] = vIndex3;

                mesh.vertices = vertices;
                mesh.uv = uv;
                mesh.triangles = triangles;

                lastMousePosition = GetCurrentMousePos();
            }
          
        }

    }

    public Vector3 GetCurrentMousePos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane + 1f; 
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
