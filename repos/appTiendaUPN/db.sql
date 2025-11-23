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


-- Categorías
INSERT INTO categorias (nombre, descripcion) VALUES
('Laptops', 'Computadoras portátiles de última generación'),
('Smartphones', 'Teléfonos inteligentes y accesorios'),
('Tablets', 'Tablets y dispositivos móviles'),
('Accesorios', 'Accesorios tecnológicos variados');

-- Productos de ejemplo
INSERT INTO productos (nombre, descripcion, precio, precioanterior, stock, imagenurl, categoriaid, esoferta) VALUES
('Laptop HP Pavilion 15', 'Intel Core i5 11va Gen, 8GB RAM, 512GB SSD, Windows 11', 2499.00, 2999.00, 15, 'https://placehold.co/300x200/2196F3/FFF?text=Laptop+HP', 1, true),
('MacBook Air M2', 'Chip M2, 8GB RAM, 256GB SSD, Pantalla Retina 13.6"', 5499.00, NULL, 8, 'https://placehold.co/300x200/4CAF50/FFF?text=MacBook+Air', 1, false),
('iPhone 15 Pro', '128GB, Titanio Azul, Triple Cámara', 5299.00, NULL, 12, 'https://placehold.co/300x200/FF9800/FFF?text=iPhone+15', 2, false),
('Samsung Galaxy S24', '256GB, Violeta, Snapdragon 8 Gen 3', 3899.00, 4299.00, 10, 'https://placehold.co/300x200/9C27B0/FFF?text=Galaxy+S24', 2, true),
('iPad Air 2024', '128GB WiFi, Pantalla 10.9", Chip M1', 2899.00, NULL, 20, 'https://placehold.co/300x200/E91E63/FFF?text=iPad+Air', 3, false),
('Samsung Galaxy Tab S9', '128GB, 11", S Pen incluido', 1899.00, 2199.00, 7, 'https://placehold.co/300x200/00BCD4/FFF?text=Tab+S9', 3, true),
('AirPods Pro 2', 'Cancelación de ruido activa, USB-C', 1099.00, NULL, 25, 'https://placehold.co/300x200/607D8B/FFF?text=AirPods', 4, false),
('Logitech MX Master 3S', 'Mouse inalámbrico, 8000 DPI', 399.00, 499.00, 30, 'https://placehold.co/300x200/795548/FFF?text=Mouse', 4, true);