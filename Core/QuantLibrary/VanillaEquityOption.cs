using System.Net.Mime;
using System.Text;
using NodaTime;

namespace QuantLibrary
{
    public enum PutCall
    {
        Put,
        Call
    }
    
    
    
    public class VanillaEquityOption : IInstrument 
    {
        public readonly Stock Underlying;
        public readonly double Strike;
        public readonly PutCall PutCall;
        public readonly LocalDate Expiry;

        public VanillaEquityOption(
            Stock underlying,
            double strike,
            PutCall putCall,
            LocalDate expiry
        )
        {
            Underlying = underlying;
            Strike = strike;
            PutCall = putCall;
            Expiry = expiry;
        }

        public Amount Value(IMarketEnv marketEnv)
        {
            /*
            double spot = marketEnv.GetPrice(new MarketKey {Stock.Ticker, "", }) 

            */
    //        if(!marketEnv.GetItem(new MarketKey(Underlying.Currency, ... , "DiscountCurve"), out var discountCurve))
    //           throw new QuantLibraryException("missing curve ");
    
    
            /*
              How do we make this easy to use? Maybe key shouldn't involve a timestamp?
              - in this context, we expect a market environment to be a consistent snapshot, containing the latest 
              values, so maybe rename it???
              
              
              Convenience functions to generate different keys?
              
              var spot = env.GetPrice(PriceKey(Underlying));
              if (!env.GetItem(DiscountCurveKey(Underlying.Currency), out IDiscountCurve curve))
                throw MissingMarketDataException($"No discount   
                
                
                
                
             
              
             */
            
            double spot = 100;
            double r = 0.05; //  marketEnv.Get
            double price;
            /*
            PutCall switch
            {
                case PutCall.Call:
                    price = BlackScholes.CallPrice(DispositionTypeNames, )
                */
            throw new System.NotImplementedException();
        }

        public CalcResults CalculateRisk(IMarketEnv marketEnv, RiskParameters riskParameters)
        {
            throw new System.NotImplementedException();
        }
    }
}