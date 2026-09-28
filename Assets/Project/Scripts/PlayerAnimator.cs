using UnityEngine;

namespace ButchersGames
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
    {
        [Header("Ссылки")]
        [SerializeField] private PlayerController controller;

        [Header("Настройки")]
        [Tooltip("скорость бега")]
        [SerializeField] private float referenceSpeed = 8f;

        [Tooltip("Ограничитель")]
        [SerializeField] private float maxSpeedMultiplier = 1.6f;

        private Animator animator;
        private static readonly int WinHash = Animator.StringToHash("Win");
        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private bool won;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            if (controller == null) controller = GetComponentInParent<PlayerController>();
        }

        private void Update()
        {
            if (controller == null) return;

            float speedNorm = controller.IsRunning ? 1f : 0f;
            animator.SetFloat(SpeedHash, speedNorm, 0.1f, Time.deltaTime);

            if (controller.IsRunning)
            {
                float mult = Mathf.Clamp(controller.ForwardSpeed / referenceSpeed,
                    0.6f, maxSpeedMultiplier);
                animator.speed = mult;
            }
            else
            {
                animator.speed = 1f;
            }
        }

        public void PlayWin()
        {
            if (won) return;
            won = true;

            animator.speed = 1f;
            animator.SetFloat(SpeedHash, 0f);
            animator.SetTrigger(WinHash);
        }
    }
}