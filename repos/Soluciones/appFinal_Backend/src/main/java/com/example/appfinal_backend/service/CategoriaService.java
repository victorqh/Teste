package com.example.appfinal_backend.service;

import com.example.appfinal_backend.dto.CategoriaDTO;
import com.example.appfinal_backend.model.Categoria;
import com.example.appfinal_backend.repository.CategoriaRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
public class CategoriaService {
    
    private final CategoriaRepository categoriaRepository;
    
    public List<CategoriaDTO> getAllCategorias() {
        return categoriaRepository.findAll().stream()
                .map(this::convertToDTO)
                .collect(Collectors.toList());
    }
    
    public CategoriaDTO getCategoriaById(Integer id) {
        Categoria categoria = categoriaRepository.findById(id)
                .orElseThrow(() -> new RuntimeException("Categoría no encontrada"));
        return convertToDTO(categoria);
    }
    
    private CategoriaDTO convertToDTO(Categoria categoria) {
        return new CategoriaDTO(
                categoria.getCategoriaId(),
                categoria.getNombre(),
                categoria.getDescripcion()
        );
    }
}
