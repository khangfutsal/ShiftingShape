using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Khang
{
    public class InfiniteScrollView : MonoBehaviour
    {
        public ScrollRect scrollRect;                          // ScrollRect chính
        public RectTransform viewPortTransform;                 // ViewPort (vùng nhìn)
        public RectTransform contentPanelTransform;             // Content chứa các item
        public HorizontalLayoutGroup HLG;                       // Layout group của content

        public RectTransform[] itemPrefabs;                     // Prefab các item có thể dùng

        public Vector2 oldVelocity;                             // Lưu velocity cũ để khôi phục
        public bool isUpdated;

        private float itemWidth;                                // Bề rộng mỗi item (gồm spacing)
        private int itemsToAdd;                                 // Số lượng item cần thêm để phủ hết view

        private void Start()
        {
            isUpdated = false;
            oldVelocity = Vector2.zero;

            // 👉 Tính bề rộng 1 item + spacing
            itemWidth = itemPrefabs[0].rect.width + HLG.spacing;

            // 👉 Tính số item cần thêm để che full viewport
            itemsToAdd = Mathf.CeilToInt(viewPortTransform.rect.width / itemWidth);

            // 👉 Thêm item vào phía cuối
            for (int i = 0; i < itemsToAdd; i++)
            {
                RectTransform rt = Instantiate(itemPrefabs[i % itemPrefabs.Length], contentPanelTransform);
                rt.SetAsLastSibling();
            }

            // 👉 Thêm item vào phía đầu
            for (int i = 0; i < itemsToAdd; i++)
            {
                int num = (itemPrefabs.Length - i - 1 + itemPrefabs.Length) % itemPrefabs.Length;
                RectTransform rt = Instantiate(itemPrefabs[num], contentPanelTransform);
                rt.SetAsFirstSibling();
            }

            // 👉 Tính offset căn giữa content ban đầu
            float totalOffset = itemWidth * itemsToAdd;
            contentPanelTransform.localPosition = new Vector3(
                -totalOffset,
                contentPanelTransform.localPosition.y,
                contentPanelTransform.localPosition.z);
        }

        private void Update()
        {
            if (isUpdated)
            {
                isUpdated = false;
                scrollRect.velocity = Vector2.zero;
            }

            // 👉 Nếu contentPanel trượt về bên trái quá mức
            if (contentPanelTransform.localPosition.x > 0)
            {
                Canvas.ForceUpdateCanvases();
                oldVelocity = scrollRect.velocity;

                // 👉 Dịch ngược lại về bên phải
                float moveAmount = itemPrefabs.Length * itemWidth;
                contentPanelTransform.localPosition -= new Vector3(moveAmount, 0, 0);

                isUpdated = true;
            }

            // 👉 Nếu contentPanel trượt về bên phải quá mức
            if (contentPanelTransform.localPosition.x < -itemPrefabs.Length * itemWidth)
            {
                Canvas.ForceUpdateCanvases();
                oldVelocity = scrollRect.velocity;

                // 👉 Dịch ngược lại về bên trái
                float moveAmount = itemPrefabs.Length * itemWidth;
                contentPanelTransform.localPosition += new Vector3(moveAmount, 0, 0);

                isUpdated = true;
            }
        }
    }
}
