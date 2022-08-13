using Microsoft.AspNetCore.Mvc.Rendering;

namespace SalesPoint.Models.VewModels {
	public class ProductoVM {

		public Producto Producto { get; set; }
		public IEnumerable<SelectListItem>? CategoriaLista { get; set; }
		public IEnumerable<SelectListItem>? TipoAplicacionLista { get; set; }

	}
}
