using UnityEngine;
using UnityEngine.Serialization;

public class CenterTarget : MonoBehaviour
{
    public float radius = 5f;
    public float height = 10f;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        var position = transform.position;
        Gizmos.DrawWireSphere(position, radius);
        Gizmos.DrawLine(new Vector3(position.x, position.y - height / 2, position.z),
            new Vector3(position.x, position.y + height / 2, position.z));
    }
}
