using System.Net.Mime;
using System.Reflection.Metadata;
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

        // Calculate risks; txlate to $ risks; get rid of Value()
        public CalcResults CalculateRisk(IMarketSnapshot marketSnapshot, RiskParameters riskParameters)
        {
            if (marketSnapshot.PricingDate > Expiry)
                // TODO
                throw new QuantLibraryException("Expired instrument");
                    
            // collect market data                    
            var spot = marketSnapshot.GetPrice(MarketKey.StockPrice(Underlying));
            
            if (!marketSnapshot.GetItem(MarketKey.DiscountCurve(Underlying.Currency), out IDiscountCurve? curve) || curve == null)
                throw new MissingMarketDataException($"No discount curve found for {Underlying.Currency}");
            var r = curve.r(Expiry);

            if (!marketSnapshot.GetItem(MarketKey.VolSurface(Underlying), out IVolSurface? vols) || vols == null)
                throw new MissingMarketDataException($"No discount curve found for {Underlying.Currency}");
            var sigma = vols.Vol(Expiry, Strike);
            
            // TODO 
            var q = 0.0;    // Let's ignore dividends for now for simplicity 
            
            var days = Period.Between(marketSnapshot.PricingDate, Expiry, PeriodUnits.Days).Days;
            var t = days / 365.0;

            var res = new CalcResults(Underlying.Currency);
            
            // calculate BS risks
            if (PutCall == PutCall.Call)
                res.BlackScholesGreeks = BlackScholes.Call(spot.Value, Strike,  t,  r,  q,  sigma);
            else
                res.BlackScholesGreeks = BlackScholes.Put(spot.Value, Strike,  t,  r,  q,  sigma);
            
            // txlate to $ risks
            // TODO 
            res.DollarGreeks.PV = new Amount(res.BlackScholesGreeks.PV * Strike, Underlying.Currency);
            res.DollarGreeks.Delta = new Amount(res.BlackScholesGreeks.Delta, Underlying.Currency);
            res.DollarGreeks.Gamma = new Amount(res.BlackScholesGreeks.Gamma, Underlying.Currency);
            res.DollarGreeks.Vega = new Amount(res.BlackScholesGreeks.Vega, Underlying.Currency);
            res.DollarGreeks.Theta = new Amount(res.BlackScholesGreeks.Theta * Strike, Underlying.Currency);
            res.DollarGreeks.Rho = new Amount(res.BlackScholesGreeks.Rho, Underlying.Currency);

            return res;
        }
    }
}