package com.example.appfinal_backend.dto;

import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.AllArgsConstructor;

import java.math.BigDecimal;
import java.time.LocalDateTime;

@Data
@NoArgsConstructor
@AllArgsConstructor
public class ProductoDTO {
    private Integer productoId;
    private String nombre;
    private String descripcion;
    private BigDecimal precio;
    private BigDecimal precioAnterior;
    private Integer stock;
    private String imagenUrl;
    private Integer categoriaId;
    private String categoriaNombre;
    private Boolean estaActivo;
    private Boolean esOferta;
    private LocalDateTime fechaCreacion;
    private String mensaje;
}
