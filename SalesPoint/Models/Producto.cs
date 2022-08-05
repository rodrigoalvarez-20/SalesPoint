using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesPoint.Models {
	public class Producto {

		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "El nombre del producto es requerido")]
		public string Name { get; set; }

		[Required(ErrorMessage = "La descripcion corta del producto es requerida")]
		[MaxLength(100)]
		public string DescripcionCorta { get; set; }

		[Required(ErrorMessage = "La descripcion completa del producto es requerida")]
		public string DescripcionProducto { get; set; }

		[Required(ErrorMessage = "El precio del producto es necesario")]
		[Range(1, double.MaxValue, ErrorMessage = "El precio debe de ser mayor a 0")]
		public double Precio { get; set; }

		public string UrlImagen { get; set; }

		// Foreign Keys

		public int CategoriaId { get; set; }
		[ForeignKey("CategoriaId")]
		public virtual Categoria Categoria { get; set; }

		public int TipoAplicacionId { get; set; }
		[ForeignKey("TipoAplicacionId")]
		public virtual TipoAplicacion TipoAplicacion { get; set; }

	}
}
