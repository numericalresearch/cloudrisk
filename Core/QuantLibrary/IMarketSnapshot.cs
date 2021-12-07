using System;
using NodaTime;

namespace QuantLibrary
{
    
    
    public struct MarketKey
    {
        public string Type; // 
        public string Name; // structured name
        public string Scenario;     // 

        public override string ToString()
        {
            return Type + ":" + Name; // TODO - is the default good enough? 
        }
        
        public static MarketKey StockPrice(Stock stock)
        {
            return new MarketKey { Type = "Stock", Name = stock.Ticker };
        }

        public static MarketKey DiscountCurve(string ccy)
        {
            return new MarketKey { Type = "DiscountCurve", Name = ccy };
        }

        public static MarketKey VolSurface(Stock stock)
        {
            return new MarketKey { Type = "VolSurface", Name = stock.Ticker };
        }

        public static MarketKey FxRate(string fromCcy, string toCcy)
        {
            return new MarketKey { Type = "FXRate", Name = $"{toCcy}/{fromCcy}" };
        }
        
        
    }

    public interface IMarketSnapshot
    {
        LocalDate PricingDate { get; set; }

        void SetPrice (MarketKey key, Amount value);
        Amount GetPrice(MarketKey key);
        bool GetPrice(MarketKey key, out Amount value);

        void SetItem<T>(MarketKey key, T item) where T : class;
        
        bool GetItem<T>(MarketKey key, out T? item) where T : class;

        Amount Convert(Amount amount, string toCurrency);
    }
}