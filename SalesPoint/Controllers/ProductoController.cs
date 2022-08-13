using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalesPoint.Datos;
using SalesPoint.Models;
using SalesPoint.Models.VewModels;
using System.IO;

namespace SalesPoint.Controllers {
	public class ProductoController : Controller {

		private readonly AppDbContext _db;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public ProductoController(AppDbContext db, IWebHostEnvironment _wh) {
			_db = db;
			_webHostEnvironment = _wh;
		}

		public IActionResult Index() {
			IEnumerable<Producto> prod_list = _db.Producto
				.Include(c => c.Categoria)
				.Include(t => t.TipoAplicacion);
			return View(prod_list);
		}

		//Get/Post para la pagina de Crear/Actualizar

		public IActionResult Upsert(int? Id) {

			var selectors = loadSelectors();

			ProductoVM prodVM = new() {
				Producto = new Producto(),
				CategoriaLista = selectors[0],
				TipoAplicacionLista = selectors[1]
			};

			if (Id == null) {
				// Creando nuevo registro
				return View(prodVM);
			} else {
				prodVM.Producto = _db.Producto.Find(Id);
				if (prodVM.Producto == null) {
					return NotFound();
				}

				return View(prodVM);

			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Upsert(ProductoVM prodVm) {
			if (ModelState.IsValid) {
				var files = HttpContext.Request.Form.Files;
				string rootPath = _webHostEnvironment.WebRootPath;
				if (prodVm.Producto.Id == 0) {
					// Crear producto
					prodVm.Producto.UrlImagen = CreateProductImage(rootPath, files[0]);
					_db.Producto.Add(prodVm.Producto);
				} else {
					// Actualizar
					var prodInDB = _db.Producto.AsNoTracking().FirstOrDefault(p => p.Id == prodVm.Producto.Id);

					if(files.Count > 0 && prodInDB != null) {
						var uploadPath = rootPath + WebConstants.ProductImagesRoute;
						if (prodInDB.UrlImagen != null) {
							// Eliminar la imagen anterior
							var prodFile = Path.Combine(uploadPath, prodInDB.UrlImagen);
							if (System.IO.File.Exists(prodFile)) {
								System.IO.File.Delete(prodFile);
							}
						}
						prodVm.Producto.UrlImagen = CreateProductImage(rootPath, files[0]);
					}else {
						prodVm.Producto.UrlImagen = prodInDB?.UrlImagen;
					}
					_db.Producto.Update(prodVm.Producto);
				}

				_db.SaveChanges();
				return RedirectToAction(nameof(Index));
			}

			var selectors = loadSelectors();

			prodVm.CategoriaLista = selectors[0];
			prodVm.TipoAplicacionLista = selectors[1];

			return View(prodVm);
			
		}


		public IActionResult Eliminar(int? Id) {
			if (Id == null || Id == 0) {
				return NotFound();
			}

			Producto prod = _db.Producto.Include(c => c.Categoria)
				.Include(t => t.TipoAplicacion)
				.FirstOrDefault(p => p.Id == Id);

			if (prod == null) {
				return NotFound();
			}

			return View(prod);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Eliminar(Producto prod) {
			if (prod == null) {
				return NotFound();
			}

			// Eliminar la imagen
			// Eliminar la imagen, si es que existe
			string rootPath = _webHostEnvironment.WebRootPath;
			var uploadPath = rootPath + WebConstants.ProductImagesRoute;
			if (prod.UrlImagen != null) {
				// Eliminar la imagen anterior
				var prodFile = Path.Combine(uploadPath, prod.UrlImagen);
				if (System.IO.File.Exists(prodFile)) {
					System.IO.File.Delete(prodFile);
				}
			}

			_db.Producto.Remove(prod);
			_db.SaveChanges();

			return RedirectToAction(nameof(Index));

		}

		private static string CreateProductImage(string rootPath, IFormFile file) {
			var uploadPath = rootPath + WebConstants.ProductImagesRoute;
			string fileName = Guid.NewGuid().ToString();
			string extension = Path.GetExtension(file.FileName);
			using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName + extension), FileMode.Create)) {
				file.CopyTo(fileStream);
			}

			return fileName + extension;

		}

		private List<IEnumerable<SelectListItem>> loadSelectors() {

			List<IEnumerable<SelectListItem>> selectors = new();

			IEnumerable<SelectListItem> dwCategoria = _db.Categoria.Select(c => new SelectListItem {
				Text = c.NombreCategoria,
				Value = c.Id.ToString()
			});

			IEnumerable<SelectListItem> dwTipoApp = _db.TipoAplicacion.Select(c => new SelectListItem {
				Text = c.NombreAplicacion,
				Value = c.Id.ToString()
			});

			selectors.Add(dwCategoria);
			selectors.Add(dwTipoApp);

			return selectors;

		}


	}
}
