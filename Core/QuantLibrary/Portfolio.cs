using System;
using System.Collections.Generic;
using System.Linq;

namespace QuantLibrary
{
    public class Portfolio : IPriceable
    {
        private List<IPriceable> _positions = new();
        
        public Portfolio()
        {}

        public Portfolio(List<IPriceable> positions)
        {
            _positions = positions;
        }

        public void Add(IPriceable position)
        {
            _positions.Add(position);
        }

        public CalcResults CalculateRisk(IMarketSnapshot market, RiskParameters riskParameters)
        {
            return AggregatePositionRisks(CalculatePositionRisks(market, riskParameters), market, riskParameters);
        }

        public List<CalcResults> CalculatePositionRisks(IMarketSnapshot market, RiskParameters riskParameters)
        {
            return _positions.Select(p => p.CalculateRisk(market, riskParameters)).ToList();
        }
        
        public static CalcResults AggregatePositionRisks( List<CalcResults> risks, IMarketSnapshot market, RiskParameters riskParameters)
        {
            var totalRisk = new CalcResults(riskParameters.DefaultCcy);
            
            foreach(var r in risks)
            {
                totalRisk += FxConvert(r, riskParameters.DefaultCcy, market);
            }
            
            return totalRisk;
        }

        private static CalcResults FxConvert(CalcResults cr, Units ccy, IMarketSnapshot market)
        {
            var converted = new CalcResults(ccy);
            converted.BlackScholesGreeks = cr.BlackScholesGreeks;
            converted.DollarGreeks = Convert(cr.DollarGreeks, ccy, market);
            return converted;
        }

        private static DollarGreeks Convert(DollarGreeks greeks, Units ccy, IMarketSnapshot market)
        {
            return new DollarGreeks(
                market.Convert(greeks.PV, ccy),
                market.Convert(greeks.Delta, ccy),
                market.Convert(greeks.Gamma, ccy),
                market.Convert(greeks.Vega, ccy),
                market.Convert(greeks.Theta, ccy),
                market.Convert(greeks.Rho, ccy)
            );
        }
    }
}