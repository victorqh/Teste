package com.example.appfinal_backend.dto;

import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.AllArgsConstructor;

import java.time.LocalDateTime;

@Data
@NoArgsConstructor
@AllArgsConstructor
public class UserDTO {
    
    private Integer userId;
    private String nombre;
    private String email;
    private String telefono;
    private String direccion;
    private String rol;
    private LocalDateTime fechaRegistro;
}
