package com.upn.ventas.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import com.upn.ventas.model.TipoArticulos;

@Repository
public interface TipoArticulosRepository extends JpaRepository<TipoArticulos, Integer> {
	
	
}
