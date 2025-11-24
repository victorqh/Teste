package com.upn.ventas.controller;

import java.net.URI;
import java.util.List;
import jakarta.validation.Valid;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.servlet.support.ServletUriComponentsBuilder;

import com.upn.ventas.dto.ArticulosDTO;
import com.upn.ventas.model.Articulos;
import com.upn.ventas.service.ArticulosServiceImp;


@CrossOrigin(origins = "http://localhost:4200")
@RestController
@RequestMapping("/articulos")
public class ArticulosController {


    @Autowired
    private ArticulosServiceImp service;


    @GetMapping
    public List<ArticulosDTO> listar() {
        return service.listarArticulosJPQL();
    }

    @GetMapping("/{id}")
    public ResponseEntity<ArticulosDTO> obtener(@PathVariable("id") Integer id) {
        return service.buscar(id).map(ResponseEntity::ok).orElse(ResponseEntity.notFound().build());
    }

    @PostMapping
    public ResponseEntity<Articulos> crear(@Valid @RequestBody Articulos nuevo) {
        int ok = service.save(nuevo);
        if (ok != 1) {
            return ResponseEntity.badRequest().build();
        }
        URI location = ServletUriComponentsBuilder.fromCurrentRequest()
                .path("/{id}")
                .buildAndExpand(nuevo.getId())
                .toUri();
        return ResponseEntity.created(location).body(nuevo);
    }

    @PutMapping("/{id}")
    public ResponseEntity<Articulos> actualizar(@PathVariable("id") Integer id,
                                                @Valid @RequestBody Articulos cambios) {
        var opt = service.listarId(id);
        if (opt.isEmpty()) {
            return ResponseEntity.notFound().build();
        }
        Articulos actual = opt.get();
        actual.setDescripcion(cambios.getDescripcion());
        actual.setPrecio(cambios.getPrecio());
        if (cambios.getTipo() != null) { // opcional: actualizar el tipo si viene
            actual.setTipo(cambios.getTipo());
        }
        
		int ok = service.save(actual);
		if (ok == 1) {
			return ResponseEntity.ok(actual);
		} else {
			return ResponseEntity.badRequest().build();
		}
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