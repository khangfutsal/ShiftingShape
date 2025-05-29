using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
namespace ShiftingShape
{
    public class CameraMoving : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private Camera camera;
        [SerializeField] private Vector3 vCamera;
        

        private void Start()
        {
            player = GameObject.Find("Player").GetComponent<Player>();
            camera = GetComponent<Camera>();
            vCamera = camera.transform.position;
        }

        private void Update()
        {
            if (GameManager.Ins.GetGameState() == GameState.StartGame)
            {
                MovePlayer();
            }
        }

        private void MovePlayer()
        {
            if (camera != null)
            {
                BaseShape baseShape = player.GetCurrentShape();
                float pivotZ = (baseShape.transform.localPosition.z) - 2.22f;
                Vector3 targetPlayer = new Vector3(vCamera.x, baseShape.transform.localPosition.y + vCamera.y, pivotZ);
                camera.transform.position = targetPlayer;
            }
        }
    }
}

