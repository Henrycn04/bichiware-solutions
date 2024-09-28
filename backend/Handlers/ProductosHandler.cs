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
            _rutaConexion = builder.Configuration.GetConnectionString("CompanyDataContext");
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
            string consulta = "SELECT MIN(Price) AS MinPrice, MAX(Price) AS MaxPrice FROM NonPerishableProduct";
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
            string consulta = "SELECT MIN(Price) AS MinPrice, MAX(Price) AS MaxPrice FROM PerishableProduct";
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
        public List<NonPerishableProductModel> ObtenerProductosNoPerecederos(string categoria = null, int precioMin = 0, int precioMax = 10000000, List<int> empresas = null)
        {
            List<NonPerishableProductModel> productos = new List<NonPerishableProductModel>();
            string consulta = "SELECT * FROM NonPerishableProduct WHERE Price BETWEEN @PrecioMin AND @PrecioMax";

            SqlCommand comandoParaConsulta = new SqlCommand(consulta, _conexion);
            comandoParaConsulta.Parameters.AddWithValue("@PrecioMin", precioMin);
            comandoParaConsulta.Parameters.AddWithValue("@PrecioMax", precioMax);

            // Agrega filtro de categoría si está definido
            if (!string.IsNullOrEmpty(categoria) && categoria != "Todas")
            {
                consulta += " AND Category = @Categoria";
                comandoParaConsulta.Parameters.AddWithValue("@Categoria", categoria);
            }

            // Agrega filtro de empresas si están seleccionadas
            if (empresas != null && empresas.Count > 0)
            {
                consulta += " AND CompanyID IN (" + string.Join(",", empresas) + ")";
            }

            comandoParaConsulta.CommandText = consulta;  // Actualiza la consulta con los filtros
            

            //Console.WriteLine("Consulta SQL: " + comandoParaConsulta.CommandText);

            DataTable tablaResultado = CrearTablaConsulta(comandoParaConsulta);

            foreach (DataRow columna in tablaResultado.Rows)
            {
                productos.Add(new NonPerishableProductModel
                {
                    ProductID = Convert.ToInt32(columna["ProductID"]),
                    ProductName = Convert.ToString(columna["ProductName"]),
                    CompanyID = Convert.ToInt32(columna["CompanyID"]),
                    ImageURL = Convert.ToString(columna["ImageURL"]),
                    Category = Convert.ToString(columna["Category"]),
                    Price = Convert.ToInt32(columna["Price"]),
                    ProductDescription = Convert.ToString(columna["ProductDescription"]),
                    Stock = Convert.ToInt32(columna["Stock"]),
                });
            }

            return productos;
        }


        // Similar para productos perecederos
        public List<PerishableProductModel> ObtenerProductosPerecederos(string categoria = null, int precioMin = 0, int precioMax = 89000, List<int> empresas = null)
        {
            List<PerishableProductModel> productos = new List<PerishableProductModel>();
            string consulta = "SELECT * FROM PerishableProduct WHERE Price BETWEEN @PrecioMin AND @PrecioMax";

            SqlCommand comandoParaConsulta = new SqlCommand(consulta, _conexion);
            comandoParaConsulta.Parameters.AddWithValue("@PrecioMin", precioMin);
            comandoParaConsulta.Parameters.AddWithValue("@PrecioMax", precioMax);

            // Agrega filtro de categoría si está definido
            if (!string.IsNullOrEmpty(categoria) && categoria != "Todas")
            {
                consulta += " AND Category = @Categoria";
                comandoParaConsulta.Parameters.AddWithValue("@Categoria", categoria);
            }

            // Agrega filtro de empresas si están seleccionadas
            if (empresas != null && empresas.Count > 0)
            {
                consulta += " AND CompanyID IN (" + string.Join(",", empresas) + ")";
            }

            comandoParaConsulta.CommandText = consulta;  // Actualiza la consulta con los filtros

            //Console.WriteLine("Consulta SQL: " + comandoParaConsulta.CommandText);

            DataTable tablaResultado = CrearTablaConsulta(comandoParaConsulta);

            foreach (DataRow columna in tablaResultado.Rows)
            {
                productos.Add(new PerishableProductModel
                {
                    ProductID = Convert.ToInt32(columna["ProductID"]),
                    ProductName = Convert.ToString(columna["ProductName"]),
                    CompanyID = Convert.ToInt32(columna["CompanyID"]),
                    ImageURL = Convert.ToString(columna["ImageURL"]),
                    Category = Convert.ToString(columna["Category"]),
                    Price = Convert.ToInt32(columna["Price"]),
                    ProductDescription = Convert.ToString(columna["ProductDescription"]),
                    DeliveryDays = Convert.ToString(columna["DeliveryDays"]),
                    ProductionLimit = Convert.ToInt32(columna["ProductionLimit"]),
                });
            }

            return productos;
        }

        // Obtener todos los IDs de empresas únicos de productos no perecederos
        public List<CompaniesIDModel> ObtenerEmpresasNoPerecederos()
        {
            var empresas = new List<CompaniesIDModel>();

            string consulta = @"
    SELECT DISTINCT E.CompanyID, E.CompanyName 
    FROM Company E
    INNER JOIN (
        SELECT DISTINCT CompanyID FROM NonPerishableProduct
    ) AS Companies ON E.CompanyID = Companies.CompanyID";

            using (SqlCommand comandoParaConsulta = new SqlCommand(consulta, _conexion))
            {
                _conexion.Open();
                using (SqlDataReader reader = comandoParaConsulta.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        empresas.Add(new CompaniesIDModel
                        {
                            CompanyID = Convert.ToInt32(reader["CompanyID"]),
                            CompanyName = reader["CompanyName"]?.ToString() ?? string.Empty
                        });
                    }
                }
                _conexion.Close();
            }

            return empresas;
        }

        // Obtener todos los IDs de empresas únicos de productos perecederos
        public List<CompaniesIDModel> ObtenerEmpresasPerecederos()
        {
            var empresas = new List<CompaniesIDModel>();

            string consulta = @"
    SELECT DISTINCT E.CompanyID, E.CompanyName 
    FROM Company E
    INNER JOIN (
        SELECT DISTINCT CompanyID FROM PerishableProduct
    ) AS Companies ON E.CompanyID = Companies.CompanyID";

            using (SqlCommand comandoParaConsulta = new SqlCommand(consulta, _conexion))
            {
                _conexion.Open();
                using (SqlDataReader reader = comandoParaConsulta.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        empresas.Add(new CompaniesIDModel
                        {
                            CompanyID = Convert.ToInt32(reader["CompanyID"]),
                            CompanyName = reader["CompanyName"]?.ToString() ?? string.Empty
                        });
                    }
                }
                _conexion.Close();
            }

            return empresas;
        }

        // Obtener todos los IDs de empresas únicos de productos perecederos y no perecederos combinados
        public List<CompaniesIDModel> ObtenerEmpresasUnicas()
        {
            var empresasNoPerecederos = ObtenerEmpresasNoPerecederos();
            var empresasPerecederos = ObtenerEmpresasPerecederos();

            // Unir ambas listas y remover duplicados
            var empresasUnicas = empresasNoPerecederos
                .Union(empresasPerecederos)
                .GroupBy(e => e.CompanyID)
                .Select(g => g.First())
                .ToList();

            return empresasUnicas;
        }







    }
}
