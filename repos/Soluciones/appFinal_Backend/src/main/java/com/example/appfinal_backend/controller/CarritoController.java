package com.example.appfinal_backend.controller;

import com.example.appfinal_backend.dto.AgregarAlCarritoRequest;
import com.example.appfinal_backend.dto.CarritoDTO;
import com.example.appfinal_backend.security.JwtUtil;
import com.example.appfinal_backend.service.CarritoService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api/carrito")
@RequiredArgsConstructor
public class CarritoController {
    
    private final CarritoService carritoService;
    private final JwtUtil jwtUtil;
    
    @GetMapping
    public ResponseEntity<CarritoDTO> getCarrito(@RequestHeader("Authorization") String token) {
        Integer userId = getUserIdFromToken(token);
        CarritoDTO carrito = carritoService.getCarritoByUserId(userId);
        return ResponseEntity.ok(carrito);
    }
    
    @PostMapping("/agregar")
    public ResponseEntity<CarritoDTO> agregarAlCarrito(
            @RequestHeader("Authorization") String token,
            @RequestBody AgregarAlCarritoRequest request) {
        try {
            Integer userId = getUserIdFromToken(token);
            CarritoDTO carrito = carritoService.agregarAlCarrito(userId, request);
            return ResponseEntity.ok(carrito);
        } catch (RuntimeException e) {
            return ResponseEntity.badRequest().build();
        }
    }
    
    @PutMapping("/item/{carritoItemId}")
    public ResponseEntity<CarritoDTO> actualizarCantidad(
            @RequestHeader("Authorization") String token,
            @PathVariable Integer carritoItemId,
            @RequestParam Integer cantidad) {
        try {
            Integer userId = getUserIdFromToken(token);
            CarritoDTO carrito = carritoService.actualizarCantidad(userId, carritoItemId, cantidad);
            return ResponseEntity.ok(carrito);
        } catch (RuntimeException e) {
            return ResponseEntity.badRequest().build();
        }
    }
    
    @DeleteMapping("/item/{carritoItemId}")
    public ResponseEntity<Void> eliminarItem(
            @RequestHeader("Authorization") String token,
            @PathVariable Integer carritoItemId) {
        try {
            Integer userId = getUserIdFromToken(token);
            carritoService.eliminarItem(userId, carritoItemId);
            return ResponseEntity.noContent().build();
        } catch (RuntimeException e) {
            return ResponseEntity.notFound().build();
        }
    }
    
    @DeleteMapping("/vaciar")
    public ResponseEntity<Void> vaciarCarrito(@RequestHeader("Authorization") String token) {
        try {
            Integer userId = getUserIdFromToken(token);
            carritoService.vaciarCarrito(userId);
            return ResponseEntity.noContent().build();
        } catch (RuntimeException e) {
            return ResponseEntity.notFound().build();
        }
    }
    
    private Integer getUserIdFromToken(String token) {
        String jwtToken = token.substring(7); // Remove "Bearer "
        return jwtUtil.getUserIdFromToken(jwtToken);
    }
}
