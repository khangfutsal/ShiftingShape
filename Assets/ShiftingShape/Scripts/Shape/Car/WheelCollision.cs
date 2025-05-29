using System.Collections;
using System.Collections.Generic;
using ShiftingShape;
using UnityEngine;

public class WheelCollision : MonoBehaviour
{
    public Car car;
    public BoxCollider boxcollider;

    private void Awake()
    {
        car = transform.parent.parent.GetComponent<Car>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((car.layerMask.value & (1 << collision.transform.gameObject.layer)) != 0)
        {
            Debug.Log("Hit with Layermask");
            car.isTouchingLayerMask = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if ((car.layerMask.value & (1 << collision.transform.gameObject.layer)) != 0)
        {
            Debug.Log("Hit with Layermask");
            car.isTouchingLayerMask = true;
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if ((car.layerMask.value & (1 << collision.transform.gameObject.layer)) != 0)
        {
            Debug.Log("Exit with Layermask");
            car.isTouchingLayerMask = false;
        }
    }
}
