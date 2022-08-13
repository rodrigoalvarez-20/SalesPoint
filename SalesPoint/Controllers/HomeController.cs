using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesPoint.Datos;
using SalesPoint.Models;
using SalesPoint.Models.VewModels;
using SalesPoint.Utilidades;
using System.Diagnostics;

namespace SalesPoint.Controllers {
	public class HomeController : Controller {
		private readonly ILogger<HomeController> _logger;

		private readonly AppDbContext _db;

		public HomeController(ILogger<HomeController> logger, AppDbContext db) {
			_logger = logger;
			_db = db;
		}

		public IActionResult Index() {

			HomeVM homeVM = new() {
				Productos = _db.Producto.Include(c => c.Categoria).Include(t => t.TipoAplicacion),
				Categorias = _db.Categoria
			};

			return View(homeVM);
		}

		public IActionResult Detalle(int? Id) {
			
			if (Id == null || Id == 0) {
				return NotFound();
            }

			List<Cart> prodsInCart = new();
			var actualCart = HttpContext.Session.Get<IEnumerable<Cart>>(WebConstants.SessionCart);
			if (actualCart != null && actualCart.Count() > 0) {
				prodsInCart = HttpContext.Session.Get<IEnumerable<Cart>>(WebConstants.SessionCart).ToList();
			}

			DetalleVM detalleVM = new() {
				Producto = _db.Producto.Include(c => c.Categoria).Include(t => t.TipoAplicacion).Where(p => p.Id == Id).FirstOrDefault(),
				IsInCart = false
			};

			foreach (var p in prodsInCart) {
				if (p.ProductoId == Id) {
					detalleVM.IsInCart = true;
                }
            }

			return View(detalleVM);

        }

        [HttpPost, ActionName("Detalle")]
        public IActionResult Detalle(int Id) {
			List<Cart> prodsInCart = new List<Cart>();
			var actualCart = HttpContext.Session.Get<IEnumerable<Cart>>(WebConstants.SessionCart);
			if (actualCart != null && actualCart.Count() > 0) {
				prodsInCart = HttpContext.Session.Get<IEnumerable<Cart>>(WebConstants.SessionCart).ToList();
			}

			prodsInCart.Add(new() {
				ProductoId = Id
			});

			HttpContext.Session.Set(WebConstants.SessionCart, prodsInCart);

			return RedirectToAction(nameof(Index));

        }

		public IActionResult RemoverCarro(int Id) {
			List<Cart> prodsInCart = new List<Cart>();
			var actualCart = HttpContext.Session.Get<IEnumerable<Cart>>(WebConstants.SessionCart);
			if (actualCart != null && actualCart.Count() > 0) {
				prodsInCart = HttpContext.Session.Get<IEnumerable<Cart>>(WebConstants.SessionCart).ToList();
			}

			var prodToRemove = prodsInCart.SingleOrDefault(x => x.ProductoId == Id);
			if (prodToRemove != null) {
				prodsInCart.Remove(prodToRemove);
            }

			HttpContext.Session.Set(WebConstants.SessionCart, prodsInCart);

			return RedirectToAction(nameof(Index));

		}


		public IActionResult Privacy() {
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error() {
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}