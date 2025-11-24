package com.upn.ventas.service;

import java.util.List;
import java.util.Optional;

import com.upn.ventas.dto.ArticulosDTO;
import com.upn.ventas.model.Articulos;

public interface ArticulosService {
	
    // JOINs (JPQL / SQL nativo)
    Optional<ArticulosDTO> buscar(Integer id);
	
    List<ArticulosDTO> listarArticulosJPQL();
    List<ArticulosDTO> listarArticulosSQL();

    ArticulosDTO buscarArticuloIdJPQL(Integer id);
    ArticulosDTO buscarArticuloIdSQL(Integer id);

    List<ArticulosDTO> buscarArticuloDescJPQL(String descripcion);
    List<ArticulosDTO> buscarArticuloDescSQL(String descripcion);
	
	
    // CRUD básico
    List<Articulos> listar();
    Optional<Articulos> listarId(Integer id);
    Articulos getId(Integer id);
    int save(Articulos a);
    void delete(Integer id);

}
