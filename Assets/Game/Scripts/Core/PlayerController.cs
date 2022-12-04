using Game.Controllers;
using UnityEngine;
using Game.Network;
using Game.Events;

namespace Game.Core
{
    public class PlayerController : MonoBehaviour
    {
        private Animator animator;
        private bool isDancing = false;

        private IMistake mistake;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            mistake = GetComponent<MistakeController>();
        }

        #region Event Handlers

        private void OnGameStarted()
        {
            animator.SetTrigger("Dance");
            isDancing = true;
        }

        private void OnSuccessfulMove()
        {
            if (!isDancing)
            {
                animator.SetTrigger("Dance");
                isDancing = true;
            }
        }

        private void OnMissedInput()
        {
            mistake.CharacterHasMadeMistake();
            
            if (isDancing)
            {
                animator.SetTrigger("Idle");
                isDancing = false;
            }
        }

        #endregion

        private void OnGUI()
        {
            if (GUI.Button(new Rect(10f, 75f, 250f, 50), "Play Dance Animations"))
            {
                NetworkManager.Instance.SendWebSocketMessage("PlayAnimation");
            }

            if (GUI.Button(new Rect(10f, 135f, 250f, 50), "Stop Dance Animations"))
            {
                NetworkManager.Instance.SendWebSocketMessage("StopAnimation");
            }
        }

        private void OnEnable()
        {
            EventManager.GameStarted += OnGameStarted;
            EventManager.SuccessfulMove += OnSuccessfulMove;
            EventManager.MissedInput += OnMissedInput;
        }

        private void OnDisable()
        {
            EventManager.GameStarted -= OnGameStarted;
            EventManager.SuccessfulMove -= OnSuccessfulMove;
            EventManager.MissedInput -= OnMissedInput;
        }
    }
}
