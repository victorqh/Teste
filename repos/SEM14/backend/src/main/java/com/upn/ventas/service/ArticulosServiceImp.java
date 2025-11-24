package com.upn.ventas.service;

import java.util.List;
import java.util.Optional;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.upn.ventas.dto.ArticulosDTO;
import com.upn.ventas.model.Articulos;
import com.upn.ventas.repository.ArticulosRepository;

@Service
public class ArticulosServiceImp implements ArticulosService {

	@Autowired
    private ArticulosRepository repository;

	

    // -------- CRUD --------
    @Override
    public List<Articulos> listar() {
        return repository.findAll();
    }

    @Override
    public Optional<Articulos> listarId(Integer id) {
        return repository.findById(id);
    }

    @Override
    public Articulos getId(Integer id) {
        Optional<Articulos> a = repository.findById(id);
        return a.orElse(null);
    }

    @Override
    public int save(Articulos a) {
        int band = 0;
        Articulos saved = repository.save(a);
        if (saved != null) {
            band = 1;
        }
        return band;
    }

    @Override
    public void delete(Integer id) {
    	repository.deleteById(id);
    }

    //Presentacion de datos
    // -------- JOINs (delegan al repositorio) --------
    @Override
    public Optional<ArticulosDTO> buscar(Integer id){
        return repository.findById(id).map(this::convertirADTO);
    }
    
    @Override
    public List<ArticulosDTO> listarArticulosJPQL() {
        return convertirListaADTO(repository.listarArticulosJPQL());
    }

    @Override
    public List<ArticulosDTO> listarArticulosSQL() {
        return convertirListaADTO(repository.listarArticulosSQL());
    }

    @Override
    public ArticulosDTO buscarArticuloIdJPQL(Integer id) {
        return convertirADTO(repository.buscarArticuloIdJPQL(id));
    }

    @Override
    public ArticulosDTO buscarArticuloIdSQL(Integer id) {
        return convertirADTO(repository.buscarArticuloIdSQL(id));
    }

    @Override
    public List<ArticulosDTO> buscarArticuloDescJPQL(String descripcion) {
        return convertirListaADTO(repository.buscarArticuloDescJPQL(descripcion));
    }

    @Override
    public List<ArticulosDTO> buscarArticuloDescSQL(String descripcion) {
        return convertirListaADTO(repository.buscarArticuloDescSQL(descripcion));
    }
    
    // Método helper para convertir listas
    private List<ArticulosDTO> convertirListaADTO(List<Articulos> articulos) {
        return articulos.stream()
                .map(this::convertirADTO)
                .toList();
    }
    
    private ArticulosDTO convertirADTO(Articulos articulo) {
        ArticulosDTO dto = new ArticulosDTO();
        dto.setId(articulo.getId());
        dto.setDescripcion(articulo.getDescripcion());
        dto.setPrecio(articulo.getPrecio());
        
        if (articulo.getTipo() != null) {
            dto.setTipoId(articulo.getTipo().getId());
            dto.setTipoNombre(articulo.getTipo().getNombre());
        }
        
        return dto;
    }

}
