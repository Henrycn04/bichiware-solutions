using backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace backend.Handlers
{
    public class CompanyProductsHandler
    {
        private SqlConnection _conexion;
        private string _rutaConexion;

        public CompanyProductsHandler()
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

        // Get all non-perishable products from a company
        public List<NonPerishableProductModel> ObtenerProductosNoPerecederos(int empresa)
        {
            List<NonPerishableProductModel> productos = new List<NonPerishableProductModel>();
            string consulta = "SELECT * FROM NonPerishableProduct WHERE CompanyID = @Empresa";

            SqlCommand comandoParaConsulta = new SqlCommand(consulta, _conexion);
            comandoParaConsulta.Parameters.AddWithValue("@Empresa", empresa);

            comandoParaConsulta.CommandText = consulta;  
        

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


        // Similar for perishable products
        public List<PerishableProductModel> ObtenerProductosPerecederos(int empresa)
        {
            List<PerishableProductModel> productos = new List<PerishableProductModel>();
            string consulta = "SELECT * FROM PerishableProduct WHERE CompanyID = @Empresa";


            SqlCommand comandoParaConsulta = new SqlCommand(consulta, _conexion);
            comandoParaConsulta.Parameters.AddWithValue("@Empresa", empresa);

            comandoParaConsulta.CommandText = consulta;  

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


    }
}
