using System;


// TODO - do I need all these constructors, and should I seal the classes?
// https://stackoverflow.com/questions/12051/calling-the-base-constructor-in-c-sharp
namespace QuantLibrary
{
    public class QuantLibraryException : Exception
    {
        public QuantLibraryException()
        {
        }

        public QuantLibraryException(string message)
            : base(message)
        {
        }
        
        public QuantLibraryException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    
    public class InvalidCurrencyExecption : QuantLibraryException
    {
        public InvalidCurrencyExecption()
        {
        }

        public InvalidCurrencyExecption(string message)
            : base(message)
        {
        }
        
        public InvalidCurrencyExecption(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class IncompatibleCurrencyException : QuantLibraryException
    {
        public IncompatibleCurrencyException()
        {
        }

        public IncompatibleCurrencyException(string message)
            : base(message)
        {
        }
        
        public IncompatibleCurrencyException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class MarketDataException : QuantLibraryException
    {
        public MarketDataException()
        {
        }

        public MarketDataException(string message)
            : base(message)
        {
        }
        
        public MarketDataException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    
    public class MissingMarketDataException : MarketDataException
    {
        public MissingMarketDataException()
        {
        }

        public MissingMarketDataException(string message)
            : base(message)
        {
        }
        
        public MissingMarketDataException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

}