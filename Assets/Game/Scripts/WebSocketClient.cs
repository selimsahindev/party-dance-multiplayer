using WebSocketSharp;
using UnityEngine;
using System.Collections;
using System;

namespace InfiniteChocolate
{
    public class WebSocketClient : MonoBehaviour
    {
        [Min(3f), SerializeField] private float autoReconnectWaitTime = 5f;

        private WebSocket ws;
        private Coroutine autoReconnect;

        private void Start()
        {
            // Create socket instance.
            ws = new WebSocket("ws://localhost:5000");

            // Register event handlers.
            ws.OnMessage += HandleOnMessage;
            ws.OnOpen += HandleOnOpen;
            ws.OnClose += HandleOnClose;

            // Establish connection.
            ws.Connect();
        
            // If the connection did not start, try to connect again.
            if (ws.ReadyState != WebSocketState.Open)
            {
                StartAutoReconnect();
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
            if (ws != null)
            {
                try
                {
                    ws.Send("Hello From The Unity.");
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e.Message);
                }
            }
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
            Debug.Log($"Message received from: {((WebSocket)sender).Url}. Data: {e.Data}");
        }
        #endregion
    }
}
