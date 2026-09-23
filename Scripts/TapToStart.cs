using UnityEngine;
using UnityEngine.EventSystems;

namespace ButchersGames
{
    public class TapToStart : MonoBehaviour
    {
        private bool started;

        private void Update()
        {
            if (started) return;
            if (GameManager.Instance == null) return;
            if (GameManager.Instance.State != GameState.Start) return;

            if (TappedThisFrame())
            {
                // игнорируем тап по UI, если он есть (на будущее)
                if (EventSystem.current != null && IsPointerOverUI()) return;

                started = true;
                GameManager.Instance.StartGame();
            }
        }

        private bool TappedThisFrame()
        {
            if (Input.GetMouseButtonDown(0)) return true;
            for (int i = 0; i < Input.touchCount; i++)
                if (Input.GetTouch(i).phase == TouchPhase.Began) return true;
            return false;
        }

        private bool IsPointerOverUI()
        {
            if (Input.touchCount > 0)
                return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}