using System;

namespace QuantLibrary
{
    public struct Amount
    {
        public readonly Decimal Value;
        public readonly Units Units;
        
        public Amount(Decimal value, Units units)
        {
            Value = value;
            Units = units;
        }
            
        private static void CheckAdditionCompatibleCurrencies(Amount lhs, Amount rhs)
        {
            if (lhs.Units == rhs.Units)
                return;
            if (lhs.Units == Units.None || lhs.Units == Units.One / QuantLibrary.Units.None)
                return;
            
            throw new IncompatibleCurrencyException($"Currencies '{lhs.Units}' and '{rhs.Units}' are not compatible for addition");   
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
            return  lhs.Value == rhs.Value && lhs.Units == rhs.Units ;   
        }
            
        public static bool operator != (Amount lhs, Amount rhs)
        {
            return !(lhs == rhs);   
        }

        public static bool operator < (Amount lhs, Amount rhs)
        {
            CheckAdditionCompatibleCurrencies(lhs, rhs);
            return lhs.Value < rhs.Value;   
        }
        public static bool operator > (Amount lhs, Amount rhs)
        {   
            CheckAdditionCompatibleCurrencies(lhs, rhs);
            return lhs.Value > rhs.Value;   
        }

        // Note: For addition and subtraction, we allow a special "Units.None" unit on the left hand side
        // to make building accumulators easier
        public static Amount operator + (Amount lhs, Amount rhs)
        {
            CheckAdditionCompatibleCurrencies(lhs, rhs);
            return new Amount(lhs.Value + rhs.Value, rhs.Units);
        }

        public static Amount operator - (Amount lhs, Amount rhs)
        {
            CheckAdditionCompatibleCurrencies(lhs, rhs);
            return new Amount(lhs.Value + rhs.Value, rhs.Units);
        }
        
        public static Amount operator * (Decimal lhs, Amount rhs)
        {
            return new Amount(lhs * rhs.Value, rhs.Units);
        }

        public static Amount operator * (Amount lhs, Decimal rhs)
        {
            return new Amount(lhs.Value * rhs, lhs.Units);
        }

        public static Amount operator * (double lhs, Amount rhs)
        {
            return new Amount((decimal)lhs * rhs.Value, rhs.Units);
        }
        public static Amount operator * (Amount lhs, double rhs)
        {
            return new Amount(lhs.Value * (decimal)rhs, lhs.Units);
        }

        public static Amount operator * (Amount lhs, Units rhs)
        {
            return new Amount(lhs.Value, lhs.Units * rhs);
        }

        public static Amount operator * (Amount lhs, Amount rhs)
        {
            return new Amount(lhs.Value * rhs.Value, lhs.Units * rhs.Units);
        }

        public static Amount operator / (decimal lhs, Amount rhs)
        {
            return new Amount(lhs/ rhs.Value, Units.One / rhs.Units);
        }

        public static Amount operator / (Amount lhs, Amount rhs)
        {
            return new Amount(lhs.Value / rhs.Value, lhs.Units / rhs.Units);
        }

        public static Amount operator / (Amount lhs, Units rhs)
        {
            return new Amount(lhs.Value, lhs.Units / rhs);
        }

        public override string ToString()
        {
            return $"{Value} {Units}";
        }
    }
}
