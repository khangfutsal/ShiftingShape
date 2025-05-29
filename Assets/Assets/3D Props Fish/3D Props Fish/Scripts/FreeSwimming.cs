using System.Linq;
using UnityEngine;

public enum ObjectType
{
    Fish,
    Paper,
    Ship
}

public class FreeSwimming : MonoBehaviour
{
    [Header(" Elements ")]
    private CenterTarget centerTarget;

    private Collider[] _obstacles;

    [Header(" Settings ")]
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float minSpeed = 1f;
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private float avoidanceDistance = 5f;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float targetProximityThreshold = 1f;
    [SerializeField] private ObjectType objectType;

    private Vector3 _targetPosition;

    void Start()
    {
        switch (objectType)
        {
            case ObjectType.Fish:
                centerTarget = GameObject.FindGameObjectWithTag("FishCenter").GetComponent<CenterTarget>();
                break;

            case ObjectType.Paper:
                centerTarget = GameObject.FindGameObjectWithTag("DoorCenter").GetComponent<CenterTarget>();
                break;

            case ObjectType.Ship:
                centerTarget = GameObject.FindGameObjectWithTag("ShipCenter").GetComponent<CenterTarget>();
                break;
        }

        SetRandomTargetPosition();
    }

    void Update()
    {
        if (centerTarget == null)
        {
            Debug.LogError("No Component found that is CenterTarget");
            return;
        }

        MoveTowardsTarget();
    }

    private void SetRandomTargetPosition()
    {
        var angle = Random.Range(0f, 2f * Mathf.PI);
        var radius = Random.Range(0f, centerTarget.radius);
        var x = radius * Mathf.Cos(angle);
        var z = radius * Mathf.Sin(angle);
        var y = Random.Range(-centerTarget.height / 2, centerTarget.height / 2);

        _targetPosition = centerTarget.transform.position + new Vector3(x, y, z);
    }

    private void MoveTowardsTarget()
    {
        _obstacles = Physics.OverlapSphere(transform.position, avoidanceDistance, obstacleLayer);

        if (_obstacles.Length > 0)
        {
            Vector3 avoidanceDirection = Vector3.zero;

            foreach (var obstacle in _obstacles)
            {
                Vector3 directionAway = (transform.position - obstacle.transform.position).normalized;
                avoidanceDirection += directionAway;
            }

            avoidanceDirection.Normalize(); // Giữ hướng chính xác thay vì cộng dồn lớn hơn 1

            // Quay dần về hướng tránh vật thể
            var avoidanceRotation = Quaternion.LookRotation(avoidanceDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, avoidanceRotation, rotationSpeed * Time.deltaTime);

            // Dùng Lerp để điều chỉnh tốc độ né tránh
            float speed = Mathf.Lerp(minSpeed, maxSpeed, 0.5f);
            transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        }

        else
        {
            Vector3 direction = _targetPosition - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            float distanceToTarget = direction.magnitude;
            float speedMultiplier = Mathf.Clamp01(distanceToTarget / avoidanceDistance);
            float speed = Mathf.Lerp(minSpeed, maxSpeed, speedMultiplier);
            transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        }

        if (Vector3.Distance(transform.position, _targetPosition) <= targetProximityThreshold)
            SetRandomTargetPosition();
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, avoidanceDistance);
    }
}
