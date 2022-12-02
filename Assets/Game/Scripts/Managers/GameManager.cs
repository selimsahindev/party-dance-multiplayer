using UnityEngine;
using Game.Events;

namespace Game.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static bool IsGameActive { get; private set; }

        private void Start()
        {
            StartGame();
        }

        private void StartGame()
        {
            IsGameActive = true;
            EventManager.GameStarted?.Invoke();
        }
    }
}
