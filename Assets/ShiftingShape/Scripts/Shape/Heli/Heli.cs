using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
namespace ShiftingShape
{
    public class Heli : BaseShape
    {
        private Rigidbody rigidbody;
        [SerializeField] private Vector3 vMove;

        [SerializeField] private float disRaycast;
        [SerializeField] private Transform pointCheckWall;
        [SerializeField] private LayerMask layerCheckWall;

        private bool isFlying;

        [SerializeField] private float timeFly;
        [SerializeField] private float limitTimeFly;
        [SerializeField] private float threshHold;

        [SerializeField] private float pushUpSpeed;

        protected override void Start()
        {
            base.Start();
            rigidbody = GetComponent<Rigidbody>();
            InActiveGravity();
            rigidbody.centerOfMass += new Vector3(0, -0.02f, 0);

        }

        private void Update()
        {
            DetectedWall();

        }


        private void DetectedWall()
        {
            var checkWall = IsFrontOfWall();
            if (checkWall)
            {
                Vector3 upwardForce = new Vector3(0, pushUpSpeed, 0);
                rigidbody.AddForce(upwardForce * Time.deltaTime, ForceMode.Acceleration);
                isFlying = true;
            }
            else
            {
                Vector3 velocity = rigidbody.velocity;
                velocity.y = Mathf.MoveTowards(velocity.y, 0, Time.deltaTime * 10f);
                rigidbody.velocity = velocity;
                isFlying = false;
            }
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
            if (!isFlying)
            {
                vMove = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, shapeSO.shapeData.speedExpected);
                rigidbody.velocity = vMove;
            }
        }

        public bool IsFrontOfWall()
        {

            if (Physics.Raycast(pointCheckWall.position, pointCheckWall.TransformDirection(Vector3.forward), out RaycastHit hit, disRaycast, layerCheckWall))
            {
                Debug.Log("Did Hit : " + hit.collider.gameObject.name);
                return true;
            }
            return false;
        }

        public void InActiveGravity()
        {
            rigidbody.useGravity = false;
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
        }

    }
}

