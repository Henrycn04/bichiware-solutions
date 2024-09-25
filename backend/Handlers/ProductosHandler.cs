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

        // Obtener el precio mínimo y máximo de los productos no perecederos y perecederos
        public (int minPrice, int maxPrice) ObtenerRangoDePreciosNoPerecederos()
        {
            string consulta = "SELECT MIN(Precio) AS MinPrice, MAX(Precio) AS MaxPrice FROM ProductoNoPerecedero";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, _conexion);

            _conexion.Open();
            SqlDataReader reader = comandoParaConsulta.ExecuteReader();

            int minPrice = 0, maxPrice = 0;

            if (reader.Read())
            {
                minPrice = Convert.ToInt32(reader["MinPrice"]);
                maxPrice = Convert.ToInt32(reader["MaxPrice"]);
            }

            _conexion.Close();

            return (minPrice, maxPrice);
        }

        public (int minPrice, int maxPrice) ObtenerRangoDePreciosPerecederos()
        {
            string consulta = "SELECT MIN(Precio) AS MinPrice, MAX(Precio) AS MaxPrice FROM ProductoPerecedero";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, _conexion);

            _conexion.Open();
            SqlDataReader reader = comandoParaConsulta.ExecuteReader();

            int minPrice = 0, maxPrice = 0;

            if (reader.Read())
            {
                minPrice = Convert.ToInt32(reader["MinPrice"]);
                maxPrice = Convert.ToInt32(reader["MaxPrice"]);
            }

            _conexion.Close();

            return (minPrice, maxPrice);
        }

        public (int minPrice, int maxPrice) ObtenerRangoDePrecios()
        {
            var (minNoPerecedero, maxNoPerecedero) = ObtenerRangoDePreciosNoPerecederos();
            var (minPerecedero, maxPerecedero) = ObtenerRangoDePreciosPerecederos();

            int minPrice = Math.Min(minNoPerecedero, minPerecedero);
            int maxPrice = Math.Max(maxNoPerecedero, maxPerecedero);

            return (minPrice, maxPrice);
        }



        // Obtener todos los productos no perecederos
        public List<ProductoNoPerecederoModel> ObtenerProductosNoPerecederos(string categoria = null, int precioMin = 0, int precioMax = 10000000, List<int> empresas = null)
        {
            List<ProductoNoPerecederoModel> productos = new List<ProductoNoPerecederoModel>();
            string consulta = "SELECT * FROM ProductoNoPerecedero WHERE Precio BETWEEN @PrecioMin AND @PrecioMax";

            SqlCommand comandoParaConsulta = new SqlCommand(consulta, _conexion);
            comandoParaConsulta.Parameters.AddWithValue("@PrecioMin", precioMin);
            comandoParaConsulta.Parameters.AddWithValue("@PrecioMax", precioMax);

            // Agrega filtro de categoría si está definido
            if (!string.IsNullOrEmpty(categoria) && categoria != "Todas")
            {
                consulta += " AND Categoria = @Categoria";
                comandoParaConsulta.Parameters.AddWithValue("@Categoria", categoria);
            }

            // Agrega filtro de empresas si están seleccionadas
            if (empresas != null && empresas.Count > 0)
            {
                consulta += " AND IDEmpresa IN (" + string.Join(",", empresas) + ")";
            }

            comandoParaConsulta.CommandText = consulta;  // Actualiza la consulta con los filtros
            

            //Console.WriteLine("Consulta SQL: " + comandoParaConsulta.CommandText);

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


        // Similar para productos perecederos
        public List<ProductoPerecederoModel> ObtenerProductosPerecederos(string categoria = null, int precioMin = 0, int precioMax = 89000, List<int> empresas = null)
        {
            List<ProductoPerecederoModel> productos = new List<ProductoPerecederoModel>();
            string consulta = "SELECT * FROM ProductoPerecedero WHERE Precio BETWEEN @PrecioMin AND @PrecioMax";

            SqlCommand comandoParaConsulta = new SqlCommand(consulta, _conexion);
            comandoParaConsulta.Parameters.AddWithValue("@PrecioMin", precioMin);
            comandoParaConsulta.Parameters.AddWithValue("@PrecioMax", precioMax);

            // Agrega filtro de categoría si está definido
            if (!string.IsNullOrEmpty(categoria) && categoria != "Todas")
            {
                consulta += " AND Categoria = @Categoria";
                comandoParaConsulta.Parameters.AddWithValue("@Categoria", categoria);
            }

            // Agrega filtro de empresas si están seleccionadas
            if (empresas != null && empresas.Count > 0)
            {
                consulta += " AND IDEmpresa IN (" + string.Join(",", empresas) + ")";
            }

            comandoParaConsulta.CommandText = consulta;  // Actualiza la consulta con los filtros

            //Console.WriteLine("Consulta SQL: " + comandoParaConsulta.CommandText);

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

        // Obtener todos los IDs de empresas únicos de productos no perecederos
        public List<EmpresasIDModel> ObtenerEmpresasNoPerecederos()
        {
            var empresas = new List<EmpresasIDModel>();

            string consulta = @"
    SELECT DISTINCT E.IDEmpresa, E.NombreEmpresa 
    FROM Empresa E
    INNER JOIN (
        SELECT DISTINCT IDEmpresa FROM ProductoNoPerecedero
    ) AS Empresas ON E.IDEmpresa = Empresas.IDEmpresa";

            using (SqlCommand comandoParaConsulta = new SqlCommand(consulta, _conexion))
            {
                _conexion.Open();
                using (SqlDataReader reader = comandoParaConsulta.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        empresas.Add(new EmpresasIDModel
                        {
                            IDEmpresa = Convert.ToInt32(reader["IDEmpresa"]),
                            NombreEmpresa = reader["NombreEmpresa"]?.ToString() ?? string.Empty
                        });
                    }
                }
                _conexion.Close();
            }

            return empresas;
        }

        // Obtener todos los IDs de empresas únicos de productos perecederos
        public List<EmpresasIDModel> ObtenerEmpresasPerecederos()
        {
            var empresas = new List<EmpresasIDModel>();

            string consulta = @"
    SELECT DISTINCT E.IDEmpresa, E.NombreEmpresa 
    FROM Empresa E
    INNER JOIN (
        SELECT DISTINCT IDEmpresa FROM ProductoPerecedero
    ) AS Empresas ON E.IDEmpresa = Empresas.IDEmpresa";

            using (SqlCommand comandoParaConsulta = new SqlCommand(consulta, _conexion))
            {
                _conexion.Open();
                using (SqlDataReader reader = comandoParaConsulta.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        empresas.Add(new EmpresasIDModel
                        {
                            IDEmpresa = Convert.ToInt32(reader["IDEmpresa"]),
                            NombreEmpresa = reader["NombreEmpresa"]?.ToString() ?? string.Empty
                        });
                    }
                }
                _conexion.Close();
            }

            return empresas;
        }

        // Obtener todos los IDs de empresas únicos de productos perecederos y no perecederos combinados
        public List<EmpresasIDModel> ObtenerEmpresasUnicas()
        {
            var empresasNoPerecederos = ObtenerEmpresasNoPerecederos();
            var empresasPerecederos = ObtenerEmpresasPerecederos();

            // Unir ambas listas y remover duplicados
            var empresasUnicas = empresasNoPerecederos
                .Union(empresasPerecederos)
                .GroupBy(e => e.IDEmpresa)
                .Select(g => g.First())
                .ToList();

            return empresasUnicas;
        }







    }
}
