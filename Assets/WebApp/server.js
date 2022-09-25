const WebSocket = require('ws');
const port = 5000;

// Run the server.
const webSocketServer = new WebSocket.Server({ port: port }, () => {
  console.log(`web socket server started.`);
});

// Handle connection event.
webSocketServer.on('connection', (webSocket) => {
  webSocket.on('message', (data) => {
    console.log('data received %o' + data);
    // Send the message back to the same client.
    webSocket.send(data);
  });
});

// Handle listening event.
webSocketServer.on('listening', () => {
  console.log(`server is listening on port ${port}...`);
});