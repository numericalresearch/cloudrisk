using System;
using System.Collections.Generic;
using System.Linq;
using NodaTime;

namespace QuantLibrary
{
    public class SimpleMarketSnapshot : IMarketSnapshot
    {
        private readonly Dictionary<Tuple<Units, Units>, decimal> _fxRates = new();
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

        public void SetFXRate(Units fromCcy, Units toCcy, Decimal rate)
        {
            if (rate == 0)
                throw new MarketDataException("FX rate must not be 0!"); 
            if (!fromCcy.IsTrivial())
                throw new MarketDataException("From unit must be trivial"); 
            if (!toCcy.IsTrivial())
                throw new MarketDataException("to unit must be trivial"); 

            _fxRates[new Tuple<Units, Units>(fromCcy, toCcy)] = rate;
        }
        public Amount GetFXRate(Units fromUnits, Units toCcy)
        {
            if (!toCcy.IsSimpleCurrency())
                throw new QuantLibraryException("Can only convert to single currencies!");

            if (fromUnits == toCcy)
                return 1m * Units.One;

            if (fromUnits == Units.One)
                return 1m * toCcy;

            if (fromUnits.IsSimpleCurrency())
            {
                decimal rate;
                if (_fxRates.TryGetValue(new Tuple<Units, Units>(fromUnits, toCcy), out rate))
                    return rate * toCcy / fromUnits;
                else if (_fxRates.TryGetValue(new Tuple<Units, Units>(toCcy, fromUnits), out rate))
                    return (1 / rate) * toCcy / fromUnits;
                
                throw new MarketDataException($"No FX rate found for {fromUnits}/{toCcy}");
            }
            else if (fromUnits.IsInvertedCurrency())
            {
                var rate = GetFXRate(fromUnits.DenominatorUnits(), toCcy);
                return (1m / rate);

            }

            throw new QuantLibraryException($"Error in GetFXRate, can't handle unit combination {fromUnits} to {toCcy}");
        }
        
        public Amount Convert(Amount amount, Units toCurrency)
        {
            var rate = GetFXRate(amount.Units, toCurrency);
            return amount * rate;
        }

    }
}