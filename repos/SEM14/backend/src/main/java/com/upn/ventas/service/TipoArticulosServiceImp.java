package com.upn.ventas.service;

import java.util.List;
import java.util.Optional;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;

import com.upn.ventas.dto.TipoArticulosDTO;
import com.upn.ventas.model.TipoArticulos;
import com.upn.ventas.repository.TipoArticulosRepository;

@Service
public class TipoArticulosServiceImp implements TipoArticulosService {

	@Autowired
	private TipoArticulosRepository repository;
	
	//solo para presentacion de datos
	@Override
	public List<TipoArticulosDTO> listar() {
		return repository.findAll().stream()
				.map(this::convertirADTO)
				.toList();
	}

	//solo para presentacion de datos
	@Override
	public Optional<TipoArticulosDTO> buscar(int id) {
	    
	    return repository.findById(id).map(this::convertirADTO);
	}
	
	//solo para presentacion de datos
	@Override
	public Page<TipoArticulosDTO> listarPaginado(int page, int size) {
		
		Pageable pageable = PageRequest.of(page, size);
		Page<TipoArticulos> pagina = repository.findAll(pageable);
		
		return pagina.map(this::convertirADTO);
	}

	private TipoArticulosDTO convertirADTO(TipoArticulos tipo) {
		TipoArticulosDTO dto = new TipoArticulosDTO();
		dto.setId(tipo.getId());
		dto.setNombre(tipo.getNombre());
		return dto;
	}
	
	//CRUD
	@Override
	public Optional<TipoArticulos> listarId(Integer id) {
		return repository.findById(id);
	}

	@Override
	public TipoArticulos getId(Integer id) {
		return repository.findById(id).orElse(null);
	}

	@Override
	public int save(TipoArticulos t) {
		int band = 0;
		TipoArticulos saved = repository.save(t);
		if (saved != null) {
			band = 1;
		}
		return band;
	}

	@Override
	public void delete(Integer id) {
		if (repository.existsById(id)) {
			repository.deleteById(id);
		}
	}
	
}
