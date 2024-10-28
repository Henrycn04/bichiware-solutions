using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Domain;
using backend.Infrastructure;
using backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace backend.Commands
{
    public class AddOrderCommand
    {
        private readonly AddOrderHandler _handler;


        public AddOrderCommand(AddOrderHandler handler)
        {
            _handler = handler;
        }
        public async Task<int> CreateOrder(AddOrderModel order)
        {
            int orderId = await _handler.InsertOrder(order);
            if (orderId == 0)
            {
                Console.WriteLine("Error: no se pudo insertar la orden.");
            }
            return orderId;
        }

        public async Task<bool> AddPerishableProducts(List<AddPerishableProductToOrderModel> perishableProducts)
        {
            foreach (var product in perishableProducts)
            {
                bool hasStock = await _handler.HasSufficientPerishableStock(product);
                if (!hasStock)
                {
                    Console.WriteLine($"Error: no hay suficiente stock para el producto perecedero {product.ProductID}.");
                    return false;
                }

                bool inserted = await _handler.InsertPerishableProduct(product);
                if (!inserted)
                {
                    Console.WriteLine($"Error: no se pudo insertar el producto perecedero {product.ProductID}.");
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> AddNonPerishableProducts(List<AddNonPerishableProductToOrderModel> nonPerishableProducts)
        {
            foreach (var product in nonPerishableProducts)
            {
                bool hasStock = await _handler.HasSufficientNonPerishableStock(product);
                if (!hasStock)
                {
                    Console.WriteLine($"Error: no hay suficiente stock para el producto no perecedero {product.ProductID}.");
                    return false;
                }

                bool inserted = await _handler.InsertNonPerishableProduct(product);
                if (!inserted)
                {
                    Console.WriteLine($"Error: no se pudo insertar el producto no perecedero {product.ProductID}.");
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> SendRealizationEmails(OrderEmailModel order)
        {
            try
            {
                return await this._handler.SendRealizationEmails(order);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}