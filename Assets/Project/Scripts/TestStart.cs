using UnityEngine;
namespace ButchersGames
{
    public class TestStart : MonoBehaviour
    {
        [SerializeField] PlayerController player;
        void Start() => player.StartRunning();
    }
}