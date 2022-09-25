using UnityEngine;
using UnitySocketIO.SocketIo;
using IngameDebugConsole;

public class Server : MonoBehaviour
{
    Socket socket;

    private void Start()
    {
        //The url must include "ws://" as the protocol
        socket = SocketIo.establishSocketConnection("ws://localhost:3000");
        socket.connect();
        socket.on("testEvent", HandleTestEvent);
    }

    private void HandleTestEvent(string arg)
    {
        Debug.Log("Received Event: " + arg);
    }
}
