using DG.Tweening;
using Khang;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace Khang
{
    public class HeliShape : BaseShape
    {
        [SerializeField] private Transform _sphereTf;
        [SerializeField] private Transform _currentSkin;

        [SerializeField] private Transform _sensorWallTf;
        [SerializeField] private float distanceRay = 3f;
        [SerializeField] private float limitHeight = 10f;

        [SerializeField] private float force = 3f;


        [SerializeField] private Vector3 vScaleTransitionUp = Vector3.one;
        [SerializeField] private Vector3 vScaleTransitionDown = new Vector3(0.5f, 0.5f, 0.5f);


        private void Awake()
        {

            Rigidbody = GetComponent<Rigidbody>();

            _currentSkin = GetComponentInChildren<CurrentSkinShape>().transform;

            _sphereTf = transform.Find("Sphere");
            _sensorWallTf = transform.Find("ForwardSensor");
        }


        private void Update()
        {
            UpdateBehaivour();
        }

        public void UpdateBehaivour()
        {
            if (isDisable) return;
            Debug.Log(Rigidbody.velocity);
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
            velocity.z = shapeData.SpeedDesired;

            if (!CheckFrontOfWall())
            {
                velocity.z = shapeData.SpeedDesired;
            }
            else
            {
                velocity.z = 0f;
            }

            if (CheckFrontOfWall() && Rigidbody.position.y >= limitHeight)
            {
                limitHeight += 5f;
            }

            if (Rigidbody.position.y < limitHeight)
            {
                velocity.y += force * Time.fixedDeltaTime;
            }
            else if (Rigidbody.position.y >= limitHeight)
            {
                velocity.y = 0f;
                Vector3 pos = Rigidbody.position;
                Rigidbody.position = pos;
            }

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

        public bool CheckFrontOfWall()
        {
            return Physics.Raycast(_sensorWallTf.position, Vector3.forward, distanceRay, shapeData.InteractLayers);
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
            if (Rigidbody.velocity.y >= 0)
            {
                limitHeight = 10f;
            }
            else if (Rigidbody.velocity.y < 0)
            {
                limitHeight = Rigidbody.position.y;
            }

        }

        public override void Disable() => isDisable = true;

        public override void Enable() => isDisable = false;

    }

}
