using System.ComponentModel.DataAnnotations;

namespace SalesPoint.Models {
	public class Categoria {

		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "El nombre de la categoria es necesario")]
		public string NombreCategoria { get; set; }

		[Required(ErrorMessage = "El valor de orden es necesario")]
		[Range(1, int.MaxValue, ErrorMessage ="El valor debe ser mayor a 0")]
		public int MostrarOrden { get; set; }

	}
}
