package com.example.appfinal_backend.service;

import com.example.appfinal_backend.dto.ProductoDTO;
import com.example.appfinal_backend.model.Producto;
import com.example.appfinal_backend.repository.ProductoRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
public class ProductoService {
    
    private final ProductoRepository productoRepository;
    
    public List<ProductoDTO> getAllProductos() {
        return productoRepository.findAll().stream()
                .map(this::convertToDTO)
                .collect(Collectors.toList());
    }
    
    public List<ProductoDTO> getProductosActivos() {
        return productoRepository.findByEstaActivoTrue().stream()
                .map(this::convertToDTO)
                .collect(Collectors.toList());
    }
    
    public List<ProductoDTO> getProductosEnOferta() {
        return productoRepository.findByEsOfertaTrue().stream()
                .map(this::convertToDTO)
                .collect(Collectors.toList());
    }
    
    public ProductoDTO getProductoById(Integer id) {
        Producto producto = productoRepository.findById(id)
                .orElseThrow(() -> new RuntimeException("Producto no encontrado"));
        return convertToDTO(producto);
    }
    
    public List<ProductoDTO> getProductosByCategoria(Integer categoriaId) {
        return productoRepository.findByCategoriaId(categoriaId).stream()
                .map(this::convertToDTO)
                .collect(Collectors.toList());
    }
    
    public List<ProductoDTO> buscarProductos(String nombre) {
        return productoRepository.findByNombreContainingIgnoreCase(nombre).stream()
                .map(this::convertToDTO)
                .collect(Collectors.toList());
    }
    
    public ProductoDTO createProducto(ProductoDTO productoDTO) {
        Producto producto = new Producto();
        producto.setNombre(productoDTO.getNombre());
        producto.setDescripcion(productoDTO.getDescripcion());
        producto.setPrecio(productoDTO.getPrecio());
        producto.setStock(productoDTO.getStock());
        producto.setImagenUrl(productoDTO.getImagenUrl());
        producto.setCategoriaId(productoDTO.getCategoriaId());
        producto.setEstaActivo(productoDTO.getEstaActivo());
        producto.setEsOferta(productoDTO.getEsOferta());
        
        Producto savedProducto = productoRepository.save(producto);
        return convertToDTO(savedProducto);
    }
    
    public ProductoDTO updateProducto(Integer id, ProductoDTO productoDTO) {
        Producto producto = productoRepository.findById(id)
                .orElseThrow(() -> new RuntimeException("Producto no encontrado"));
        
        producto.setNombre(productoDTO.getNombre());
        producto.setDescripcion(productoDTO.getDescripcion());
        producto.setPrecio(productoDTO.getPrecio());
        producto.setStock(productoDTO.getStock());
        producto.setImagenUrl(productoDTO.getImagenUrl());
        producto.setCategoriaId(productoDTO.getCategoriaId());
        producto.setEstaActivo(productoDTO.getEstaActivo());
        producto.setEsOferta(productoDTO.getEsOferta());
        
        Producto updatedProducto = productoRepository.save(producto);
        return convertToDTO(updatedProducto);
    }
    
    public void deleteProducto(Integer id) {
        if (!productoRepository.existsById(id)) {
            throw new RuntimeException("Producto no encontrado");
        }
        productoRepository.deleteById(id);
    }
    
    private ProductoDTO convertToDTO(Producto producto) {
        return new ProductoDTO(
                producto.getProductoId(),
                producto.getNombre(),
                producto.getDescripcion(),
                producto.getPrecio(),
                producto.getPrecioAnterior(),
                producto.getStock(),
                producto.getImagenUrl(),
                producto.getCategoriaId(),
                producto.getCategoria() != null ? producto.getCategoria().getNombre() : null,
                producto.getEstaActivo(),
                producto.getEsOferta(),
                producto.getFechaCreacion()
        );
    }
}
