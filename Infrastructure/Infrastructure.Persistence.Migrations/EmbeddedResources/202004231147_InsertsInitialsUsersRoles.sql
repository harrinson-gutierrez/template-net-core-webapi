
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

INSERT INTO roles(role_id, role_name) VALUES(1, 'ADMINISTRADOR');
INSERT INTO roles(role_id, role_name) VALUES(2, 'USER');

INSERT INTO users(user_id, username, email, email_confirmed, password_hash, security_stamp)VALUES(1, 'HGUTIECO@GMAIL.COM', 'HGUTIECO@GMAIL.COM', true, 'AQAAAAEAACcQAAAAENw39/9uR6w/L4jeAIRU6vbRrr05fD9FFNXbFUeyxIkEza8fdDYh/KzNVjjLPav2qA==', '');

INSERT INTO users_roles(user_id, role_id) VALUES (1, 1);
