using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Khang
{
    public class ItemDisplay : MonoBehaviour
    {
        public float speed = 50f;

        public void Rotation(float delta)
        {
            Vector3 euler = transform.localEulerAngles;
            euler.y += -delta;
            transform.localEulerAngles = euler;
        }

        private void OnDisable()
        {
            transform.localEulerAngles = new Vector3(180, 0, 180);

        }
    }
}

