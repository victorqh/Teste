package com.upn.ventas.service;

import java.util.*;

import org.springframework.data.domain.Page;

import com.upn.ventas.dto.TipoArticulosDTO;
import com.upn.ventas.model.TipoArticulos;

public interface TipoArticulosService {

	//Para Presentacion
	public List<TipoArticulosDTO> listar();
	public Optional<TipoArticulosDTO> buscar(int id);
	public Page<TipoArticulosDTO> listarPaginado(int page, int size);
	
	//Para CRUD
	Optional<TipoArticulos> listarId(Integer id);
	TipoArticulos getId(Integer id);
	int save(TipoArticulos t); // 1 si guardó, 0 si no
	void delete(Integer id);
	
}

