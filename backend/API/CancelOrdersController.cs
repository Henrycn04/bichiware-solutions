using backend.Domain;
using backend.Commands;
using Microsoft.AspNetCore.Mvc;
using backend.Handlers;

namespace backend.API
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CancelOrdersController : ControllerBase
    {
        private readonly CancelOrdersCommand _cancelOrdersCommand;

        public CancelOrdersController(CancelOrdersCommand cancelOrdersCommand)
        {
            this._cancelOrdersCommand = cancelOrdersCommand;
        }

        [HttpPost]
        public string CancelOrderByUser(ConfirmOrderModel order)
        {
            int orderID = order.OrderID;
            int rowsAffected = this._cancelOrdersCommand.CancelOrderByUser(orderID);
            if (rowsAffected > 0)
            {
                return "Orden cancelada exitosamente";
            }
            else if (rowsAffected == -1)
            {
                return $"Error cancelando orden con ID = {orderID}. La órden ya fue confirmada por un administrador." ;
            }
            else
            {
                return $"Error cancelando orden con ID = {orderID}. La órden no existe o el ID recibido es inválido";
            }
        }
   
        [HttpPost]
        public string CancelOrderByEntrepreneur(ConfirmOrderModel order)
        {
            int orderID = order.OrderID;
            int rowsAffected = this._cancelOrdersCommand.CancelOrderByEntrepreneur(orderID);
            if (rowsAffected > 0)
            {
                return "Orden cancelada exitosamente";
            }
            else if (rowsAffected == -1)
            {
                return $"Error ccancelando orden con ID = {orderID}. La órden ya está en envío.";
            }
            else
            {
                return $"Error cancelando orden con ID = {orderID}. La órden no existe o el ID recibido es inválido.";
            }
        }
    }
}
