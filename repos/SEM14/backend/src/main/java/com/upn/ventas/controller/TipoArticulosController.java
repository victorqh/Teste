package com.upn.ventas.controller;

import java.net.URI;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.servlet.support.ServletUriComponentsBuilder;

import com.upn.ventas.dto.TipoArticulosDTO;
import com.upn.ventas.model.TipoArticulos;
import com.upn.ventas.service.TipoArticulosServiceImp;

import jakarta.validation.Valid;


@CrossOrigin(origins = "http://localhost:4200")
@RestController
@RequestMapping("/tipoarticulos")
public class TipoArticulosController {

	@Autowired
	private TipoArticulosServiceImp service;
	
	@GetMapping
	public List<TipoArticulosDTO> listar(){
		return service.listar();
	}
	
	@GetMapping("/paginado")
	public ResponseEntity<Page<TipoArticulosDTO>> listarPaginado(
				@RequestParam(name="page", defaultValue = "0") int page,
				@RequestParam(name="size", defaultValue = "5") int size
			){
		Page<TipoArticulosDTO> pagina = service.listarPaginado(page, size);
		return ResponseEntity.ok(pagina);
	}
	
	//CRUD
	@GetMapping("/{id}")
	public ResponseEntity<TipoArticulosDTO> obtener(@PathVariable("id") Integer id) {
		return service.buscar(id).map(ResponseEntity::ok).orElse(ResponseEntity.notFound().build());
	}

	@PostMapping
	public ResponseEntity<TipoArticulos> crear(@Valid @RequestBody TipoArticulos nuevo) {
		int ok = service.save(nuevo);
		if (ok != 1) {
			return ResponseEntity.badRequest().build();
		}
		URI location = ServletUriComponentsBuilder.fromCurrentRequest().path("/{id}").buildAndExpand(nuevo.getId())
				.toUri();
		return ResponseEntity.created(location).body(nuevo);
	}

	@PutMapping("/{id}")
	public ResponseEntity<TipoArticulos> actualizar(@PathVariable("id") Integer id,
			@Valid @RequestBody TipoArticulos cambios) {
		var opt = service.listarId(id);
		if (opt.isEmpty()) {
			return ResponseEntity.notFound().build();
		}
		TipoArticulos actual = opt.get();
		actual.setNombre(cambios.getNombre());
		int ok = service.save(actual);
		return (ok == 1) ? ResponseEntity.ok(actual) : ResponseEntity.badRequest().build();
	}

	@DeleteMapping("/{id}")
	public ResponseEntity<Void> eliminar(@PathVariable("id") Integer id) {
		if (service.listarId(id).isEmpty()) {
			return ResponseEntity.notFound().build();
		}
		service.delete(id);
		return ResponseEntity.noContent().build();
	}
	
}
