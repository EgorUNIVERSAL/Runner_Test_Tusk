using UnityEngine;

namespace ButchersGames
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("Цель")]
        [SerializeField] private Transform target;

        [Header("Позиция относительно игрока")]
        [Tooltip("Дистанция")]
        [SerializeField] private float distance = 8f;

        [Tooltip("Угол над горизонтом")]
        [Range(0f, 80f)]
        [SerializeField] private float pitch = 25f;

        [Tooltip("Боковое смещение")]
        [SerializeField] private float sideOffset = 0f;

        [Header("Куда смотрит")]
        [Tooltip("Высота точки")]
        [SerializeField] private float lookAtHeight = 1.2f;

        [Tooltip("Опережение")]
        [SerializeField] private float lookAhead = 2f;

        [Header("Сглаживание")]
        [SerializeField] private float followSpeed = 8f;

        [SerializeField] private float rotateSpeed = 8f;

        public void SetTarget(Transform t) => target = t;

        private void LateUpdate()
        {
            if (target == null) return;

            float rad = pitch * Mathf.Deg2Rad;
            Vector3 localOffset = new Vector3(
                sideOffset,
                Mathf.Sin(rad) * distance,
                -Mathf.Cos(rad) * distance);

            Vector3 desiredPos = target.position + target.rotation * localOffset;

            transform.position = Vector3.Lerp(transform.position, desiredPos,
                followSpeed * Time.deltaTime);

            Vector3 lookPoint = target.position
                              + Vector3.up * lookAtHeight
                              + target.forward * lookAhead;

            Quaternion desiredRot = Quaternion.LookRotation(lookPoint - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot,
                rotateSpeed * Time.deltaTime);
        }
    }
}