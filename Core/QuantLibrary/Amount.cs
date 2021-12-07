namespace QuantLibrary
{

    public struct Amount
    {
        public double Value;
        public string Currency { get; set; } // TODO validation check
            
            
        public static bool IsValidCurrency(string ccy)
        {
            // TODO this works for now
            return ccy == "EUR" || ccy == "GBP" || ccy == "USD" || ccy == "DKK";
        }
            
        public Amount(double value, string currency)
        {
            if (!IsValidCurrency(currency))
                throw new InvalidCurrencyExecption($"Invalid currency '{currency}'");
            Value = value;
            Currency = currency;
        }
            
        private static void CheckCompatibleCurrencies(Amount lhs, Amount rhs)
        {
            if (lhs.Currency != rhs.Currency)
                throw new IncompatibleCurrencyException($"Currencies '{lhs.Currency}' and '{rhs.Currency}' are not compatible for arithmetic");
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is Amount))
                return false;

            Amount other = (Amount)obj;
            return this == other;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode() ^ Currency.GetHashCode();
        }
        
        public static bool operator == (Amount lhs, Amount rhs)
        {
            CheckCompatibleCurrencies(lhs, rhs);
            return lhs.Value == rhs.Value;   
        }
            
        public static bool operator != (Amount lhs, Amount rhs)
        {
            CheckCompatibleCurrencies(lhs, rhs);
            return !(lhs == rhs);   
        }

        public static bool operator < (Amount lhs, Amount rhs)
        {
            CheckCompatibleCurrencies(lhs, rhs);
            return lhs.Value < rhs.Value;   
        }
        public static bool operator > (Amount lhs, Amount rhs)
        {   
            CheckCompatibleCurrencies(lhs, rhs);
            return lhs.Value > rhs.Value;   
        }
        
        // TODO
        
        public static Amount operator * (double lhs, Amount rhs)
        {
            return new Amount(lhs * rhs.Value, rhs.Currency);
        }

        public static Amount operator * (Amount lhs, double rhs)
        {
            return new Amount(lhs.Value * rhs, lhs.Currency);
        }

        public static Amount operator +(Amount lhs, Amount rhs)
        {
            CheckCompatibleCurrencies(lhs, rhs);
            return new Amount(lhs.Value + rhs.Value, lhs.Currency);
        }
    }
}
