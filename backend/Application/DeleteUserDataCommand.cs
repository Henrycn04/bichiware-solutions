using backend.Domain;
using backend.Handlers;
using backend.Infrastructure;
using System;

namespace backend.Application
{
    public class DeleteUserDataCommand
    {
        private readonly IUserDataHandler _userDataHandler;
        private readonly ICompanyProfileDataHandler _companyProfileDataHandler;
        private readonly IOrdersHandler _orderHandler;
        const string validIDException = "The ID has to be positive.";
        const string userNotFoundException = "User not found";
        const string companiesRelatedException = "User cannot be deletead because it is the only memebr of a compnany.";
        const string ordersInProgressRelatedException = "User cannot be deletead because it has orders in progress.";
        public DeleteUserDataCommand(
            IUserDataHandler userDataHandler, ICompanyProfileDataHandler companyProfileDataHandler,
            IOrdersHandler orderHandler)
        {
            _userDataHandler = userDataHandler;
            _companyProfileDataHandler = companyProfileDataHandler;
            _orderHandler = orderHandler;
        }

        public void DeleteUserData(int userId)
        {
            if (!IsValidUserId(userId))
            {
                throw new ArgumentException(validIDException);
            }

            var user = _userDataHandler.getUserData(userId);
            if (user == null)
            {
                throw new ArgumentException(userNotFoundException);
            }
     
            if (_companyProfileDataHandler.userHasRelatedCompanies(userId))
            {
                throw new InvalidOperationException(companiesRelatedException);
            }
            if (_orderHandler.userHasRelatedOrdersInProgress(userId))
            {
                throw new InvalidOperationException(ordersInProgressRelatedException);
            }

            if (_orderHandler.userHasRelatedOrders(userId))
            {
                _userDataHandler.logicDeleteUserData(userId);
            }
            else
            {
                _userDataHandler.deleteUserData(userId);
            }
        }

        private bool IsValidUserId(int userId)
        {
            return userId > 0;
        }
    }
}
