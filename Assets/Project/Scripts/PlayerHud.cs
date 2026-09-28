using UnityEngine;
using UnityEngine.UI;

namespace ButchersGames
{
    public class PlayerHud : MonoBehaviour
    {
        [Header("Ссылки")]
        [SerializeField] private Slider valueSlider;
        [SerializeField] private PlayerStats stats;
        [SerializeField] private Transform cameraTransform;

        [Header("Настройки")]
        [Tooltip("Плавность изменения шкалы.")]
        [SerializeField] private float smoothSpeed = 8f;

        private float targetValue;

        private void Start()
        {
            if (stats == null) stats = GetComponentInParent<PlayerStats>();
            if (cameraTransform == null && Camera.main != null)
                cameraTransform = Camera.main.transform;

            if (stats != null)
            {
                stats.OnValueChanged += OnValueChanged;

                // стартовая позиция без анимации «с нуля»
                targetValue = Mathf.InverseLerp(stats.MinValue, stats.MaxValue, stats.Value);
                if (valueSlider != null) valueSlider.value = targetValue;
            }
        }

        private void OnDestroy()
        {
            if (stats != null) stats.OnValueChanged -= OnValueChanged;
        }

        private void OnValueChanged(int value)
        {
            if (stats == null) return;
            targetValue = Mathf.InverseLerp(stats.MinValue, stats.MaxValue, value);
        }

        private void LateUpdate()
        {
            if (valueSlider != null)
                valueSlider.value = Mathf.Lerp(valueSlider.value, targetValue,
                    smoothSpeed * Time.deltaTime);

            if (cameraTransform != null)
                transform.rotation = cameraTransform.rotation;
        }
    }
}