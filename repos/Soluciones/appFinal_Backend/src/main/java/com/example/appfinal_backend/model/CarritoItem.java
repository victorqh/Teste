package com.example.appfinal_backend.model;

import jakarta.persistence.*;
import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.AllArgsConstructor;

import java.math.BigDecimal;

@Entity
@Table(name = "carritoitems")
@Data
@NoArgsConstructor
@AllArgsConstructor
public class CarritoItem {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "carritoitemid")
    private Integer carritoItemId;
    
    @Column(name = "carritoid", nullable = false)
    private Integer carritoId;
    
    @Column(name = "productoid", nullable = false)
    private Integer productoId;
    
    @Column(name = "cantidad", nullable = false)
    private Integer cantidad = 1;
    
    @Column(name = "precio", nullable = false, precision = 18, scale = 2)
    private BigDecimal precio;
    
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "carritoid", insertable = false, updatable = false)
    private Carrito carrito;
    
    @ManyToOne(fetch = FetchType.EAGER)
    @JoinColumn(name = "productoid", insertable = false, updatable = false)
    private Producto producto;
}
