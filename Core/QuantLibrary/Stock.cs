
using System;

namespace QuantLibrary
{
    public class Stock : IInstrument
    {
        public readonly string Ticker;
        public readonly Units Currency;

        public Stock(string ticker, Units ccy)
        {
            Ticker = ticker;
            Currency = ccy;
        }
        
        public CalcResults CalculateRisk(IMarketSnapshot marketSnapshot, RiskParameters riskParameters)
        {
            var priceKey = MarketKey.StockPrice(this);
            if (!marketSnapshot.GetPrice(priceKey, out var price))
                throw new MarketDataException($"No price found for {priceKey}");  
            
            var res = new CalcResults(Currency);
            
            res.BlackScholesGreeks.PV = (double)price.Value;
            res.BlackScholesGreeks.Delta = 1;
            res.BlackScholesGreeks.Gamma = 0;
            res.BlackScholesGreeks.Vega = 0;
            res.BlackScholesGreeks.Theta = 0;
            res.BlackScholesGreeks.Rho = 0;

            res.DollarGreeks.PV = price;
            res.DollarGreeks.Delta = new Amount(1m, Units.One);

            return res;
        }
    }
}