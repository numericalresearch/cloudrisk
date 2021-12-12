using System;
using NodaTime;

namespace QuantLibrary
{
    
    
    public struct MarketKey
    {
        public string Type;         // Broad category of item being referenced - e.g. stock price, yield curve  
        public string Name;         // suitable unique moniker 
        public string Scenario;     // null by default, will be useful when implementing scenarios later

        public override string ToString()
        {
            if (Scenario != null)
                return Type + ":" + Name;
            else
                return Type + ":" + Name + ":" + Scenario;
        }
        
        public static MarketKey StockPrice(Stock stock)
        {
            return new MarketKey { Type = "Stock", Name = stock.Ticker };
        }

        public static MarketKey DiscountCurve(Units ccy)
        {
            return new MarketKey { Type = "DiscountCurve", Name = ccy.ToString() };
        }

        public static MarketKey VolSurface(Stock stock)
        {
            return new MarketKey { Type = "VolSurface", Name = stock.Ticker };
        }

        public static MarketKey FxRate(Units fromCcy, Units toCcy)
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

        Amount Convert(Amount amount, Units toCurrency);
    }
}