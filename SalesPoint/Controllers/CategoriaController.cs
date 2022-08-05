using Microsoft.AspNetCore.Mvc;
using SalesPoint.Datos;
using SalesPoint.Models;

namespace SalesPoint.Controllers {
	public class CategoriaController : Controller {

		private readonly AppDbContext _db;

		public CategoriaController(AppDbContext db) { 
			this._db = db;
		}

		public IActionResult Index() {

			IEnumerable<Categoria> cat_list = _db.Categoria;


			return View(cat_list);
		}

		public IActionResult Agregar() {
			return View();
		}

		public IActionResult Editar(int? Id) {
			if (Id == null || Id == 0) {
				return NotFound();
			}

			var selectedCat = _db.Categoria.Find(Id);

			if (selectedCat == null) {
				return NotFound();
			}

			return View(selectedCat);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Agregar(Categoria formCat) {

			if (ModelState.IsValid) {
				_db.Categoria.Add(formCat);
				_db.SaveChanges();
				return RedirectToAction(nameof(Index));
			}

			return View(formCat);
			
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Editar(Categoria editedCat) {
			if (ModelState.IsValid) {
				_db.Categoria.Update(editedCat);
				_db.SaveChanges();
				return RedirectToAction(nameof(Index));
			}

			return View(editedCat);
		}

		public IActionResult Eliminar(int? Id) {
			if (Id == null || Id == 0) {
				return NotFound();
			}

			var selectedCat = _db.Categoria.Find(Id);

			if (selectedCat == null) {
				return NotFound();
			}

			return View(selectedCat);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Eliminar(Categoria editedCat) {
			if (editedCat == null) {
				return NotFound();
			}

			_db.Categoria.Remove(editedCat);
			_db.SaveChanges();

			return RedirectToAction(nameof(Index));
		}

	}
}
