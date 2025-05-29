using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Khang
{
    public class CameraMoving : MonoBehaviour
    {
        [SerializeField] private Player player;
        private Vector3 offset;      // khoảng cách cố định giữa camera và player
        private Vector3 initialCamPosition;  // vị trí setup ban đầu
        private Vector3 velocity = Vector3.zero;

        private void Start()
        {
            player = GameObject.Find("Player").GetComponent<Player>();
            
            // Lưu vị trí setup từ scene
            initialCamPosition = transform.position;
            offset = initialCamPosition - new Vector3(3.5f, 0, 2);

        }

        private void LateUpdate()
        {
            if (GameController.Ins.GetGameResult()) return;
            MovePlayer();
        }

        private void MovePlayer()
        {
            Transform targetTf = player.GetCurrentShape().transform;
            Vector3 desiredPosition = targetTf.position + offset;

            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, 0.2f);
        }
    }
}

