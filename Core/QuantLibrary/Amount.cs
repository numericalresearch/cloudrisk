namespace QuantLibrary
{

    public struct Amount
    {
        public double Value;
        public Units Units { get; set; }
            
            
        public static bool IsValidCurrency(string ccy)
        {
            // TODO this works for now
            return ccy == "EUR" || ccy == "GBP" || ccy == "USD" || ccy == "DKK";
        }
            
        public Amount(double value, Unit unit)
        {
            Value = value;
            Units = new Units(unit);
        }
        public Amount(double value, Units units)
        {
            Value = value;
            Units = units;
        }
            
        private static void CheckCompatibleCurrencies(Amount lhs, Amount rhs)
        {
            if (lhs.Units != rhs.Units)
                throw new IncompatibleCurrencyException($"Currencies '{lhs.Units}' and '{rhs.Units}' are not compatible for arithmetic");   
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
            return Value.GetHashCode() ^ Units.GetHashCode();
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
            return new Amount(lhs * rhs.Value, rhs.Units);
        }

        public static Amount operator * (Amount lhs, double rhs)
        {
            return new Amount(lhs.Value * rhs, lhs.Units);
        }

        public static Amount operator +(Amount lhs, Amount rhs)
        {
            CheckCompatibleCurrencies(lhs, rhs);
            return new Amount(lhs.Value + rhs.Value, lhs.Units);
        }
    }
}
