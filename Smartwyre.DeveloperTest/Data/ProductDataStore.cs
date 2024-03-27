using Smartwyre.DeveloperTest.Types;
using static Smartwyre.DeveloperTest.Data.ProductDataStore;

namespace Smartwyre.DeveloperTest.Data;

public interface IProductDataStore
{
    Product GetProduct(string identifier);
}

public class ProductDataStore: IProductDataStore
{
    public Product GetProduct(string productIdentifier)
    {
        // Access database to retrieve account, code removed for brevity 
        return new Product();
    }
    
}
