# ?? SOLUCIÓN: Error de Autenticación PostgreSQL

## ? Error Actual:
```
28P01: la autentificación password falló para el usuario 'postgres'
```

## ? SOLUCIONES (Prueba en orden):

### Solución 1: Probar contraseñas comunes

Edita `appsettings.json` y prueba estas contraseñas una por una:

1. **postgres** (más común)
```json
"Password=postgres"
```

2. **admin** (la que tienes ahora)
```json
"Password=admin"
```

3. **root**
```json
"Password=root"
```

4. **1234**
```json
"Password=1234"
```

5. **Sin contraseña** (algunos instaladores)
```json
"Password="
```

Después de cada cambio, ejecuta:
```bash
dotnet ef database update
```

---

### Solución 2: Resetear la contraseña de PostgreSQL (Recomendado)

#### En Windows:

1. **Buscar el archivo pg_hba.conf**
   - Ubicación típica: `C:\Program Files\PostgreSQL\[versión]\data\pg_hba.conf`
   - O busca en: `C:\Program Files\PostgreSQL\`

2. **Editar pg_hba.conf** (como Administrador)
   
   Busca estas líneas:
   ```
   # IPv4 local connections:
   host    all             all             127.0.0.1/32            scram-sha-256
   ```
   
   Cámbialo temporalmente a:
   ```
   # IPv4 local connections:
   host    all             all             127.0.0.1/32            trust
   ```

3. **Reiniciar PostgreSQL**
   - Abre "Servicios" de Windows (Win + R ? `services.msc`)
   - Busca "postgresql-x64-[versión]"
   - Click derecho ? Reiniciar

4. **Cambiar la contraseña**
   
   Abre CMD o PowerShell como Administrador:
   ```bash
   psql -U postgres
   ```
   
   Dentro de psql:
   ```sql
   ALTER USER postgres WITH PASSWORD 'tuNuevaContraseña';
   \q
   ```

5. **Revertir pg_hba.conf**
   
   Vuelve a cambiar:
   ```
   host    all             all             127.0.0.1/32            scram-sha-256
   ```

6. **Reiniciar PostgreSQL de nuevo**

7. **Actualizar appsettings.json**
   ```json
   "Password=tuNuevaContraseña"
   ```

---

### Solución 3: Verificar puerto y servicio

1. **Verificar que PostgreSQL esté corriendo:**
   - Abre "Servicios" (Win + R ? `services.msc`)
   - Busca "postgresql-x64-[versión]"
   - Estado debe ser "En ejecución"

2. **Verificar el puerto:**
   
   Por defecto es 5432, pero puede ser diferente. Verifica en:
   `C:\Program Files\PostgreSQL\[versión]\data\postgresql.conf`
   
   Busca:
   ```
   port = 5432
   ```
   
   Si es diferente, actualiza tu connection string:
   ```json
   "Host=localhost;Port=TU_PUERTO;Database=appFinalDb;Username=postgres;Password=admin"
   ```

---

### Solución 4: Usar pgAdmin para verificar

1. Abre **pgAdmin 4**
2. Intenta conectar al servidor con diferentes contraseñas
3. Una vez que funcione en pgAdmin, usa esa misma contraseña en `appsettings.json`

---

### Solución 5: Reinstalar PostgreSQL (Última opción)

Si nada funciona:

1. Desinstala PostgreSQL completamente
2. Descarga desde: https://www.postgresql.org/download/windows/
3. Durante la instalación:
   - **Anota la contraseña que configures**
   - Puerto: 5432
   - Locale: Spanish, Spain
4. Actualiza `appsettings.json` con la nueva contraseña

---

## ?? SOLUCIÓN RÁPIDA RECOMENDADA:

### Paso 1: Probar con contraseña común
Edita `appsettings.json` y cambia a:
```json
"DefaultConnection": "Host=localhost;Database=appFinalDb;Username=postgres;Password=postgres"
```

### Paso 2: Ejecutar
```bash
dotnet ef database update
```

### Si no funciona ? Ve a Solución 2 (Resetear contraseña)

---

## ?? Configuraciones Alternativas

Si tienes PostgreSQL en Docker o instalación personalizada:

### Docker:
```json
"Host=localhost;Port=5432;Database=appFinalDb;Username=postgres;Password=postgres"
```

### PostgreSQL con SSL:
```json
"Host=localhost;Database=appFinalDb;Username=postgres;Password=admin;SSL Mode=Require"
```

### Sin SSL (más permisivo):
```json
"Host=localhost;Database=appFinalDb;Username=postgres;Password=admin;SSL Mode=Disable"
```

---

## ? Verificar que funcionó:

Una vez que ejecutes `dotnet ef database update` sin errores, verás:
```
Build started...
Build succeeded.
Done.
```

Y en pgAdmin verás la base de datos `appFinalDb` con las tablas:
- AspNetUsers
- AspNetRoles
- AspNetUserRoles
- etc.

---

## ?? Si sigues con problemas:

1. Comparte la salida de:
   ```bash
   psql --version
   ```

2. Verifica la ubicación de PostgreSQL:
   ```bash
   where psql
   ```

3. Prueba conectar directamente:
   ```bash
   psql -U postgres -h localhost
   ```
   (Te pedirá la contraseña - prueba las que mencionamos arriba)

---

## ?? Consejo:

La contraseña más común que se olvida es la que pusiste durante la instalación de PostgreSQL. 
Si no la recuerdas, la **Solución 2** (resetear contraseña) es tu mejor opción.
