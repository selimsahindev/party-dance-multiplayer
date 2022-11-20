using UnityEngine;

namespace Game.Constants
{
    [CreateAssetMenu(fileName = "New Game Settings", menuName = "Scriptables/Settings")]
    public class Settings : ScriptableObject
    {
        [Header("Network Settings"), Space(2)]
        [SerializeField] private string webSocketDomain;
        [SerializeField] private int webSocketPort;

        public string WebSocketURL
        {
            get {
                return $"ws://{webSocketDomain}:{webSocketPort}";
            }
        }
    }
}
