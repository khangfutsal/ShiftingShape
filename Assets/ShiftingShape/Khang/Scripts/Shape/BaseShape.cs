using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Khang
{
    public abstract class BaseShape : MonoBehaviour
    {
        public ShapeData shapeData;

        public Rigidbody Rigidbody;

        public bool InLayersInteract;
        [SerializeField] protected bool isDisable;
        protected bool isPlayer;


        public void SetIsPlayer(bool _isPlayer) => isPlayer = _isPlayer;

        public abstract void Move();
        public abstract IEnumerator DisappearShape(float time);
        public abstract IEnumerator AppearShape(float time);
        public abstract void Disable();
        public abstract void Enable();

    }

}
