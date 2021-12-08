
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
            var res = new CalcResults(Currency);
            
            res.BlackScholesGreeks.PV = (double)marketSnapshot.GetPrice(MarketKey.StockPrice(this)).Value;
            res.BlackScholesGreeks.Delta = 1;
            res.BlackScholesGreeks.Gamma = 0;
            res.BlackScholesGreeks.Vega = 0;
            res.BlackScholesGreeks.Theta = 0;
            res.BlackScholesGreeks.Rho = 0;

            res.DollarGreeks.PV = marketSnapshot.GetPrice(MarketKey.StockPrice(this));
            res.DollarGreeks.Delta = new Amount(1m, Units.One);

            return res;
        }
    }
}