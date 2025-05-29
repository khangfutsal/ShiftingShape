using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ShiftingShape
{
    public class Car : BaseShape
    {
        private Rigidbody rigidbody;
        [SerializeField] private Vector3 vMove;

        public float minX = -30f; // Ngưỡng giới hạn trục X
        public float correctionSpeed = 50f; // Tốc độ giảm giá trị rotation.x

        protected override void Start()
        {
            base.Start();
            rigidbody = GetComponent<Rigidbody>();
            rigidbody.centerOfMass += new Vector3(0, -0.05f, 0);
        }


        private void FixedUpdate()
        {
            if (GameManager.Ins.GetGameState() == GameState.StartGame || GameManager.Ins.GetGameState() == GameState.WinGame)
            {
                Move();
            }
        }

        public void Move()
        {
            if (isTouchingLayerMask)
                vMove = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, shapeSO.shapeData.speedExpected);

            else
                vMove = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, shapeSO.shapeData.speedUnexpected);

            rigidbody.velocity = vMove;

        }

        public override void TransitionShape()
        {
            rigidbody.automaticCenterOfMass = true;
        }

        public override void CompleteShape()
        {
            rigidbody.automaticCenterOfMass = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (GameManager.Ins.GetGameState() == GameState.WinGame) return;
            if (other.gameObject.CompareTag("Finish") && playerControl)
            {
                GameManager.Ins.FinishGame();
            }
            if ((layerMask.value & (1 << other.transform.gameObject.layer)) != 0)
            {
                isTouchingLayerMask = true;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (GameManager.Ins.GetGameState() == GameState.WinGame) return;
            if (other.gameObject.CompareTag("Finish") && playerControl)
            {
                GameManager.Ins.FinishGame();
            }
            if ((layerMask.value & (1 << other.transform.gameObject.layer)) != 0)
            {
                isTouchingLayerMask = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (GameManager.Ins.GetGameState() == GameState.WinGame) return;
            if ((layerMask.value & (1 << other.transform.gameObject.layer)) != 0)
            {
                isTouchingLayerMask = false;
            }
        }

    }
}

