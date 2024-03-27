using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;
using System;
using System.Linq;
using System.Linq.Expressions;
using static Smartwyre.DeveloperTest.Data.ProductDataStore;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService : IRebateService{

    private readonly IRebateDataStore _rebateDataStore;
    private readonly IProductDataStore _productDataStore;

    public RebateService(IRebateDataStore rebateDataStore, IProductDataStore productDataStore)
    {
        _rebateDataStore = rebateDataStore;
        _productDataStore = productDataStore;
    }

    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        try {
            CalculateRebateResult result = new CalculateRebateResult();
            decimal rebateAmount = 0m;

            //Lookup the rebate that the request is being made against.
            Rebate rebate = _rebateDataStore.GetRebate(request?.RebateIdentifier);

            //Lookup the product that the request is being made against.
            Product product = _productDataStore.GetProduct(request?.ProductIdentifier);


            //Check that the rebate and request are valid. Validations are made before any calculation to avoid unnecessary processing.
            //Validation logic is now encapsulated
            bool passedInitialValidation = InitialValidations(request, rebate, product);

            if (!passedInitialValidation)
            {
                result.Success = false;
                return result;
            }

            //Calculate Rebate
            //Calculation logic is now encapsulated
            rebateAmount = CalculateRebateAmount(request, rebate, product);

            //Assumption: From checking the previous formulas, the calculation was always unsuccesful when the resulting amount was 0. So I am assuming that no rebate means the calculation was unsuccesful.
            result.Success = rebateAmount > 0;

            if (result.Success)
            {
                _rebateDataStore.StoreCalculationResult(rebate, rebateAmount);
            }

            return result;

        }
        catch (Exception ex) {

            //Should save in log table if existing
            throw ex;
        }

    }


    private bool InitialValidations(CalculateRebateRequest request, Rebate rebate, Product product)
    {
        //Not all validations have been grouped for better readibility

        //Null checks
        if (request == null || rebate == null || product == null) return false;


        //If no amount has been sold there should be no rebate (assumption)
        if (request.Volume == 0) return false;

        //Check whether the product supports the incentive type
        //No longer using the SupportedIncentiveType enum as it difficults maintenability and 
        if (product.SupportedIncentives ==null || !product.SupportedIncentives.Contains(rebate.Incentive)) return false;

        return true;

    }

    private decimal CalculateRebateAmount(CalculateRebateRequest request, Rebate rebate, Product product)
    {
        //Simplified calculation of each incentive type
        //To add new Incentive type we need to add it to the enum and to this method.

        switch (rebate.Incentive)
        {
            case IncentiveType.FixedCashAmount:
                return rebate.Amount;

            case IncentiveType.FixedRateRebate:
                return product.Price * rebate.Percentage * request.Volume;

            case IncentiveType.AmountPerUom:
                return rebate.Amount * request.Volume;

            default:
                return 0m;
        }
    }

}
