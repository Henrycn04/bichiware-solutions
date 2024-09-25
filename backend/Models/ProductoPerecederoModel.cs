using Microsoft.AspNetCore.Mvc;

namespace backend.Models
{
    public class ProductoPerecederoModel : Controller
    {
        public int IDProducto { get; set; }
        public string NombreProducto { get; set; }
        public int IDEmpresa { get; set; }
        public string ImagenURL { get; set; }
        public string Categoria { get; set; }
        public int Precio { get; set; }
        public string Descripcion { get; set; }
        public string DiasEntrega { get; set; }
        public int LimiteProduccion { get; set; }
    }
}
