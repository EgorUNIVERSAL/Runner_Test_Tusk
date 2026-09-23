using UnityEngine;
using UnityEngine.EventSystems;

namespace ButchersGames
{
    public class SwipeInput : MonoBehaviour
    {
        [SerializeField] private PlayerController player;
        [SerializeField] private float sensitivity = 0.012f;

        private bool dragging;
        private Vector2 lastPos;

        private void Update()
        {
            if (player == null || !player.IsRunning) return;

#if UNITY_EDITOR
            // отладка в редакторе
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                player.MoveHorizontal(-25f, sensitivity);
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                player.MoveHorizontal(25f, sensitivity);
#endif

            // мышь (в редакторе и на ПК)
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                    return;
                dragging = true;
                lastPos = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                dragging = false;
            }
            else if (dragging && Input.GetMouseButton(0))
            {
                Vector2 cur = Input.mousePosition;
                player.MoveHorizontal(cur.x - lastPos.x, sensitivity);
                lastPos = cur;
            }

            // тач (устройство)
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch t = Input.GetTouch(i);
                switch (t.phase)
                {
                    case TouchPhase.Began:
                        if (EventSystem.current != null &&
                            EventSystem.current.IsPointerOverGameObject(t.fingerId)) break;
                        dragging = true;
                        lastPos = t.position;
                        break;
                    case TouchPhase.Moved:
                        if (dragging)
                        {
                            player.MoveHorizontal(t.position.x - lastPos.x, sensitivity);
                            lastPos = t.position;
                        }
                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        dragging = false;
                        break;
                }
            }
        }
    }
}