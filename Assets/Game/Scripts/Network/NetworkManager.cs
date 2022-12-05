using NativeWebSocket;
using UnityEngine;
using UnityEngine.Events;
using Game.ScriptableObjects;

namespace Game.Network
{
    public class NetworkManager : MonoBehaviour
    {
        public WebSocket websocket { get; private set; }
        public static UnityAction<string> OnMessage;
        public static NetworkManager Instance;

        private NetworkSettings settings;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
                settings = Resources.Load<NetworkSettings>("NetworkSettings");
            }
            else
            {
                Destroy(this);
            }
        }

        private async void Start()
        {
            websocket = new WebSocket(settings.WebSocketURL);

            websocket.OnOpen += () => Debug.Log("WebSocket: Connection Open.");
            websocket.OnError += (e) => Debug.Log("WebSocket: Error! " + e);
            websocket.OnClose += (e) => Debug.Log("WebSocket: Connection Closed.");

            websocket.OnMessage += (bytes) =>
            {
                // Getting the message as a string.
                string message = System.Text.Encoding.UTF8.GetString(bytes);
                OnMessage?.Invoke(message);

                Debug.Log("WebSocket: Message Received: " + message);
            };

            await websocket.Connect();
        }

#if !UNITY_WEBGL || UNITY_EDITOR
        private void Update()
        {
            websocket.DispatchMessageQueue();
        }
#endif

        public async void SendWebSocketMessage(string message)
        {
            if (websocket.State == WebSocketState.Open)
            {
                // Sending plain text
                await websocket.SendText(message);
            }
        }

        private async void OnApplicationQuit()
        {
            await websocket.Close();
        }
    }
}
