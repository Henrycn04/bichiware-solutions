using backend.Domain;
using backend.Handlers;
using backend.Queries;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalProfitsTest
{
   public class TotalProfitsQueryTest
    {
        private readonly Mock<ITotalProfitsHandler> _mockHandler;
        private readonly TotalProfitsQuery _query;
        const string nullRequest = "The request cant be null";
        const string emptyYearList = "The list of years cant be empty";
        const string emptyIDList = "The list of company ID cant be empty";
        const string validYear = "Year must be positive";
        const string validID = "ID must be positive";

    public TotalProfitsQueryTest()
    {
        _mockHandler = new Mock<ITotalProfitsHandler>();
        _query = new TotalProfitsQuery(_mockHandler.Object);
    }

    [Test]
    public void GetTotalProfits_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        // Arrange
        TotalProftsRequestModel request = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _query.GetTotalProfits(request));
        Assert.AreEqual(nullRequest, exception.Message);
    }

    [Test]
    public void GetTotalProfits_ShouldThrowArgumentException_WhenYearsListIsNullOrEmpty()
    {
        // Arrange
        var request = new TotalProftsRequestModel
        {
            Years = new List<int>(), // Empty list
            CompanyIDs = new List<int> { 1 }
        };

        // Act & Assert
        var exception =  Assert.Throws<ArgumentException>(() => _query.GetTotalProfits(request));
        Assert.AreEqual(emptyYearList , exception.Message);
    }

    [Test]
    public void GetTotalProfits_ShouldThrowArgumentException_WhenCompanyIDsListIsNullOrEmpty()
    {
        // Arrange
        var request = new TotalProftsRequestModel
        {
            Years = new List<int> { 2020 },
            CompanyIDs = new List<int>() // Empty list
        };

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _query.GetTotalProfits(request));
        Assert.AreEqual(emptyIDList, exception.Message);
    }

    [Test]
    public void GetTotalProfits_ShouldThrowArgumentException_WhenYearsContainNegativeOrZero()
    {
        // Arrange
        var request = new TotalProftsRequestModel
        {
            Years = new List<int> { 2020, -1 },
            CompanyIDs = new List<int> { 1 }
        };

        // Act & Assert
        var exception =  Assert.Throws<ArgumentException>(() => _query.GetTotalProfits(request));
        Assert.AreEqual(validYear, exception.Message);
    }

    [Test]
    public async Task GetTotalProfits_ShouldThrowArgumentException_WhenCompanyIDsContainNegativeOrZero()
    {
        // Arrange
        var request = new TotalProftsRequestModel
        {
            Years = new List<int> { 2020 },
            CompanyIDs = new List<int> { -5 } // Invalid ID
        };

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _query.GetTotalProfits(request));
        Assert.AreEqual(validID, exception.Message);
    }

    [Test]
    public async Task GetTotalProfits_ShouldCallHandler_WhenRequestIsValid()
    {
        // Arrange
        var request = new TotalProftsRequestModel
        {
            Years = new List<int> { 2020, 2021 },
            CompanyIDs = new List<int> { 1, 2 }
        };

        var mockResponse = new List<TotalProfitsResponseModel>
        {
            new TotalProfitsResponseModel { CompanyID = 1, Year = 2020, TotalOrderPrice=10, TotalShippingCost=10, TotalPrice=10 },
            new TotalProfitsResponseModel { CompanyID = 2, Year = 2021, TotalOrderPrice=10, TotalShippingCost=10, TotalPrice=10 }
        };

        _mockHandler
            .Setup(h => h.GetTotalProfits(request))
            .ReturnsAsync(mockResponse);

        // Act
        var response = await _query.GetTotalProfits(request);

        // Assert
        _mockHandler.Verify(h => h.GetTotalProfits(request), Times.Once);
        Assert.AreEqual(mockResponse, response);
    }
   }
}
