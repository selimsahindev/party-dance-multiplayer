using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using Game.Network;

namespace Game.Core
{
    public class PlayerController : MonoBehaviour
    {
    #if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void Send(string message);
    #endif

        private void OnGUI()
        {
            if (GUI.Button(new Rect(10f, 75f, 250f, 50), "Play Dance Animations"))
            {
                WebSocketClient.Instance.ws.Send("PlayAnimation");
            }

            if (GUI.Button(new Rect(10f, 135f, 250f, 50), "Stop Dance Animations"))
            {
                WebSocketClient.Instance.ws.Send("StopAnimation");
            }
        }
    }
}
