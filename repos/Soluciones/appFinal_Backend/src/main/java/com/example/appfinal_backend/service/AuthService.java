package com.example.appfinal_backend.service;

import com.example.appfinal_backend.dto.LoginRequest;
import com.example.appfinal_backend.dto.LoginResponse;
import com.example.appfinal_backend.model.User;
import com.example.appfinal_backend.repository.UserRepository;
import com.example.appfinal_backend.security.JwtUtil;
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
        User user = userRepository.findByEmail(request.getEmail())
                .orElseThrow(() -> new RuntimeException("Credenciales inválidas"));
        
        if (!passwordEncoder.matches(request.getPassword(), user.getPasswordHash())) {
            throw new RuntimeException("Credenciales inválidas");
        }
        
        String token = jwtUtil.generateToken(user.getEmail(), user.getUserId(), user.getRol());
        
        LoginResponse.UserInfo userInfo = new LoginResponse.UserInfo(
                user.getUserId(),
                user.getNombre(),
                user.getEmail(),
                user.getRol()
        );
        
        return new LoginResponse(token, userInfo);
    }
}
