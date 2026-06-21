CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Username VARCHAR(100) NOT NULL UNIQUE,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Nome VARCHAR(200) NOT NULL,
    PasswordHash VARCHAR(MAX) NOT NULL,
    Role VARCHAR(50) DEFAULT 'user'
);

INSERT INTO Users (Username, Email, Nome, PasswordHash, Role)
VALUES (
    'admin',
    'admin@email.com',
    'Administrador',
    '$2a$11$Zs1Sh/RpvF9A3zqGbKvvN.vMhMbMVv8pN8sFdYh8xD4mP2Q4xZ9nK',
    'admin'
);