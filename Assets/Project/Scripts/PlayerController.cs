using System.Collections;
using UnityEngine;

namespace ButchersGames
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Движение")]
        [SerializeField] private float forwardSpeed = 8f;
        [SerializeField] private float laneHalfWidth = 2.5f;

        private CharacterController cc;
        private Vector3 idealCenterPos;   
        private float lateralOffset;   
        private float velocityY;
        private bool isRunning;
        private bool isTurning;

        public bool IsRunning => isRunning;
        public bool IsTurning => isTurning;
        public float ForwardSpeed { get => forwardSpeed; set => forwardSpeed = value; }
        public float LaneHalfWidth { get => laneHalfWidth; set => laneHalfWidth = value; }

        private void Awake()
        {
            cc = GetComponent<CharacterController>();
            idealCenterPos = transform.position;
        }

        private void Update()
        {
            if (!isRunning) return;

            // гравитация
            if (cc.isGrounded && velocityY < 0f) velocityY = -2f;
            velocityY += Physics.gravity.y * Time.deltaTime;

            // продвигаем призрак вперёд вдоль текущего «лица» игрока
            idealCenterPos += transform.forward * forwardSpeed * Time.deltaTime;

            // куда игрок должен стоять
            Vector3 desired = idealCenterPos + transform.right * lateralOffset;

            // двигаем
            Vector3 diff = desired - transform.position;
            diff.y = 0f;

            cc.Move(diff + Vector3.up * velocityY * Time.deltaTime);
        }

        public void StartRunning() => isRunning = true;
        public void StopRunning() => isRunning = false;

        /// <summary>Ввод: deltaX в пикселях за кадр.</summary>
        public void MoveHorizontal(float deltaX, float sensitivity)
        {
            // во время поворота руль игнорируем — иначе слетает с дуги
            if (isTurning) return;

            lateralOffset += deltaX * sensitivity;
            lateralOffset = Mathf.Clamp(lateralOffset, -laneHalfWidth, laneHalfWidth);
        }

        /// <summary>Вызывается TurnZone при входе в поворот.</summary>
        public void StartTurn(float degrees, float duration)
        {
            Debug.Log($"[Player] StartTurn(deg={degrees}, dur={duration}, isTurning={isTurning})", this);
            if (isTurning) return;
            StartCoroutine(TurnRoutine(degrees, duration));
        }

        private IEnumerator TurnRoutine(float degrees, float duration)
        {
            
            isTurning = true;
            Quaternion start = transform.rotation;
            Quaternion end = start * Quaternion.Euler(0f, degrees, 0f);
            Debug.Log($"[Player] TurnRoutine started, start={start.eulerAngles}, end={end.eulerAngles}", this);

            float t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                transform.rotation = Quaternion.Slerp(start, end, Mathf.Clamp01(t / duration));
                yield return null;
            }
            transform.rotation = end;
            isTurning = false;
        }

        public void SnapTo(Vector3 worldPos, Quaternion worldRot)
        {
            cc.enabled = false;
            transform.SetPositionAndRotation(worldPos, worldRot);
            idealCenterPos = worldPos;
            lateralOffset = 0f;
            cc.enabled = true;
        }
    }
}