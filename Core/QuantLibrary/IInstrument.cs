namespace QuantLibrary
{
    public interface IPriceable
    {
        public CalcResults CalculateRisk(IMarketSnapshot marketSnapshot, RiskParameters riskParameters);        
    }
    
    public interface IInstrument : IPriceable
    {
        // What's unique to instruments rather than all priceables
    }
}