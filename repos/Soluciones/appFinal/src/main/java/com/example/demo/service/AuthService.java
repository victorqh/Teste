package com.example.demo.service;

import com.example.demo.dto.LoginRequest;
import com.example.demo.dto.LoginResponse;
import com.example.demo.model.User;
import com.example.demo.repository.UserRepository;
import com.example.demo.security.JwtUtil;
import lombok.RequiredArgsConstructor;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;

@Service
@RequiredArgsConstructor
public class AuthService {
    
    private final UserRepository userRepository;
    private final PasswordEncoder passwordEncoder;
    private final JwtUtil jwtUtil;
    
    public LoginResponse login(LoginRequest request) {
        // Validar que username y password no estén vacíos
        if (request.getUsername() == null || request.getUsername().trim().isEmpty() ||
            request.getPassword() == null || request.getPassword().isEmpty()) {
            return new LoginResponse(false, "username/password requeridos");
        }
        
        // Buscar usuario por email (usando email como username)
        User user = userRepository.findByEmail(request.getUsername())
                .orElse(null);
        
        if (user == null) {
            return new LoginResponse(false, "Credenciales inválidas");
        }
        
        // Verificar contraseña
        if (!passwordEncoder.matches(request.getPassword(), user.getPasswordHash())) {
            return new LoginResponse(false, "Credenciales inválidas");
        }
        
        // Generar token JWT
        String token = jwtUtil.generateToken(user.getEmail(), user.getUserId());
        
        // Crear respuesta exitosa
        LoginResponse.UserInfo userInfo = new LoginResponse.UserInfo(
            user.getUserId(),
            user.getEmail()
        );
        
        return new LoginResponse(true, token, userInfo);
    }
}
