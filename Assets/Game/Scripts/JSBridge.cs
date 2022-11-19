using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class JSBridge : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void Send(string message);
#endif

    public static JSBridge Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            Debug.Log("Sending the message.");
            Send("Hello from Unity!");
#else
            Debug.Log("Switch to WebGL to send the message.");
#endif
        }
    }
}
