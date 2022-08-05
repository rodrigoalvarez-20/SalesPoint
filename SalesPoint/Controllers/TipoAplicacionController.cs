using Microsoft.AspNetCore.Mvc;
using SalesPoint.Datos;
using SalesPoint.Models;

namespace SalesPoint.Controllers {
	public class TipoAplicacionController : Controller {

		private readonly AppDbContext _db;

		public TipoAplicacionController(AppDbContext db) {
			this._db = db;
		}

		public IActionResult Index() {

			IEnumerable<TipoAplicacion> app_list = _db.TipoAplicacion;


			return View(app_list);
		}

		public IActionResult Agregar() {
			return View();
		}

		public IActionResult Editar(int? Id) {
			if (Id == null || Id == 0) {
				return NotFound();
			}

			var selectedAppType = _db.TipoAplicacion.Find(Id);

			if (selectedAppType == null) {
				return NotFound();
			}

			return View(selectedAppType);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Agregar(TipoAplicacion formAppType) {

			if (ModelState.IsValid) {
				_db.TipoAplicacion.Add(formAppType);
				_db.SaveChanges();
				return RedirectToAction(nameof(Index));
			}

			return View(formAppType);

		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Editar(TipoAplicacion editedAppType) {
			if (ModelState.IsValid) {
				_db.TipoAplicacion.Update(editedAppType);
				_db.SaveChanges();
				return RedirectToAction(nameof(Index));
			}

			return View(editedAppType);
		}

		public IActionResult Eliminar(int? Id) {
			if (Id == null || Id == 0) {
				return NotFound();
			}

			var selectedAppType = _db.TipoAplicacion.Find(Id);

			if (selectedAppType == null) {
				return NotFound();
			}

			return View(selectedAppType);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Eliminar(TipoAplicacion selectedAppType) {
			if (selectedAppType == null) {
				return NotFound();
			}

			_db.TipoAplicacion.Remove(selectedAppType);
			_db.SaveChanges();

			return RedirectToAction(nameof(Index));
		}
	}
}
