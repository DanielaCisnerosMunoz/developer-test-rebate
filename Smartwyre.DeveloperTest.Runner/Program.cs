using Smartwyre.DeveloperTest.Data;
using System;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{
    static void Main(string[] args)
    {
        var rebateDataStore = new RebateDataStore();
        var productDataStore = new ProductDataStore();

        var rebateService = new RebateService(rebateDataStore, productDataStore);

        Console.WriteLine("Enter Rebate ID");
        string rebateIdentifier = Console.ReadLine();

        Console.WriteLine("Enter Product ID");
        string productIdentifier = Console.ReadLine();

        Console.WriteLine("Enter Volume:");
        int volume;

        while (!int.TryParse(Console.ReadLine(), out volume))
        {
            Console.WriteLine("Invalid input. Please enter a valid volume:");
        }

        // Create a CalculateRebateRequest object with user input
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = rebateIdentifier,
            ProductIdentifier = productIdentifier,
            Volume = volume
        };

        var result = rebateService.Calculate(request);

        if (result.Success) Console.WriteLine("Success");        
        else Console.WriteLine("Failure");
        
    }
}
