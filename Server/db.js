require('dotenv').config();
const { Client } = require('pg');

const client = new Client({
  host: process.env.PGHOST,
  port: process.env.PGPORT,
  database: process.env.PGDATABASE,
  user: process.env.PGUSER,
  password: process.env.PGPASSWORD,
});

const createUser = async (user) => {
  const query = {
    text: 'INSERT INTO users(username, email, password) VALUES($1, $2, $3)',
    values: [user.username, user.email, user.password],
  };

  try {
    await client.connect();
    const res = await client.query(query);

    console.log(res.rows[0]);
  } catch (err) {
    console.log(err.stack);
  }

  await client.end();
};

const test = async () => {
  try {
    await client.connect();
    const res = await client.query('SELECT $1::text as message', [
      'Hello world!',
    ]);
    console.log(res.rows[0].message); // Hello world!
  } catch (error) {
    console.log(error.stack);
  }

  await client.end();
  process.exit();
};

const executeQuery = async (query) => {
  try {
    await client.connect();
    const res = await client.query(query);

    console.log('Query executed successfully.');
    console.log(res.rows);
  } catch (error) {
    console.log(error.stack);
  }

  await client.end();
  process.exit();
};

if (process.argv[2] === '--test') {
  test();
} else if (process.argv[2] == '--query') {
  executeQuery(process.argv[3]);
}

module.exports = {
  createUser,
};
