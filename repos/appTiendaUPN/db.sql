-- 1. Tabla de Usuarios (ya existe según tu README)
CREATE TABLE users (
    userid SERIAL PRIMARY KEY,
    nombre VARCHAR(120) NOT NULL,
    email VARCHAR(120) UNIQUE NOT NULL,
    passwordhash VARCHAR(500) NOT NULL,
    telefono VARCHAR(20),
    direccion VARCHAR(200),
    rol VARCHAR(20) NOT NULL DEFAULT 'Cliente',
    fecharegistro TIMESTAMP NOT NULL DEFAULT NOW()
);

-- 2. Tabla de Categorías
CREATE TABLE categorias (
    categoriaid SERIAL PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    descripcion VARCHAR(500)
);

-- 3. Tabla de Productos
CREATE TABLE productos (
    productoid SERIAL PRIMARY KEY,
    nombre VARCHAR(200) NOT NULL,
    descripcion TEXT,
    precio DECIMAL(18,2) NOT NULL,
    precioanterior DECIMAL(18,2),
    stock INT NOT NULL DEFAULT 0,
    imagenurl VARCHAR(500),
    categoriaid INT NOT NULL,
    estaactivo BOOLEAN DEFAULT TRUE,
    esoferta BOOLEAN DEFAULT FALSE,
    fechacreacion TIMESTAMP DEFAULT NOW(),
    CONSTRAINT fk_producto_categoria FOREIGN KEY (categoriaid) 
        REFERENCES categorias(categoriaid)
);

-- 4. Tabla de Carritos
CREATE TABLE carritos (
    carritoid SERIAL PRIMARY KEY,
    userid INT NOT NULL,
    fechacreacion TIMESTAMP DEFAULT NOW(),
    CONSTRAINT fk_carrito_user FOREIGN KEY (userid) 
        REFERENCES users(userid)
);

-- 5. Tabla de Items del Carrito (SIN GUION BAJO)
CREATE TABLE carritoitems (
    carritoitemid SERIAL PRIMARY KEY,
    carritoid INT NOT NULL,
    productoid INT NOT NULL,
    cantidad INT NOT NULL DEFAULT 1,
    precio DECIMAL(18,2) NOT NULL,
    CONSTRAINT fk_carritoitem_carrito FOREIGN KEY (carritoid) 
        REFERENCES carritos(carritoid) ON DELETE CASCADE,
    CONSTRAINT fk_carritoitem_producto FOREIGN KEY (productoid) 
        REFERENCES productos(productoid)
);
