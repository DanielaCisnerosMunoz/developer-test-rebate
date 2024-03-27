using System;
using Xunit;
using Moq;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System.Collections.Generic;
using static Smartwyre.DeveloperTest.Data.ProductDataStore;

namespace Smartwyre.DeveloperTest.Tests;

public class PaymentServiceTests
{
    //Any valid request
    //Return success
    [Fact]
    public void RebateService_Calculate_ValidRequest()
    {
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "rebate_001",
            ProductIdentifier = "product_001",
            Volume = 10 
        };

        var expectedRebate = new Rebate { Amount = 100, Incentive = IncentiveType.FixedCashAmount };
        var expectedProduct = new Product { SupportedIncentives = new List<IncentiveType> { IncentiveType.FixedCashAmount } };

        var mockRebateDataStore = new Mock<IRebateDataStore>();
        mockRebateDataStore.Setup(x => x.GetRebate(request.RebateIdentifier)).Returns(expectedRebate);

        var mockProductDataStore = new Mock<IProductDataStore>();
        mockProductDataStore.Setup(x => x.GetProduct(request.ProductIdentifier)).Returns(expectedProduct);

        var rebateService = new RebateService(mockRebateDataStore.Object, mockProductDataStore.Object);

        var result = rebateService.Calculate(request);

        Assert.True(result.Success);
    }

    //Null rebate 
    //Return failure
    [Fact]
    public void RebateService_Calculate_NullRebate()
    {
        // Arrange
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "rebate_nonexistent",
            ProductIdentifier = "product_nonexistent",
            Volume = 10
        };

        var expectedProduct = new Product { SupportedIncentives = new List<IncentiveType> { IncentiveType.FixedCashAmount } };

        var mockRebateDataStore = new Mock<IRebateDataStore>();
        mockRebateDataStore.Setup(x => x.GetRebate(request.RebateIdentifier)).Returns((Rebate)null);

        var mockProductDataStore = new Mock<IProductDataStore>();
        mockProductDataStore.Setup(x => x.GetProduct(request.ProductIdentifier)).Returns(expectedProduct);

        var rebateService = new RebateService(mockRebateDataStore.Object, mockProductDataStore.Object);

        var result = rebateService.Calculate(request);

        Assert.False(result.Success);
    }



}

