package com.example.appfinal_backend.model;

import jakarta.persistence.*;
import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.AllArgsConstructor;
import org.hibernate.annotations.CreationTimestamp;

import java.time.LocalDateTime;

@Entity
@Table(name = "users")
@Data
@NoArgsConstructor
@AllArgsConstructor
public class User {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "userid")
    private Integer userId;
    
    @Column(name = "nombre", nullable = false, length = 120)
    private String nombre;
    
    @Column(name = "email", nullable = false, unique = true, length = 120)
    private String email;
    
    @Column(name = "passwordhash", nullable = false, length = 500)
    private String passwordHash;
    
    @Column(name = "telefono", length = 20)
    private String telefono;
    
    @Column(name = "direccion", length = 200)
    private String direccion;
    
    @Column(name = "rol", nullable = false, length = 20)
    private String rol = "Cliente";
    
    @CreationTimestamp
    @Column(name = "fecharegistro", nullable = false, updatable = false)
    private LocalDateTime fechaRegistro;
}
