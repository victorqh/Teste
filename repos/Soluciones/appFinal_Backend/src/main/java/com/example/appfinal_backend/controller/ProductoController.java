package com.example.appfinal_backend.controller;

import com.example.appfinal_backend.dto.ProductoDTO;
import com.example.appfinal_backend.service.ProductoService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/productos")
@RequiredArgsConstructor
public class ProductoController {
    
    private final ProductoService productoService;
    
    @GetMapping
    public ResponseEntity<List<ProductoDTO>> getAllProductos() {
        List<ProductoDTO> productos = productoService.getProductosActivos();
        return ResponseEntity.ok(productos);
    }
    
    @GetMapping("/{id}")
    public ResponseEntity<ProductoDTO> getProductoById(@PathVariable Integer id) {
        try {
            ProductoDTO producto = productoService.getProductoById(id);
            return ResponseEntity.ok(producto);
        } catch (RuntimeException e) {
            return ResponseEntity.notFound().build();
        }
    }
    
    @GetMapping("/categoria/{categoriaId}")
    public ResponseEntity<List<ProductoDTO>> getProductosByCategoria(@PathVariable Integer categoriaId) {
        List<ProductoDTO> productos = productoService.getProductosByCategoria(categoriaId);
        return ResponseEntity.ok(productos);
    }
    
    @GetMapping("/ofertas")
    public ResponseEntity<List<ProductoDTO>> getProductosEnOferta() {
        List<ProductoDTO> productos = productoService.getProductosEnOferta();
        return ResponseEntity.ok(productos);
    }
    
    @GetMapping("/buscar")
    public ResponseEntity<List<ProductoDTO>> buscarProductos(@RequestParam String q) {
        List<ProductoDTO> productos = productoService.buscarProductos(q);
        return ResponseEntity.ok(productos);
    }
}
