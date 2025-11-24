package com.example.appfinal_backend.dto;

import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.AllArgsConstructor;

@Data
@NoArgsConstructor
@AllArgsConstructor
public class AgregarAlCarritoRequest {
    private Integer productoId;
    private Integer cantidad;
}
