using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ShiftingShape
{
    public class Boat : BaseShape
    {
        private Rigidbody rigidbody;
        [SerializeField] private Vector3 vMove;

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

        public override void CompleteShape()
        {
            rigidbody.automaticCenterOfMass = false;
        }

        public override void TransitionShape()
        {
            rigidbody.automaticCenterOfMass = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (GameManager.Ins.GetGameState() == GameState.WinGame) return;
            if (other.gameObject.CompareTag("Finish") && playerControl)
            {
                GameManager.Ins.FinishGame();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (GameManager.Ins.GetGameState() == GameState.WinGame) return;
            if ((layerMask.value & (1 << collision.transform.gameObject.layer)) != 0)
            {
                Debug.Log("Hit with Layermask");
                isTouchingLayerMask = true;
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            if (GameManager.Ins.GetGameState() == GameState.WinGame) return;
            if ((layerMask.value & (1 << collision.transform.gameObject.layer)) != 0)
            {
                Debug.Log("Hit with Layermask");
                isTouchingLayerMask = true;
            }

        }

        private void OnCollisionExit(Collision collision)
        {
            if (GameManager.Ins.GetGameState() == GameState.WinGame) return;
            if ((layerMask.value & (1 << collision.transform.gameObject.layer)) != 0)
            {
                Debug.Log("Exit with Layermask");
                isTouchingLayerMask = false;
            }
        }
    }
}

