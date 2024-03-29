﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SalesPoint.Models;

namespace SalesPoint.Datos {
	public class AppDbContext : IdentityDbContext {
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { 
		
		}

		//Aqui se mapean todas las tablas
		// El nombre de cada propiedad va a ser el nombre de la tabla
		public DbSet<Categoria> Categoria { get; set; }
		public DbSet<TipoAplicacion> TipoAplicacion { get; set; }

		public DbSet<Producto> Producto { get; set; }

		public DbSet<UsuarioAplicacion> UsuarioAplicacion { get; set; }

	}
}
