using Microsoft.AspNetCore.Identity;

namespace SalesPoint.Models {
	public class UsuarioAplicacion : IdentityUser {

		public string NombreCompleto { get; set; }


	}
}
