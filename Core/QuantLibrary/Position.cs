using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NodaTime;

namespace QuantLibrary
{
    
    public struct RiskMeasure
    {
        public static RiskMeasure operator *(double d, RiskMeasure r)
        {
            return new RiskMeasure();
        }
    }

    public struct BlackScholesGreeks
    {
        public double PV;
        public double Delta;
        public double Gamma;
        public double Vega;
        public double Theta;
        public double Rho;

        public BlackScholesGreeks(double pv, double delta, double gamma, double vega, double theta, double rho)
        {
            PV = pv;
            Delta = delta;
            Gamma = gamma;
            Vega = vega;
            Theta = theta;
            Rho = rho;
        }

        public static DollarGreeks operator * (Amount amount, BlackScholesGreeks greeks)
        {
            return new DollarGreeks (
                greeks.PV * amount,
                greeks.Delta * amount,
                greeks.Gamma * amount,
                greeks.Vega * amount,
                greeks.Theta * amount,
                greeks.Rho * amount
            );
        }
        public static BlackScholesGreeks operator * (double amount, BlackScholesGreeks greeks)
        {
            return new BlackScholesGreeks (
                greeks.PV * amount,
                greeks.Delta * amount,
                greeks.Gamma * amount,
                greeks.Vega * amount,
                greeks.Theta * amount,
                greeks.Rho * amount
            );
        }

        public static BlackScholesGreeks operator * (Decimal amount, BlackScholesGreeks greeks)
        {
            return (double)amount * greeks;
        }
    }

    public struct DollarGreeks
    {
        public Amount PV;
        public Amount Delta;
        public Amount Gamma;
        public Amount Vega;
        public Amount Theta;
        public Amount Rho;

        public DollarGreeks(Amount pv, Amount delta, Amount gamma, Amount vega, Amount theta, Amount rho)
        {
            PV = pv;
            Delta = delta;
            Gamma = gamma;
            Vega = vega;
            Theta = theta;
            Rho = rho;
        }

        public static DollarGreeks operator + (DollarGreeks lhs, DollarGreeks rhs)
        {
            return new DollarGreeks(
                lhs.PV + rhs.PV,
                lhs.Delta + rhs.Delta,
                lhs.Gamma + rhs.Gamma,
                lhs.Vega + rhs.Vega,
                lhs.Theta + rhs.Theta,
                lhs.Rho + rhs.Rho
            );
        }
        public static DollarGreeks operator * (Decimal lhs, DollarGreeks rhs)
        {
            return new DollarGreeks(
                lhs * rhs.PV,
                lhs * rhs.Delta,
                lhs * rhs.Gamma,
                lhs * rhs.Vega,
                lhs * rhs.Theta,
                lhs * rhs.Rho
            );
        }
    }

    public class CalcResults
    {
        public Amount Value => DollarGreeks.PV;
        
        public BlackScholesGreeks BlackScholesGreeks;
        public DollarGreeks DollarGreeks;

        public CalcResults(Units ccy)
        {
            DollarGreeks.PV = new Amount(0m, ccy);
            DollarGreeks.Delta = new Amount(0m, Units.One);
            DollarGreeks.Gamma = new Amount(0m, Units.One/ccy);
            DollarGreeks.Vega = new Amount(0m, ccy);
            DollarGreeks.Theta = new Amount(0m, ccy);
            DollarGreeks.Rho = new Amount(0m, ccy);
        }

        public static CalcResults operator * (Decimal lhs, CalcResults rhs)
        {
            var res = new CalcResults(rhs.DollarGreeks.PV.Units);
            res.BlackScholesGreeks = lhs * rhs.BlackScholesGreeks;
            res.DollarGreeks = lhs * rhs.DollarGreeks;
            return res;
        }

        public static CalcResults operator + (CalcResults lhs, CalcResults rhs)
        {
            var res = new CalcResults(lhs.DollarGreeks.PV.Units);    
            res.DollarGreeks = lhs.DollarGreeks + rhs.DollarGreeks;
            return res;
        }
    }
    
    public class RiskParameters
    {
        public Units DefaultCcy = Units.GBP;
        
        public static readonly RiskParameters Default = new();
    };
    
    public interface IPosition : IPriceable
    {
        public IInstrument Instrument { get; }
        public Decimal Quantity { get; }
    }
    
    public class SimplePosition : IPosition
    {
        public IInstrument Instrument { get; }
        public Decimal Quantity { get; }
             
        public SimplePosition(IInstrument instrument, Decimal quantity)
        {
            Instrument = instrument;
            Quantity = quantity;
        }
        
        public CalcResults CalculateRisk(IMarketSnapshot marketSnapshot, RiskParameters riskParameters)
        {
            return Quantity * Instrument.CalculateRisk(marketSnapshot, riskParameters);
        }
    }
}