// If running in the production environment, load all the variables from .env file.
if (process.env.NODE_ENV !== 'production') {
  require('dotenv').config();
}

const WebSocket = require('ws');
const express = require('express');
const bodyParser = require('body-parser');
const app = express();

const { check, body, oneOf, validationResult } = require('express-validator');

const { createUser } = require('./db.js');

const websocketPort = process.env.WEBSOCKET_PORT || 5001;
const port = process.env.HTTP_PORT || 3000;

const loginValidation = [
  oneOf([
    check('usernameOrEmail')
      .exists()
      .withMessage('Username is required.')
      .isLength({ min: 1 })
      .withMessage('Wrong username length.'),

    check('usernameOrEmail')
      .exists()
      .withMessage('Username or mail is required.')
      .isEmail()
      .withMessage('Email is not valid.'),
  ]),

  check('password').exists().withMessage('Password is required.'),
];

app.use(express.static(__dirname + '/public'));
app.use(bodyParser.urlencoded({ limit: '30mb', extended: false }));

app.get('/', (req, res) => res.redirect('/login'));

app.get('/login', (req, res) => {
  res.sendFile('./public/views/login.html', { root: __dirname });
});

app.post('/login', loginValidation, (req, res) => {
  const errors = validationResult(req);
  if (!errors.isEmpty()) {
    return res.status(400).json({ errors: errors.array() });
  }

  const login = {
    usernameOrEmail: req.body.usernameOrEmail,
    password: req.body.password,
  };

  const isEmail = login.usernameOrEmail.includes('@');
  console.log(login);
  return res.status(200).json({ isEmail: isEmail });
});

app.get(
  '/register',
  (req, res) => {
    res.sendFile('./public/views/register.html', { root: __dirname });
  },
  body('usernameOrEmail').isLength({ min: 1 }),
  body('password').isLength({ min: 6 }),
  async (req, res) => {
    const errors = validationResult(req);
    if (!errors.isEmpty()) {
      return res.status(400).json({ errors: errors.array() });
    }
  }
);

app.post(
  '/register',
  body('username').isLength({ min: 1 }),
  body('email').isEmail(),
  body('password').isLength({ min: 6 }),
  async (req, res) => {
    const errors = validationResult(req);
    if (!errors.isEmpty()) {
      return res.status(400).json({ errors: errors.array() });
    }

    user = {
      username: req.body.username,
      email: req.body.email,
      password: req.body.password,
    };

    await createUser(user);
    res.redirect('/');
  }
);

app.listen(port, () => {
  console.log(`Server is listening on port ${port}...`);
});

// Run the server.
const webSocketServer = new WebSocket.Server({ port: websocketPort }, () => {
  console.log(`Web socket server started at ${websocketPort}...`);
});

// Handle connection event.
webSocketServer.on('connection', (webSocket) => {
  webSocket.on('message', (data) => {
    console.log('Data received: ' + data);
    // Send the message back to the same client.
    webSocket.send(data.toString());
  });
});
