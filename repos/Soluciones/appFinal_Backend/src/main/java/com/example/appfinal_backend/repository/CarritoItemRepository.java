package com.example.appfinal_backend.repository;

import com.example.appfinal_backend.model.CarritoItem;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.Optional;

@Repository
public interface CarritoItemRepository extends JpaRepository<CarritoItem, Integer> {
    
    Optional<CarritoItem> findByCarritoIdAndProductoId(Integer carritoId, Integer productoId);
}
