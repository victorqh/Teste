package com.example.appfinal_backend.repository;

import com.example.appfinal_backend.model.Carrito;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.Optional;

@Repository
public interface CarritoRepository extends JpaRepository<Carrito, Integer> {
    
    Optional<Carrito> findByUserId(Integer userId);
}
