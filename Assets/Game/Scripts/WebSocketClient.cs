using WebSocketSharp;
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;

namespace Game.Network
{
    public class WebSocketClient : MonoBehaviour
    {
        [Min(3f), SerializeField] private float autoReconnectWaitTime = 5f;

        public WebSocket ws { get; private set; }

        private Coroutine autoReconnect;

        public static UnityAction<string> OnMessage;
        public static WebSocketClient Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            using (ws = new WebSocket("ws://localhost:5000"))
            {            
                // Register event handlers.
                ws.OnMessage += HandleOnMessage;
                ws.OnOpen += HandleOnOpen;
                ws.OnClose += HandleOnClose;

                // Establish connection.
                ws.Connect();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("WS State: " + ws.ReadyState.ToString());
                SendTestMessage();
            }
        }

        private void SendTestMessage()
        {
            Debug.Log("SendTestMessage Called.");
            if (ws != null)
            {
                try
                {
                    ws.Send("Hello From The Unity. (Web Socket Client)");
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e.Message);
                }
            }
            Debug.Log("SendTestMessage End.");
        }

        #region Auto Reconnect
        private void StartAutoReconnect()
        {
            if (autoReconnect == null)
            {
                autoReconnect = StartCoroutine(ReconnectRoutine());
            }
        }

        private void StopAutoReconnect()
        {
            if (autoReconnect != null)
            {
                StopCoroutine(autoReconnect);
                autoReconnect = null;
            }
        }

        /// <summary>
        /// Attempts to reconnect with increasing wait times.
        /// </summary>
        private IEnumerator ReconnectRoutine()
        {
            while (ws.ReadyState != WebSocketState.Open)
            {
                Debug.Log($"Trying to reconnect in {autoReconnectWaitTime} seconds...");
                yield return new WaitForSeconds(autoReconnectWaitTime);

                // Try to connect.
                ws.Connect();
            }

            // Routine is already over but this will handle our current routine info.
            StopAutoReconnect();
        }
        #endregion

        #region Event Handlers
        private void HandleOnOpen(object sender, EventArgs e)
        {
            Debug.Log("WebSocket connection successful.");
        }

        private void HandleOnClose(object sender, CloseEventArgs e)
        {
            Debug.Log("WebSocket disconnected.");
            StartAutoReconnect();
        }

        private void HandleOnMessage(object sender, MessageEventArgs e)
        {
            OnMessage?.Invoke(e.Data);
            Debug.Log($"Message received from: {((WebSocket)sender).Url}. Data: {e.Data}");
        }
        #endregion
    }
}
