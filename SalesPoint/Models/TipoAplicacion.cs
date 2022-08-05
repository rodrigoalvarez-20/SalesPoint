using System.ComponentModel.DataAnnotations;

namespace SalesPoint.Models {
	public class TipoAplicacion {
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "El nombre del tipo de aplicacion es requerido")]
		public string NombreAplicacion { get; set; }
	}
}
