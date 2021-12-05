using System.Collections.Generic;
using NodaTime;

namespace QuantLibrary
{
    public class SimpleMarketEnv : IMarketEnv
    {
        private readonly Dictionary<MarketKey, Amount> Simple = new (); 
        private readonly Dictionary<MarketKey, object> Structured = new (); 

        public LocalDate PricingDate { get; set; }

        public void SetPrice(MarketKey key, Amount value)
        {
            Simple[key] = value;
        }

        public Amount GetPrice(MarketKey key)
        {
            if (GetPrice(key, out var value))
                return value;
            throw new MissingMarketDataException($"No value found for {key.ToString()}");
        }

        public bool GetPrice(MarketKey key, out Amount value)
        {
            return Simple.TryGetValue(key, out value);
        }

        
        public bool GetItem<T>(MarketKey key, out T item)
        {
            throw new System.NotImplementedException();
        }
    }
}