package com.example.appfinal_backend.model;

import jakarta.persistence.*;
import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.AllArgsConstructor;
import org.hibernate.annotations.CreationTimestamp;

import java.math.BigDecimal;
import java.time.LocalDateTime;

@Entity
@Table(name = "productos")
@Data
@NoArgsConstructor
@AllArgsConstructor
public class Producto {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "productoid")
    private Integer productoId;
    
    @Column(name = "nombre", nullable = false, length = 200)
    private String nombre;
    
    @Column(name = "descripcion", columnDefinition = "TEXT")
    private String descripcion;
    
    @Column(name = "precio", nullable = false, precision = 18, scale = 2)
    private BigDecimal precio;
    
    @Column(name = "precioanterior", precision = 18, scale = 2)
    private BigDecimal precioAnterior;
    
    @Column(name = "stock", nullable = false)
    private Integer stock = 0;
    
    @Column(name = "imagenurl", length = 500)
    private String imagenUrl;
    
    @Column(name = "categoriaid", nullable = false)
    private Integer categoriaId;
    
    @Column(name = "estaactivo")
    private Boolean estaActivo = true;
    
    @Column(name = "esoferta")
    private Boolean esOferta = false;
    
    @CreationTimestamp
    @Column(name = "fechacreacion", updatable = false)
    private LocalDateTime fechaCreacion;
    
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "categoriaid", insertable = false, updatable = false)
    private Categoria categoria;


    @Column(name = "mensaje", columnDefinition = "TEXT")
    private String mensaje;
}
