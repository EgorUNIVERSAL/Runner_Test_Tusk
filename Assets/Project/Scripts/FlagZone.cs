using UnityEngine;
namespace ButchersGames
{
    public class FlagZone : MonoBehaviour
    {
        [SerializeField] private AudioSource source;
        [SerializeField] private AudioClip clip;
        [SerializeField] private Animator left_anim;
        [SerializeField] private Animator right_anim;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                left_anim.Play("Left_flag_UP", 0, 0f);
                right_anim.Play("Right_flag_up", 0, 0f);
                source.PlayOneShot(clip);
                other.GetComponent<PlayerStats>().Multiply(2);
            }
        }
    }
}
