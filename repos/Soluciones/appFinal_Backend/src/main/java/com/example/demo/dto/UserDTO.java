package com.example.demo.dto;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

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
