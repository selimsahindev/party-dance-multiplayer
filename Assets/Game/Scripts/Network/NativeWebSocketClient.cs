using NativeWebSocket;
using UnityEngine;
using UnityEngine.Events;
using Game.Constants;

namespace Game.Network
{
    public class NativeWebSocketClient : MonoBehaviour
    {
        public WebSocket websocket { get; private set; }
        public static UnityAction<string> OnMessage;
        public static NativeWebSocketClient Instance;

        private Settings settings;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            settings = Resources.Load<Settings>("GameSettings");
        }

        private async void Start()
        {
            websocket = new WebSocket(settings.WebSocketURL);

            websocket.OnOpen += () => Debug.Log("(WebSocket) Connection Open.");
            websocket.OnError += (e) => Debug.Log("(WebSocket) Error! " + e);
            websocket.OnClose += (e) => Debug.Log("(WebSocket) Connection Closed.");

            websocket.OnMessage += (bytes) =>
            {
                // Getting the message as a string.
                string message = System.Text.Encoding.UTF8.GetString(bytes);
                OnMessage?.Invoke(message);

                Debug.Log("(WebSocket) Message Received: " + message);
            };

            // waiting for messages
            await websocket.Connect();
        }

        private void Update()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            websocket.DispatchMessageQueue();
#endif
        }

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
