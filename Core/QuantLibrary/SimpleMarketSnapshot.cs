using System;
using System.Collections.Generic;
using NodaTime;

namespace QuantLibrary
{
    public class SimpleMarketSnapshot : IMarketSnapshot
    {
        private readonly Dictionary<Tuple<string, string>, double> _fxRates = new();
        private readonly Dictionary<MarketKey, Amount> _simple = new (); 
        private readonly Dictionary<MarketKey, object> _structured = new (); 

        public LocalDate PricingDate { get; set; }

        public void SetPrice(MarketKey key, Amount value)
        {
            _simple[key] = value;
        }

        public Amount GetPrice(MarketKey key)
        {
            if (GetPrice(key, out var value))
                return value;
            throw new MissingMarketDataException($"No value found for {key.ToString()}");
        }

        public bool GetPrice(MarketKey key, out Amount value)
        {
            return _simple.TryGetValue(key, out value);
        }

        public void SetItem<T>(MarketKey key, T item) where T : class 
        {
            _structured[key] = item;
        }
        
        public bool GetItem<T>(MarketKey key, out T?  item) where T : class
        {
            if (!_structured.TryGetValue(key, out var obj))
            {
                item = null;
                return false;
            }

            item = obj as T;
            if (item == null)
                throw new MarketDataException($"Item for {key.ToString()} is not compatible with requested type");
            
            return true;
        }

        public void SetFXRate(string fromCcy, string toCcy, double rate)
        {
            if (rate == 0.0)
                throw new MarketDataException("FX rate must not be 0!");

            _fxRates[new Tuple<string, string>(fromCcy, toCcy)] = rate;
        }
        public double GetFXRate(string fromCcy, string toCcy)
        {
            if (fromCcy == toCcy)
                return 1.0;
            
            double rate;
            if (_fxRates.TryGetValue(new Tuple<string, string>(fromCcy, toCcy), out rate))
                return rate;
            else if (_fxRates.TryGetValue(new Tuple<string, string>(toCcy, fromCcy), out rate))
            {
                return 1 / rate;
            }
            throw new MarketDataException($"No FX rate found for {fromCcy}/{toCcy} is not compatible with requested type");
        }
        
        public Amount Convert(Amount amount, string toCurrency)
        {
            var rate = GetFXRate(amount.Currency, toCurrency);
            return new Amount(amount.Value * rate, toCurrency);
        }

    }
}