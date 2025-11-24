# Sistema de Ventas - Proyecto Full Stack

Sistema de gestión de artículos e inventario desarrollado con **Spring Boot** (backend) y **Angular** (frontend).

## 📋 Descripción

Aplicación web para administrar artículos y sus categorías (tipos de artículos), con funcionalidades completas de CRUD.

## 🛠️ Tecnologías

### Backend
- **Java 21**
- **Spring Boot 4.0**
- **Spring Data JPA**
- **MySQL**
- **Maven**

### Frontend
- **Angular 20**
- **TypeScript**
- **HttpClient**
- **Router**

## 🗄️ Estructura del Proyecto

```
SEM14/
├── backend/           # API REST con Spring Boot
│   ├── src/
│   │   └── main/
│   │       ├── java/com/upn/ventas/
│   │       │   ├── controller/
│   │       │   ├── service/
│   │       │   ├── repository/
│   │       │   ├── model/
│   │       │   └── dto/
│   │       └── resources/
│   └── pom.xml
│
└── frontend/          # Aplicación Angular
    ├── src/app/
    │   ├── components/
    │   ├── services/
    │   └── models/
    └── package.json
```

## 🚀 Instalación y Ejecución

### Prerrequisitos
- Java JDK 21
- Node.js 22+
- MySQL 8+
- Maven
- Angular CLI

### 1. Configurar Base de Datos

```sql
CREATE DATABASE proyecto20;
```

Actualizar credenciales en `backend/src/main/resources/application.properties`:
```properties
spring.datasource.url=jdbc:mysql://localhost:3306/proyecto20?serverTimezone=UTC
spring.datasource.username=root
spring.datasource.password=TU_PASSWORD
```

### 2. Ejecutar Backend

```bash
cd backend
mvnw spring-boot:run
```

El servidor iniciará en: `http://localhost:8085`

### 3. Ejecutar Frontend

```bash
cd frontend
npm install
ng serve
```

La aplicación estará disponible en: `http://localhost:4200`

## 📡 API Endpoints

### Artículos
- `GET /articulos` - Listar todos
- `GET /articulos/{id}` - Obtener por ID
- `POST /articulos` - Crear nuevo
- `PUT /articulos/{id}` - Actualizar
- `DELETE /articulos/{id}` - Eliminar

### Tipos de Artículos
- `GET /tipoarticulos` - Listar todos
- `GET /tipoarticulos/{id}` - Obtener por ID
- `GET /tipoarticulos/paginado?page=0&size=5` - Listar paginado
- `POST /tipoarticulos` - Crear nuevo
- `PUT /tipoarticulos/{id}` - Actualizar
- `DELETE /tipoarticulos/{id}` - Eliminar

## 📊 Modelo de Datos

### Articulos
```json
{
  "id": 1,
  "descripcion": "Inca Kola 2L",
  "precio": 7.50,
  "tipoId": 1,
  "tipoNombre": "Bebidas"
}
```

### TipoArticulos
```json
{
  "id": 1,
  "nombre": "Bebidas"
}
```

## ✨ Características

- ✅ CRUD completo de Artículos
- ✅ CRUD completo de Tipos de Artículos
- ✅ Paginación en tipos de artículos
- ✅ Consultas personalizadas con JPQL y SQL nativo
- ✅ DTOs para transferencia de datos
- ✅ Validaciones
- ✅ CORS configurado
- ✅ Interfaz responsiva
- ✅ Manejo de errores

## 📝 Datos de Ejemplo

El sistema incluye 13 categorías predefinidas:
- Bebidas
- Limpieza
- Snacks
- Lácteos
- Abarrotes
- Congelados
- Carnes
- Frutas y Verduras
- Panadería
- Cuidado Personal
- Mascotas
- Enlatados
- Dulcería

## 👨‍💻 Desarrollo

### Código Optimizado
El backend utiliza:
- **Streams** para procesamiento de listas
- **DTOs** para separar capa de presentación
- **Servicios** con métodos reutilizables
- **Repositorios JPA** con consultas personalizadas

### Frontend Modular
- Componentes standalone
- Servicios inyectables
- Modelos tipados
- Routing configurado
