package com.example.appfinal_backend.repository;

import com.example.appfinal_backend.model.Producto;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface ProductoRepository extends JpaRepository<Producto, Integer> {
    
    List<Producto> findByCategoriaId(Integer categoriaId);
    
    List<Producto> findByEstaActivoTrue();
    
    List<Producto> findByEsOfertaTrue();
    
    List<Producto> findByNombreContainingIgnoreCase(String nombre);
}
