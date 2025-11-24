package com.upn.ventas.dto;

public class ArticulosDTO {

	private Integer id;
	private String descripcion;
	private double precio;
	private Integer tipoId;
	private String tipoNombre;

	public Integer getId() {
		return id;
	}

	public void setId(Integer id) {
		this.id = id;
	}

	public String getDescripcion() {
		return descripcion;
	}

	public void setDescripcion(String descripcion) {

		this.descripcion = descripcion;

	}

	public double getPrecio() {

		return precio;

	}

	public void setPrecio(double precio) {

		this.precio = precio;

	}

	public Integer getTipoId() {

		return tipoId;

	}

	public void setTipoId(Integer tipoId) {

		this.tipoId = tipoId;

	}

	public String getTipoNombre() {

		return tipoNombre;

	}

	public void setTipoNombre(String tipoNombre) {

		this.tipoNombre = tipoNombre;

	}

}