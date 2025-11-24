package com.example.appfinal_backend.dto;

import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.AllArgsConstructor;

@Data
@NoArgsConstructor
@AllArgsConstructor
public class CreateUserRequest {
    
    private String nombre;
    private String email;
    private String password;
    private String telefono;
    private String direccion;
    private String rol;
}
