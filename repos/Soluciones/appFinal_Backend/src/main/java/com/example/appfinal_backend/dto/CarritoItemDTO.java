package com.example.appfinal_backend.dto;

import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.AllArgsConstructor;

import java.math.BigDecimal;

@Data
@NoArgsConstructor
@AllArgsConstructor
public class CarritoItemDTO {
    private Integer carritoItemId;
    private Integer productoId;
    private String productoNombre;
    private String productoImagen;
    private BigDecimal precioUnitario;
    private Integer cantidad;
    private BigDecimal subtotal;
}
