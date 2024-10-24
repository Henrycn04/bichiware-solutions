﻿using backend.Models;
using MailKit.Search;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;

namespace backend.Handlers
{
    public class OrdersHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;

        public OrdersHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("BichiwareSolutionsContext");
            _connection = new SqlConnection(_routeConnection);
        }

        public int getAmountOfOrders()
        {
            string query = "SELECT COUNT(*) AS AmountOfOrders FROM Orders";
            SqlCommand commandForQuery = new SqlCommand(query, _connection);
            _connection.Open();
            int amount = (int)commandForQuery.ExecuteScalar();
            _connection.Close();
            return amount;
        }

        public List<OrdersModel> getOrdersData()
        {
            List<OrdersModel> orders = new List<OrdersModel>();
            List<OrdersModel> auxiliarOrders = new List<OrdersModel>();
            string query = @"
                SELECT o.OrderID, pr.ProfileName, CONCAT(ad.Province, ', ', ad.Canton, ', ', ad.District, ', ', ad.ExactAddress) AS OrderAddress, (o.Tax + o.ShippingCost + o.ProductCost) AS TotalAmount
                FROM Orders o
                INNER JOIN Profile pr ON o.UserID = pr.UserID
                INNER JOIN Address ad ON o.AddressID = ad.AddressID
                WHERE OrderStatus = 1
                ";
            SqlCommand commandForQuery = new SqlCommand(query, _connection);
            _connection.Open();
            using (SqlDataReader reader = commandForQuery.ExecuteReader())
            {
                while (reader.Read())
                {
                    var order = new OrdersModel
                    {
                        OrderID = reader.GetInt32("OrderID"),
                        ClientName = reader["ProfileName"].ToString(),
                        OrderAddress = reader["OrderAddress"].ToString(),
                        TotalAmount = reader.GetDecimal("TotalAmount")
                    };
                    auxiliarOrders.Add(order);
                }
            }
            _connection.Close();
            for (int i = 0; i < auxiliarOrders.Count(); i++)
            {
                auxiliarOrders[i] = getPerishableProducts(auxiliarOrders[i]);
                auxiliarOrders[i] = getNonPerishableProducts(auxiliarOrders[i]);
                orders.Add(auxiliarOrders[i]);
            }
            return orders;
        }

        public OrdersModel getPerishableProducts(OrdersModel order)
        {
            order.OrderProducts = new List<OrdersModel.OrderProductsModel>();
            string queryForPerishableProducts = @"
                SELECT op.ProductName, op.Quantity 
                FROM OrderedPerishable op
                INNER JOIN Orders o ON op.OrderID = o.OrderID
                WHERE o.OrderStatus = 1 AND o.OrderID = @OrderID
            ";
            SqlCommand commandForQuery = new SqlCommand(queryForPerishableProducts, _connection);
            commandForQuery.Parameters.AddWithValue("@OrderID", order.OrderID);
            _connection.Open();
            using (SqlDataReader reader = commandForQuery.ExecuteReader())
            {
                while (reader.Read())
                {
                    var orderPerishableProducts = new OrdersModel.OrderProductsModel
                    {
                        ProductName = reader["ProductName"].ToString(),
                        Quantity = reader.GetInt32("Quantity")
                    };
                    order.OrderProducts.Add(orderPerishableProducts);
                }
            }
            _connection.Close();
            return order;
        }

        public OrdersModel getNonPerishableProducts(OrdersModel order)
        {
            string queryForNonPerishableProducts = @"
                SELECT op.ProductName, op.Quantity 
                FROM OrderedNonPerishable op
                INNER JOIN Orders o ON op.OrderID = o.OrderID
                WHERE o.OrderStatus = 1 AND o.OrderID = @OrderID
            ";
            SqlCommand commandForQuery = new SqlCommand(queryForNonPerishableProducts, _connection);
            commandForQuery.Parameters.AddWithValue("@OrderID", order.OrderID);
            _connection.Open();
            using (SqlDataReader reader = commandForQuery.ExecuteReader())
            {
                while (reader.Read())
                {
                    var orderNonPerishableProducts = new OrdersModel.OrderProductsModel
                    {
                        ProductName = reader["ProductName"].ToString(),
                        Quantity = reader.GetInt32("Quantity")
                    };
                    order.OrderProducts.Add(orderNonPerishableProducts);
                }
            }
            _connection.Close();
            return order;
        }
    }
}
