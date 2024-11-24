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

        private DataTable CreateQueryTable(SqlCommand comandoParaConsulta)
        {
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            DataTable consultaFormatoTabla = new DataTable();
            _conexion.Open();
            adaptadorParaTabla.Fill(consultaFormatoTabla);
            _conexion.Close();
            return consultaFormatoTabla;
        }

        // Get the minimum and maximum prices of non-perishable and perishable products
        public (int minPrice, int maxPrice) ObtenerRangoDePreciosNoPerecederos()
        {
            string consulta = "SELECT MIN(Price) AS MinPrice, MAX(Price) AS MaxPrice FROM NonPerishableProduct WHERE Deleted = 0";
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
            string consulta = "SELECT MIN(Price) AS MinPrice, MAX(Price) AS MaxPrice FROM PerishableProduct WHERE Deleted = 0";
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



        // Get all non-perishable products
        public List<NonPerishableProductModel> GetNonPerishableProducts(string categoria = null, int precioMin = 0, int precioMax = 10000000, List<int> empresas = null)
        {
            List<NonPerishableProductModel> productos = new List<NonPerishableProductModel>();
            string consulta = "SELECT * FROM NonPerishableProduct WHERE Price BETWEEN @PrecioMin AND @PrecioMax AND Deleted = 0";

            SqlCommand comandoParaConsulta = new SqlCommand(consulta, _conexion);
            comandoParaConsulta.Parameters.AddWithValue("@PrecioMin", precioMin);
            comandoParaConsulta.Parameters.AddWithValue("@PrecioMax", precioMax);

            // Add category filter if defined

            if (!string.IsNullOrEmpty(categoria) && categoria != "Todas")
            {
                consulta += " AND Category = @Categoria";
                comandoParaConsulta.Parameters.AddWithValue("@Categoria", categoria);
            }

            // Add company filter if selected

            if (empresas != null && empresas.Count > 0)
            {
                consulta += " AND CompanyID IN (" + string.Join(",", empresas) + ")";
            }

            comandoParaConsulta.CommandText = consulta;  // Update the query with the filters

            
            DataTable tablaResultado = CreateQueryTable(comandoParaConsulta);

            foreach (DataRow columna in tablaResultado.Rows)
            {
                productos.Add(new NonPerishableProductModel
                {
                    ProductID = Convert.ToInt32(columna["ProductID"]),
                    ProductName = Convert.ToString(columna["ProductName"]),
                    CompanyID = Convert.ToInt32(columna["CompanyID"]),
                    CompanyName = Convert.ToString(columna["CompanyName"]),
                    ImageURL = Convert.ToString(columna["ImageURL"]),
                    Category = Convert.ToString(columna["Category"]),
                    Price = Convert.ToInt32(columna["Price"]),
                    ProductDescription = Convert.ToString(columna["ProductDescription"]),
                    Stock = Convert.ToInt32(columna["Stock"]),
                });
            }

            return productos;
        }


        // Similar for perishable products
        public List<PerishableProductModel> GetPerishableProducts(string categoria = null, int precioMin = 0, int precioMax = 89000, List<int> empresas = null)
        {
            List<PerishableProductModel> productos = new List<PerishableProductModel>();
            string consulta = "SELECT * FROM PerishableProduct WHERE Price BETWEEN @PrecioMin AND @PrecioMax AND Deleted = 0";

            SqlCommand comandoParaConsulta = new SqlCommand(consulta, _conexion);
            comandoParaConsulta.Parameters.AddWithValue("@PrecioMin", precioMin);
            comandoParaConsulta.Parameters.AddWithValue("@PrecioMax", precioMax);

            // Add category filter if defined
            if (!string.IsNullOrEmpty(categoria) && categoria != "Todas")
            {
                consulta += " AND Category = @Categoria";
                comandoParaConsulta.Parameters.AddWithValue("@Categoria", categoria);
            }

            // Add company filter if selected

            if (empresas != null && empresas.Count > 0)
            {
                consulta += " AND CompanyID IN (" + string.Join(",", empresas) + ")";
            }

            comandoParaConsulta.CommandText = consulta;  // Update the query with the filters


            DataTable tablaResultado = CreateQueryTable(comandoParaConsulta);

            foreach (DataRow columna in tablaResultado.Rows)
            {
                productos.Add(new PerishableProductModel
                {
                    ProductID = Convert.ToInt32(columna["ProductID"]),
                    ProductName = Convert.ToString(columna["ProductName"]),
                    CompanyID = Convert.ToInt32(columna["CompanyID"]),
                    ImageURL = Convert.ToString(columna["ImageURL"]),
                    Category = Convert.ToString(columna["Category"]),
                    CompanyName = Convert.ToString(columna["CompanyName"]),
                    Price = Convert.ToInt32(columna["Price"]),
                    ProductDescription = Convert.ToString(columna["ProductDescription"]),
                    DeliveryDays = Convert.ToString(columna["DeliveryDays"]),
                    ProductionLimit = Convert.ToInt32(columna["ProductionLimit"]),
                });
            }

            return productos;
        }

        // Get all unique company IDs from non-perishable products
        public List<CompaniesIDModel> ObtenerEmpresasNoPerecederos()
        {
            var empresas = new List<CompaniesIDModel>();

            string consulta = @"
            SELECT DISTINCT E.CompanyID, E.CompanyName 
            FROM Company E
            INNER JOIN (
                SELECT DISTINCT CompanyID FROM NonPerishableProduct WHERE Deleted = 0
            ) AS Companies ON E.CompanyID = Companies.CompanyID
            WHERE E.Deleted = 0";

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

        // Get all unique company IDs from perishable products

        public List<CompaniesIDModel> ObtenerEmpresasPerecederos()
        {
            var empresas = new List<CompaniesIDModel>();

            string consulta = @"
    SELECT DISTINCT E.CompanyID, E.CompanyName 
    FROM Company E
    INNER JOIN (
        SELECT DISTINCT CompanyID FROM PerishableProduct WHERE Deleted = 0
    ) AS Companies ON E.CompanyID = Companies.CompanyID
    WHERE E.Deleted = 0";

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

        // Get all unique company IDs from combined perishable and non-perishable products

        public List<CompaniesIDModel> ObtenerEmpresasUnicas()
        {
            var empresasNoPerecederos = ObtenerEmpresasNoPerecederos();
            var empresasPerecederos = ObtenerEmpresasPerecederos();

            // Merge both lists and remove duplicates
            var empresasUnicas = empresasNoPerecederos
                .Union(empresasPerecederos)
                .GroupBy(e => e.CompanyID)
                .Select(g => g.First())
                .ToList();

            return empresasUnicas;
        }

        public List<AddDeliveryModel> GetDeliveries(string id)
        {
            List<AddDeliveryModel> productos = new List<AddDeliveryModel>();
            string query = "SELECT * FROM Delivery WHERE ProductID = @id AND Deleted = 0";

            SqlCommand queryCommand = new SqlCommand(query, _conexion);
            queryCommand.Parameters.AddWithValue("@id", id);
    

            DataTable tableResult = CreateQueryTable(queryCommand);

            foreach (DataRow column in tableResult.Rows)
            {
                productos.Add(new AddDeliveryModel
                {
                    ProductID = Convert.ToInt32(column["ProductID"]),
                    BatchNumber = Convert.ToInt32(column["BatchNumber"]),
                    ExpirationDate = Convert.ToDateTime(column["ExpirationDate"]),
                    ReservedUnits = Convert.ToInt32(column["ReservedUnits"]),
                });

    }

            return productos;
        }






    }
}
