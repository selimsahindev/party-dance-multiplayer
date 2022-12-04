using UnityEngine;
using Game.Network;

namespace Game.Core
{
    public class AudienceController : MonoBehaviour
    {
        private Animator animator;
        private bool startDance;
        private bool stopDance;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            NetworkManager.OnMessage += HandleOnMessage;
        }

        private void Update()
        {
            if (startDance || stopDance)
            {
                animator.SetTrigger(startDance ? "Dance" : "Idle");
                startDance = false;
                stopDance = false;
            }
        }

        private void HandleOnMessage(string message)
        {
            if (message == "PlayAnimation")
            {
                startDance = true;
            }
            else if (message == "StopAnimation")
            {
                stopDance = true;
            }
        }
    }
}
