var WebSocketBridgePlugin = {
  Send: function (message) {
    console.log(UTF8ToString(message));
    socket.send(UTF8ToString(message));
    SendMessage(UTF8ToString(message));
  }
}

mergeInto(LibraryManager.library, WebSocketBridgePlugin);