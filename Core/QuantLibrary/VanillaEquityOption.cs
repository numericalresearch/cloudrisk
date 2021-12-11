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

        // Calculate risks; translate to $ risks
        public CalcResults CalculateRisk(IMarketSnapshot marketSnapshot, RiskParameters riskParameters)
        {
            if (marketSnapshot.PricingDate > Expiry)
                throw new QuantLibraryException("Expired instrument");
                    
            // collect market data                    
            var spot = marketSnapshot.GetPrice(MarketKey.StockPrice(Underlying));
            
            if (!marketSnapshot.GetItem(MarketKey.DiscountCurve(Underlying.Currency), out IDiscountCurve? curve) || curve == null)
                throw new MissingMarketDataException($"No discount curve found for {Underlying.Currency}");
            var r = curve.r(Expiry);

            if (!marketSnapshot.GetItem(MarketKey.VolSurface(Underlying), out IVolSurface? vols) || vols == null)
                throw new MissingMarketDataException($"No discount curve found for {Underlying.Currency}");
            var sigma = vols.Vol(Expiry, Strike);
            
            var q = 0.0;    // Let's ignore dividends for now for simplicity 
            
            var days = Period.Between(marketSnapshot.PricingDate, Expiry, PeriodUnits.Days).Days;
            var t = days / 365.0;

            var res = new CalcResults(Underlying.Currency);
            
            // calculate BS risks
            if (PutCall == PutCall.Call)
                res.BlackScholesGreeks = BlackScholes.Call((double)spot.Value, Strike,  t,  r,  q,  sigma);
            else
                res.BlackScholesGreeks = BlackScholes.Put((double)spot.Value, Strike,  t,  r,  q,  sigma);
            
            // translate to $ risks
            res.DollarGreeks.PV = new Amount((decimal)(res.BlackScholesGreeks.PV), Underlying.Currency);
            res.DollarGreeks.Delta = new Amount((decimal)res.BlackScholesGreeks.Delta, Units.One);
            res.DollarGreeks.Gamma = new Amount((decimal)res.BlackScholesGreeks.Gamma, Units.One / Underlying.Currency);
            res.DollarGreeks.Vega = new Amount((decimal)res.BlackScholesGreeks.Vega, Underlying.Currency);
            res.DollarGreeks.Theta = new Amount((decimal)(res.BlackScholesGreeks.Theta), Underlying.Currency);
            res.DollarGreeks.Rho = new Amount((decimal)res.BlackScholesGreeks.Rho, Underlying.Currency);

            return res;
        }
    }
}