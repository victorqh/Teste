package com.example.demo.service;

import com.example.demo.dto.LoginRequest;
import com.example.demo.dto.LoginResponse;
import com.example.demo.dto.UserDTO;
import com.example.demo.model.User;
import com.example.demo.repository.UserRepository;
import com.example.demo.security.JwtUtil;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;

@Service
public class AuthService {

    private final UserRepository userRepository;
    private final PasswordEncoder passwordEncoder;
    private final JwtUtil jwtUtil;
    private final UserService userService;

    public AuthService(UserRepository userRepository, PasswordEncoder passwordEncoder, 
                      JwtUtil jwtUtil, UserService userService) {
        this.userRepository = userRepository;
        this.passwordEncoder = passwordEncoder;
        this.jwtUtil = jwtUtil;
        this.userService = userService;
    }

    public LoginResponse login(LoginRequest request) {
        User user = userRepository.findByEmail(request.getEmail())
                .orElseThrow(() -> new RuntimeException("Credenciales inválidas"));

        if (!passwordEncoder.matches(request.getPassword(), user.getPasswordHash())) {
            throw new RuntimeException("Credenciales inválidas");
        }

        String token = jwtUtil.generateToken(user.getEmail());
        
        UserDTO userDTO = new UserDTO();
        userDTO.setUserId(user.getUserId());
        userDTO.setNombre(user.getNombre());
        userDTO.setEmail(user.getEmail());
        userDTO.setTelefono(user.getTelefono());
        userDTO.setDireccion(user.getDireccion());
        userDTO.setRol(user.getRol());
        userDTO.setFechaRegistro(user.getFechaRegistro());

        LoginResponse response = new LoginResponse();
        response.setToken(token);
        response.setUser(userDTO);
        
        return response;
    }
}
