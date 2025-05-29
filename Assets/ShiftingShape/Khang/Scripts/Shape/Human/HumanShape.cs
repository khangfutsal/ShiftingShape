using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;
using Unity.VisualScripting;
namespace Khang
{
    public class HumanShape : BaseShape
    {
        [SerializeField] private HumanShapeAnimator _humanShapeAnimator;

        [SerializeField] private Transform _sphereTf;
        [SerializeField] private Transform _currentSkin;

        [SerializeField] private const float _threshHoldVelocityY = 1f;

        [SerializeField] private Vector3 vScaleTransitionUp = Vector3.one;
        [SerializeField] private Vector3 vScaleTransitionDown = new Vector3(0.5f, 0.5f, 0.5f);

        [SerializeField] private Transform groundTf;

        [SerializeField] private float climbSpeed;
        [SerializeField] private LayerMask layerMaskWall;
        [SerializeField] private Transform wallTf;
        [SerializeField] private float rayDistanceWall = 0.5f;
        [SerializeField] private bool interactWall;


        private void Awake()
        {
            _humanShapeAnimator = GetComponent<HumanShapeAnimator>();

            Rigidbody = GetComponent<Rigidbody>();

            _currentSkin = GetComponentInChildren<CurrentSkinShape>().transform;

            _sphereTf = transform.Find("Sphere");
            groundTf = transform.Find("CheckGround");
            wallTf = transform.Find("CheckWall");
        }

        private void Start()
        {
            _sphereTf.gameObject.SetActive(false);
        }

        private void Update()
        {
            UpdateBehaivour();
        }

        public void UpdateBehaivour()
        {
            if (isDisable) return;
            CheckGameState();
            InLayersInteract = IsGrounded();

            void CheckGameState()
            {
                if (GameController.Ins.GetGameResult() && isPlayer) isDisable = true;
                if (GameController.Ins.GetGameResult() && !isPlayer) isDisable = true;
            }
        }


        private void FixedUpdate()
        {
            if (isDisable || GameController.Ins.GameManager.GetGameState() == GameState.Pause) return;
            FixedUpdateBehaviour();
        }

        public void FixedUpdateBehaviour()
        {
            if (interactWall)   // Dang climbing
            {
                Climb();
            }
            else
            {
                if (IsGrounded())
                {
                    Move();
                }
                else _humanShapeAnimator.PlayAnim("Falling");
            }


        }

        public override void Move()
        {

            if (InLayersInteract)
            {
                Vector3 vMove = new Vector3(0, 0, shapeData.SpeedDesired);
                Rigidbody.velocity = vMove;


            }

            else
            {
                Vector3 vMove = new Vector3(0, 0, shapeData.SpeedUndesired);
                Rigidbody.velocity = vMove;
            }
            _humanShapeAnimator.PlayAnim("Running");

        }

        public void Climb()
        {
            Vector3 vClimb = new Vector3(0, climbSpeed, 0.5f);
            Rigidbody.velocity = vClimb;

            _humanShapeAnimator.PlayAnim("Climbing");
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Finish") && !isPlayer && !GameController.Ins.GetGameResult())
            {
                GameController.Ins.GameManager.SetGameState(GameState.Lose);
                GameController.Ins.SetResultGame(true);


            }
            else if (other.CompareTag("Finish") && isPlayer && !GameController.Ins.GetGameResult())
            {
                GameController.Ins.GameManager.SetGameState(GameState.Win);
                GameController.Ins.SetResultGame(true);

            }
            if ((layerMaskWall.value & (1 << other.gameObject.layer)) != 0)
            {
                interactWall = true;
            }
        }


        private void OnTriggerExit(Collider other)
        {
            if ((layerMaskWall.value & (1 << other.gameObject.layer)) != 0)
            {
                interactWall = false;
            }
        }

        public bool IsWall()
        {
            return Physics.Raycast(wallTf.position, Vector3.forward, rayDistanceWall, layerMaskWall);
        }

        public bool IsGrounded()
        {
            return Physics.Raycast(groundTf.position, Vector3.down, 0.5f, shapeData.InteractLayers);
        }

        public override IEnumerator DisappearShape(float time)
        {
            _humanShapeAnimator.PlayAnim("Rolling");

            _sphereTf.gameObject.SetActive(true);
            _sphereTf.localScale = vScaleTransitionDown;

            _currentSkin.DOScale(vScaleTransitionDown, 0.5f);

            _sphereTf.DOScale(vScaleTransitionUp, 0.5f);
            yield return new WaitForSeconds(0.25f);
            this.gameObject.SetActive(false);
            _sphereTf.gameObject.SetActive(false);

            yield return null;


        }

        public override IEnumerator AppearShape(float time)
        {
            this.gameObject.SetActive(true);

            _sphereTf.gameObject.SetActive(true);
            _sphereTf.localScale = vScaleTransitionDown;
            _currentSkin.localScale = vScaleTransitionDown;

            _currentSkin.DOScale(vScaleTransitionUp, 0.5f);
            yield return new WaitForSeconds(0.25f);
            _sphereTf.gameObject.SetActive(false);
            yield return null;
        }

        public override void Disable() => isDisable = true;


        public override void Enable() => isDisable = false;

    }

}
