using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Khang
{
    public class BoatShape : BaseShape
    {
        [SerializeField] private Transform _sphereTf;
        [SerializeField] private Transform _currentSkin;

        [SerializeField] private Vector3 vScaleTransitionUp = Vector3.one;
        [SerializeField] private Vector3 vScaleTransitionDown = new Vector3(0.5f, 0.5f, 0.5f);

        [SerializeField] private Transform groundTf;

        private void Awake()
        {

            Rigidbody = GetComponent<Rigidbody>();

            _currentSkin = GetComponentInChildren<CurrentSkinShape>().transform;

            _sphereTf = transform.Find("Sphere");
            groundTf = transform.Find("CheckGround");
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

            if (IsGrounded())
            {
                Move();
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
        }

        private void OnCollisionStay(Collision collision)
        {
            if ((shapeData.InteractLayers.value & (1 << collision.gameObject.layer)) != 0)
            {
                Debug.Log("Interact layers Boat : " + collision.gameObject.name);
                InLayersInteract = true;
            }
            else InLayersInteract = false;
        }

        public bool IsGrounded()
        {
            return Physics.Raycast(groundTf.position, Vector3.down, 0.5f, shapeData.InteractLayers);
        }

        public override IEnumerator DisappearShape(float time)
        {

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

