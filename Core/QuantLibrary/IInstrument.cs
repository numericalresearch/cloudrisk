namespace QuantLibrary
{
    public interface IPriceable
    {
        public Amount Value(IMarketEnv marketEnv);
        public CalcResults CalculateRisk(IMarketEnv marketEnv, RiskParameters riskParameters);        
    }
    
    public interface IInstrument : IPriceable
    {
        // What's unique to instruments rather than all priceables
    }
}