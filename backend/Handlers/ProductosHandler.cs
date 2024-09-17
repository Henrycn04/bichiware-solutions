using backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace backend.Handlers
{
    public class ProductosHandler
    {
        private SqlConnection _conexion;
        private string _rutaConexion;

        public ProductosHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _rutaConexion = builder.Configuration.GetConnectionString("ProductosContext");
            _conexion = new SqlConnection(_rutaConexion);
        }

        private DataTable CrearTablaConsulta(SqlCommand comandoParaConsulta)
        {
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            DataTable consultaFormatoTabla = new DataTable();
            _conexion.Open();
            adaptadorParaTabla.Fill(consultaFormatoTabla);
            _conexion.Close();
            return consultaFormatoTabla;
        }

        // Obtener todos los productos no perecederos
        public List<ProductoNoPerecederoModel> ObtenerProductosNoPerecederos(string categoria = null)
        {
            List<ProductoNoPerecederoModel> productos = new List<ProductoNoPerecederoModel>();
            string consulta = "SELECT * FROM ProductoNoPerecedero";

            SqlCommand comandoParaConsulta = new SqlCommand(consulta, _conexion);

            // Si la categoría no está vacía y no es "Todas", se aplica el filtro
            if (!string.IsNullOrEmpty(categoria) && categoria != "Todas")
            {
                consulta += " WHERE Categoria = @Categoria";
                comandoParaConsulta.CommandText = consulta;  // Actualiza la consulta con WHERE
                comandoParaConsulta.Parameters.AddWithValue("@Categoria", categoria);
            }

            DataTable tablaResultado = CrearTablaConsulta(comandoParaConsulta);

            foreach (DataRow columna in tablaResultado.Rows)
            {
                productos.Add(new ProductoNoPerecederoModel
                {
                    IDProducto = Convert.ToInt32(columna["IDProducto"]),
                    NombreProducto = Convert.ToString(columna["NombreProducto"]),
                    IDEmpresa = Convert.ToInt32(columna["IDEmpresa"]),
                    ImagenURL = Convert.ToString(columna["ImagenURL"]),
                    Categoria = Convert.ToString(columna["Categoria"]),
                    Precio = Convert.ToInt32(columna["Precio"]),
                    Descripcion = Convert.ToString(columna["Descripcion"]),
                    Existencias = Convert.ToInt32(columna["Existencias"]),
                });
            }
            return productos;
        }



        // Obtener todos los productos perecederos
        public List<ProductoPerecederoModel> ObtenerProductosPerecederos(string categoria = null)
        {
            List<ProductoPerecederoModel> productos = new List<ProductoPerecederoModel>();
            string consulta = "SELECT * FROM ProductoPerecedero";

            SqlCommand comandoParaConsulta = new SqlCommand(consulta, _conexion);

            if (!string.IsNullOrEmpty(categoria) && categoria != "Todas")
            {
                consulta += " WHERE Categoria = @Categoria";
                comandoParaConsulta.CommandText = consulta;  // Actualiza la consulta con WHERE
                comandoParaConsulta.Parameters.AddWithValue("@Categoria", categoria);
            }

            DataTable tablaResultado = CrearTablaConsulta(comandoParaConsulta);

            foreach (DataRow columna in tablaResultado.Rows)
            {
                productos.Add(new ProductoPerecederoModel
                {
                    IDProducto = Convert.ToInt32(columna["IDProducto"]),
                    NombreProducto = Convert.ToString(columna["NombreProducto"]),
                    IDEmpresa = Convert.ToInt32(columna["IDEmpresa"]),
                    ImagenURL = Convert.ToString(columna["ImagenURL"]),
                    Categoria = Convert.ToString(columna["Categoria"]),
                    Precio = Convert.ToInt32(columna["Precio"]),
                    Descripcion = Convert.ToString(columna["Descripcion"]),
                    DiasEntrega = Convert.ToString(columna["DiasEntrega"]),
                    LimiteProduccion = Convert.ToInt32(columna["LimiteProduccion"]),
                });
            }

            return productos;
        }

    }
}
