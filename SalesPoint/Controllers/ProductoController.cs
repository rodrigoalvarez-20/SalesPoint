using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalesPoint.Datos;
using SalesPoint.Models;
using SalesPoint.Models.VewModels;

namespace SalesPoint.Controllers {
	public class ProductoController : Controller {

		private readonly AppDbContext _db;

		public ProductoController(AppDbContext db) {
			_db = db;
		}

		public IActionResult Index() {
			IEnumerable<Producto> prod_list = _db.Producto
				.Include(c => c.Categoria)
				.Include(t => t.TipoAplicacion);
			return View(prod_list);
		}


		//Get

		public IActionResult Upsert(int? Id) {

			//IEnumerable<SelectListItem> dwCategoria = _db.Categoria.Select(c => new SelectListItem {
			//	Text = c.NombreCategoria,
			//	Value = c.Id.ToString()
			//});

			//ViewBag.dwCategoria = dwCategoria;

			//Producto prod = new Producto();

			IEnumerable<SelectListItem> dwCategoria = _db.Categoria.Select(c => new SelectListItem {
				Text = c.NombreCategoria,
				Value = c.Id.ToString()
			});

			IEnumerable<SelectListItem> dwTipoApp = _db.TipoAplicacion.Select(c => new SelectListItem {
				Text = c.NombreAplicacion,
				Value = c.Id.ToString()
			});

			ProductoVM prodVM = new ProductoVM() {
				Producto = new Producto(),
				CategoriaLista = dwCategoria,
				TipoAplicacionLista = dwTipoApp
			};

			if (Id == null) {
				// Creando nuevo registro
				return View(prod);
			}else {
				prod = _db.Producto.Find(Id);
				if (prod == null) { 
					return NotFound();
				}

				return View(prod);

			}
		}

	}
}
