﻿using System;
using System.Data;
using System.Data.SqlClient;
using backend.Domain;
using backend.Models;



namespace backend.Infrastructure
{
    public interface IUpdateProductHandler
    {
        void UpdatePerishableProduct(UpdatePerishablProductModel product);
        void UpdateNonPerishableProduct(UpdateNonPerishableProductModel product);
        void LogicPerishableProductDelete(int productId);
        void PerishableProductDelete(int productId);
        void LogicNonPerishableProductDelete(int productId);
        void NonPerishableProductDelete(int productId);
    }
    public class UpdateProductHandler : IUpdateProductHandler
    {
        private readonly SqlConnection _connection;
        private string _ConectionString;

        public UpdateProductHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _ConectionString = builder.Configuration.GetConnectionString("BichiwareSolutionsContext");
            _connection = new SqlConnection(_ConectionString);
        }


        public void UpdatePerishableProduct(UpdatePerishablProductModel product)
        {
            using (var command = new SqlCommand("UpdatePerishableProductData", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Agregamos los parámetros necesarios
                command.Parameters.AddWithValue("@ProductID", product.ProductID);
                command.Parameters.AddWithValue("@ProductName", product.Name);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@ImageURL", product.Image);
                command.Parameters.AddWithValue("@Weight", product.Weight);
                command.Parameters.AddWithValue("@ProductDescription", product.Description);
                command.Parameters.AddWithValue("@DeliveryDays", product.DeliveryDays);
                command.Parameters.AddWithValue("@ProductionLimit", product.Limit);


                try
                {
                    _connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new InvalidOperationException("No perishable product was updated. Please check the provided Product ID.");
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("An error occurred while updating the perishable product.", ex);
                }
                finally
                {
                    _connection.Close();
                }
            }
        }

        public void UpdateNonPerishableProduct(UpdateNonPerishableProductModel product)
        {
            using (var command = new SqlCommand("UpdateNonPerishableProductData", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Agregamos los parámetros necesarios
                command.Parameters.AddWithValue("@ProductID", product.ProductID);
                command.Parameters.AddWithValue("@ProductName", product.Name);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@ImageURL", product.Image);
                command.Parameters.AddWithValue("@Weight", product.Weight);
                command.Parameters.AddWithValue("@ProductDescription", product.Description);
                command.Parameters.AddWithValue("@Stock", product.Stock);

                try
                {
                    _connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new InvalidOperationException("No non-perishable product was updated. Please check the provided Product ID.");
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("An error occurred while updating the non-perishable product.", ex);
                }
                finally
                {
                    _connection.Close();
                }
            }
        }
        public void LogicPerishableProductDelete(int productId)
        {
            using (var command = new SqlCommand("logicPerishableProductDelete", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ProductID", productId);

                try
                {
                    _connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new InvalidOperationException("No product was logically deleted. Please check the provided Product ID.");
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("An error occurred while performing a logical delete on the product.", ex);
                }
                finally
                {
                    _connection.Close();
                }
            }
        }
        public void PerishableProductDelete(int productId)
        {
            using (var command = new SqlCommand("PerishableProductDelete", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ProductID", productId);

                try
                {
                    _connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new InvalidOperationException("No product was deleted. Please check the provided Product ID.");
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("An error occurred while deleting the product.", ex);
                }
                finally
                {
                    _connection.Close();
                }
            }
        }
        public void LogicNonPerishableProductDelete(int productId)
        {
            using (var command = new SqlCommand("logicNonPerishableProductDelete", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ProductID", productId);

                try
                {
                    _connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new InvalidOperationException("No product was logically deleted. Please check the provided Product ID.");
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("An error occurred while performing a logical delete on the product.", ex);
                }
                finally
                {
                    _connection.Close();
                }
            }
        }
        public void NonPerishableProductDelete(int productId)
        {
            using (var command = new SqlCommand("nonPerishableProductDelete", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ProductID", productId);

                try
                {
                    _connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new InvalidOperationException("No product was deleted. Please check the provided Product ID.");
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("An error occurred while deleting the product.", ex);
                }
                finally
                {
                    _connection.Close();
                }
            }
        }
    }
}



