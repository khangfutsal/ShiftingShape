using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Khang
{
    public class GliderShape : BaseShape
    {

        [SerializeField] private Transform _sphereTf;
        [SerializeField] private Transform _currentSkin;

        [SerializeField] private Vector3 vScaleTransitionUp = Vector3.one;
        [SerializeField] private Vector3 vScaleTransitionDown = new Vector3(0.5f, 0.5f, 0.5f);

        [SerializeField] private float gravity = 2f;
        [SerializeField] private float speedDrop = 1f;

        [SerializeField] private LayerMask _layerWall;
        [SerializeField] private Transform wallTf;
        [SerializeField] private float distance = 0.5f;

        [SerializeField] private Transform groundTf;

        private void Awake()
        {

            Rigidbody = GetComponent<Rigidbody>();

            _currentSkin = GetComponentInChildren<CurrentSkinShape>().transform;

            groundTf = transform.Find("CheckGround");
            _sphereTf = transform.Find("Sphere");
            wallTf = transform.Find("ForwardSensor");
        }


        private void Update()
        {
            UpdateBehaivour();
        }

        public void UpdateBehaivour()
        {
            if (isDisable) return;
            Debug.Log(Rigidbody.velocity);
            InLayersInteract = IsGrounded();
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
            Move();
        }

        public override void Move()
        {
            Vector3 velocity = Rigidbody.velocity;

            if (!CheckFrontOfWall())
            {
                // Xử lý tốc độ Z
                if (!InLayersInteract)
                {
                    velocity.z = shapeData.SpeedDesired;
                    velocity.y += -gravity * speedDrop * Time.fixedDeltaTime;
                }
                else
                {
                    velocity.z = shapeData.SpeedUndesired;
                }
            }
            else velocity.z = 0;

            // Gán lại velocity
            Rigidbody.velocity = velocity;
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

        public bool IsGrounded()
        {
            return Physics.Raycast(groundTf.position, Vector3.down, 0.5f, shapeData.InteractLayers);
        }

        public bool CheckFrontOfWall()
        {
            return Physics.Raycast(wallTf.position, Vector3.forward, distance, _layerWall);
        }


        public override IEnumerator DisappearShape(float time)
        {
            _sphereTf.gameObject.SetActive(true);
            _sphereTf.localScale = vScaleTransitionDown;

            _currentSkin.DOScale(vScaleTransitionDown, 0.5f);

            _sphereTf.DOScale(vScaleTransitionUp, 0.5f);
           
            yield return new WaitForSeconds(0.25f);
            _sphereTf.gameObject.SetActive(false);
            this.gameObject.SetActive(false);

            yield return null;


        }

        public override IEnumerator AppearShape(float time)
        {
            this.gameObject.SetActive(true);
            DefaultProperties();

            _sphereTf.gameObject.SetActive(true);
            _sphereTf.localScale = vScaleTransitionUp;
            _currentSkin.localScale = vScaleTransitionDown;
            _currentSkin.DOScale(vScaleTransitionUp, 0.5f);
            yield return new WaitForSeconds(0.25f);
            _sphereTf.gameObject.SetActive(false);
            yield return null;
        }

        public void DefaultProperties()
        {
            InLayersInteract = false;
        }

        public override void Disable() => isDisable = true;

        public override void Enable() => isDisable = false;
    }

}
