namespace SalesPoint.Models.VewModels {
    public class DetalleVM {
        public Producto Producto { get; set; }
        public bool IsInCart { get; set; }

        public DetalleVM() {
            Producto = new Producto();
        }

    }
}
