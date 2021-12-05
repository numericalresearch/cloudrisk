
namespace QuantLibrary
{
    public class Stock : IInstrument
    {
        public readonly string Ticker;
        public readonly string Currency;

        public Stock(string ticker, string ccy)
        {
            Ticker = ticker;
            Currency = ccy;
        }
        
        public Amount Value(IMarketEnv marketEnv)
        {
            throw new System.NotImplementedException();
        }

        public CalcResults CalculateRisk(IMarketEnv marketEnv, RiskParameters riskParameters)
        {
            throw new System.NotImplementedException();
        }
    }
}