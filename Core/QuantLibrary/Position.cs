using System;
using System.Collections.Generic;
using NodaTime;

namespace QuantLibrary
{
    
    public struct RiskMeasure
    {
    }

    public class CalcResults
    {
        public Amount Value;
        public Dictionary<string, RiskMeasure> risk;
    }
    
    public class RiskParameters
    {
    };
    
    public interface IPosition : IPriceable
    {
        public IInstrument Instrument { get; }
        public double Quantity { get; }

        
    }
    
    public class SimplePosition : IPosition
    {
        public IInstrument Instrument { get; }
        public double Quantity { get; }
             
        public SimplePosition(IInstrument instrument, double quantity)
        {
            Instrument = instrument;
            Quantity = quantity;
        }

        public Amount Value(IMarketEnv marketEnv)
        {
            return Quantity * Instrument.Value(marketEnv);
        }

        public CalcResults CalculateRisk(IMarketEnv marketEnv, RiskParameters riskParameters)
        {
            throw new NotImplementedException();
        }
    }
}