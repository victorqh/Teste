CREATE DATABASE appFinal;
USE appFinal;

CREATE TABLE users (
    userid INT PRIMARY KEY AUTO_INCREMENT,
    nombre VARCHAR(120) NOT NULL,
    email VARCHAR(120) UNIQUE NOT NULL,
    passwordhash VARCHAR(500) NOT NULL,
    telefono VARCHAR(20),
    direccion VARCHAR(200),
    rol VARCHAR(20) NOT NULL DEFAULT 'Cliente',
    fecharegistro TIMESTAMP NOT NULL DEFAULT NOW()
);