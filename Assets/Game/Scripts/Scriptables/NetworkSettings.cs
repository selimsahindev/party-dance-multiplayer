using UnityEngine;

namespace Game.Constants
{
    [CreateAssetMenu(fileName = "New Network", menuName = "Scriptables/Network Settings")]
    public class NetworkSettings : ScriptableObject
    {
        [Space(2)]
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
