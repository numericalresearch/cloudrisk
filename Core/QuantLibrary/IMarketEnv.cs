using NodaTime;

namespace QuantLibrary
{
    
    
    public struct MarketKey
    {
        public string Type; // 
        public string Name; // structured name
        public Instant? Timestamp;   // Null for wildcards? 
        public string Scenario;     // 

        public override string ToString()
        {
            return Type + ":" + Name; // TODO - is the default good enough? 
        }
    }
    
    
    // Snapshot, not env!? !??!
    public interface IMarketEnv
    {
        LocalDate PricingDate { get; set; }
        // Do I need an instant here? 

        Amount GetPrice(MarketKey key);
        bool GetPrice(MarketKey key, out Amount value);

        bool GetItem<T>(MarketKey key, out T item);
    }
}