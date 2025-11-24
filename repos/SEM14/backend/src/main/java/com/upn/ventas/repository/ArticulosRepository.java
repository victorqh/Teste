package com.upn.ventas.repository;

import com.upn.ventas.model.Articulos;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface ArticulosRepository extends JpaRepository<Articulos, Integer> {

	/*
	 * Listado de Articulos con el campo nombre del Tipo Articulo
	 */

	@Query("SELECT a FROM Articulos a JOIN a.tipo t ORDER BY a.id")
	List<Articulos> listarArticulosJPQL();

	@Query(value = "SELECT a.* " +
			"FROM articulos a " +
			"INNER JOIN tipo_articulos t ON t.id = a.tipo_id "
			+ "ORDER BY a.id", nativeQuery = true)

	List<Articulos> listarArticulosSQL();

	/*
	 * 
	 * Busqueda de Articulos por ID con el campo nombre del Tipo Articulo
	 * 
	 */

	@Query("SELECT a FROM Articulos a JOIN a.tipo t WHERE a.id = :id")
	Articulos buscarArticuloIdJPQL(@Param("id") Integer id);

	@Query(value = "SELECT a.* " +
			"FROM articulos a " +
			"INNER JOIN tipo_articulos t ON t.id = a.tipo_id " +
			"WHERE a.id = :id", nativeQuery = true)

	Articulos buscarArticuloIdSQL(@Param("id") Integer id);

	/*
	 * 
	 * Busqueda de Articulos por la descripcion con el campo nombre del Tipo
	 * Articulo
	 * 
	 */

	@Query("SELECT a FROM Articulos a JOIN a.tipo t WHERE a.descripcion LIKE %:descripcion% ORDER BY a.id")
	List<Articulos> buscarArticuloDescJPQL(@Param("descripcion") String descripcion);

	@Query(value = "SELECT a.* " +
			"FROM articulos a " +
			"INNER JOIN tipo_articulos t ON t.id = a.tipo_id "
			+ "WHERE a.descripcion LIKE CONCAT('%', :descripcion, '%') " +
			"ORDER BY a.id", nativeQuery = true)
	List<Articulos> buscarArticuloDescSQL(@Param("descripcion") String descripcion);

}