package com.example.appfinal_backend.service;

import com.example.appfinal_backend.dto.AgregarAlCarritoRequest;
import com.example.appfinal_backend.dto.CarritoDTO;
import com.example.appfinal_backend.dto.CarritoItemDTO;
import com.example.appfinal_backend.model.Carrito;
import com.example.appfinal_backend.model.CarritoItem;
import com.example.appfinal_backend.model.Producto;
import com.example.appfinal_backend.repository.CarritoItemRepository;
import com.example.appfinal_backend.repository.CarritoRepository;
import com.example.appfinal_backend.repository.ProductoRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.util.List;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
public class CarritoService {
    
    private final CarritoRepository carritoRepository;
    private final CarritoItemRepository carritoItemRepository;
    private final ProductoRepository productoRepository;
    
    public CarritoDTO getCarritoByUserId(Integer userId) {
        Carrito carrito = carritoRepository.findByUserId(userId)
                .orElseGet(() -> crearNuevoCarrito(userId));
        return convertToDTO(carrito);
    }
    
    @Transactional
    public CarritoDTO agregarAlCarrito(Integer userId, AgregarAlCarritoRequest request) {
        Carrito carrito = carritoRepository.findByUserId(userId)
                .orElseGet(() -> crearNuevoCarrito(userId));
        
        Producto producto = productoRepository.findById(request.getProductoId())
                .orElseThrow(() -> new RuntimeException("Producto no encontrado"));
        
        if (producto.getStock() < request.getCantidad()) {
            throw new RuntimeException("Stock insuficiente");
        }
        
        CarritoItem itemExistente = carritoItemRepository
                .findByCarritoIdAndProductoId(carrito.getCarritoId(), request.getProductoId())
                .orElse(null);
        
        if (itemExistente != null) {
            itemExistente.setCantidad(itemExistente.getCantidad() + request.getCantidad());
            carritoItemRepository.save(itemExistente);
        } else {
            CarritoItem nuevoItem = new CarritoItem();
            nuevoItem.setCarritoId(carrito.getCarritoId());
            nuevoItem.setProductoId(request.getProductoId());
            nuevoItem.setCantidad(request.getCantidad());
            nuevoItem.setPrecio(producto.getPrecio());
            carritoItemRepository.save(nuevoItem);
        }
        
        carrito = carritoRepository.findById(carrito.getCarritoId()).orElseThrow();
        return convertToDTO(carrito);
    }
    
    @Transactional
    public CarritoDTO actualizarCantidad(Integer userId, Integer carritoItemId, Integer cantidad) {
        Carrito carrito = carritoRepository.findByUserId(userId)
                .orElseThrow(() -> new RuntimeException("Carrito no encontrado"));
        
        CarritoItem item = carritoItemRepository.findById(carritoItemId)
                .orElseThrow(() -> new RuntimeException("Item no encontrado"));
        
        if (cantidad <= 0) {
            carritoItemRepository.delete(item);
        } else {
            item.setCantidad(cantidad);
            carritoItemRepository.save(item);
        }
        
        carrito = carritoRepository.findById(carrito.getCarritoId()).orElseThrow();
        return convertToDTO(carrito);
    }
    
    @Transactional
    public void eliminarItem(Integer userId, Integer carritoItemId) {
        Carrito carrito = carritoRepository.findByUserId(userId)
                .orElseThrow(() -> new RuntimeException("Carrito no encontrado"));
        
        CarritoItem item = carritoItemRepository.findById(carritoItemId)
                .orElseThrow(() -> new RuntimeException("Item no encontrado"));
        
        carritoItemRepository.delete(item);
    }
    
    @Transactional
    public void vaciarCarrito(Integer userId) {
        Carrito carrito = carritoRepository.findByUserId(userId)
                .orElseThrow(() -> new RuntimeException("Carrito no encontrado"));
        
        carritoItemRepository.deleteAll(carrito.getItems());
    }
    
    private Carrito crearNuevoCarrito(Integer userId) {
        Carrito carrito = new Carrito();
        carrito.setUserId(userId);
        return carritoRepository.save(carrito);
    }
    
    private CarritoDTO convertToDTO(Carrito carrito) {
        List<CarritoItemDTO> items = carrito.getItems().stream()
                .map(this::convertItemToDTO)
                .collect(Collectors.toList());
        
        BigDecimal total = items.stream()
                .map(CarritoItemDTO::getSubtotal)
                .reduce(BigDecimal.ZERO, BigDecimal::add);
        
        Integer cantidadItems = items.stream()
                .mapToInt(CarritoItemDTO::getCantidad)
                .sum();
        
        return new CarritoDTO(
                carrito.getCarritoId(),
                carrito.getUserId(),
                carrito.getFechaCreacion(),
                items,
                total,
                cantidadItems
        );
    }
    
    private CarritoItemDTO convertItemToDTO(CarritoItem item) {
        BigDecimal subtotal = item.getPrecio().multiply(new BigDecimal(item.getCantidad()));
        
        return new CarritoItemDTO(
                item.getCarritoItemId(),
                item.getProductoId(),
                item.getProducto() != null ? item.getProducto().getNombre() : null,
                item.getProducto() != null ? item.getProducto().getImagenUrl() : null,
                item.getPrecio(),
                item.getCantidad(),
                subtotal
        );
    }
}
