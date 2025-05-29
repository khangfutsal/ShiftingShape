using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ShiftingShape
{
    public class Human : BaseShape
    {
        private Rigidbody rigidbody;
        [SerializeField] private Animator anim;

        [SerializeField] private Vector3 vMove;
        [SerializeField] private Transform pointCheckClimb;
        [SerializeField] private float disRaycast;
        [SerializeField] private float climbSpeed;

        [SerializeField] private bool canClimb;

        protected override void Start()
        {
            base.Start();
            rigidbody = GetComponent<Rigidbody>();
            rigidbody.centerOfMass += new Vector3(0, -0.05f, 0);

        }

        private void Update()
        {
            if (GameManager.Ins.GetGameState() == GameState.StartGame || GameManager.Ins.GetGameState() == GameState.WinGame)
            {
                CheckClimbRay();
                Move();
            }
        }


        public void CheckClimbRay()
        {
            if (Physics.Raycast(pointCheckClimb.position, pointCheckClimb.TransformDirection(Vector3.forward), out RaycastHit hit, disRaycast))
            {
                if (hit.collider.CompareTag("Slope"))
                {
                    anim.SetBool("IsClimb", true);
                    rigidbody.useGravity = false;
                    canClimb = true;
                }
            }
            else
            {
                anim.SetBool("IsClimb", false);
                rigidbody.useGravity = true;
                canClimb = false;
            }
        }

        public void Move()
        {
            if (canClimb)
            {
                vMove = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, 0);
                Vector3 upwardForce = new Vector3(0, climbSpeed, 0);
                rigidbody.AddForce(upwardForce * Time.deltaTime);
                anim.SetBool("IsClimb",true);
            }
            else if (isTouchingLayerMask)
            {
                anim.SetTrigger("Run");
                anim.SetBool("IsClimb", false);
                vMove = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, shapeSO.shapeData.speedExpected);
            }

            else
            {
                anim.SetTrigger("Run");
                anim.SetBool("IsClimb", false);
                vMove = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, shapeSO.shapeData.speedUnexpected);
            }

            rigidbody.velocity = vMove;

        }

        public override void TransitionShape()
        {
            //anim.SetTrigger("Transition");
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
            if ((canClimb)) return;
            if ((layerMask.value & (1 << collision.transform.gameObject.layer)) != 0)
            {
                isTouchingLayerMask = true;
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            if (GameManager.Ins.GetGameState() == GameState.WinGame) return;
            if ((canClimb)) return;
            if ((layerMask.value & (1 << collision.transform.gameObject.layer)) != 0)
            {
                isTouchingLayerMask = true;
            }

        }

        private void OnCollisionExit(Collision collision)
        {
            if (GameManager.Ins.GetGameState() == GameState.WinGame) return;
            if ((canClimb)) return;
            if ((layerMask.value & (1 << collision.transform.gameObject.layer)) != 0)
            {
                isTouchingLayerMask = false;
            }
        }

        public override void CompleteShape()
        {
            rigidbody.automaticCenterOfMass = false;
        }
    }
}

