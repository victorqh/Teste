-- Script para actualizar contraseñas existentes a BCrypt
-- IMPORTANTE: Esto establecerá la contraseña "Password123" para todos los usuarios
-- Los usuarios deberán cambiarla después

-- Ejemplo de hash BCrypt para la contraseña "Password123"
-- Puedes generar más hashes aquí: https://bcrypt-generator.com/

UPDATE users 
SET passwordhash = '$2a$11$rGxQxVzVzQ7ZqH.GvMxFkOnFzH9k9wZ5yL3qM5MYN5e1Y3JhZ8pte'
WHERE LENGTH(passwordhash) < 50;

-- Si quieres contraseñas diferentes por usuario, genera un hash para cada uno:
-- Password123 -> $2a$11$rGxQxVzVzQ7ZqH.GvMxFkOnFzH9k9wZ5yL3qM5MYN5e1Y3JhZ8pte

-- Verifica cuántos usuarios se actualizaron
SELECT COUNT(*) as usuarios_actualizados FROM users WHERE LENGTH(passwordhash) >= 50;
