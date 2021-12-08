using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantLibrary
{
    public struct Unit
    {
        public static readonly Unit _DKK = new Unit("DKK");
        public static readonly Unit _EUR = new Unit("EUR");
        public static readonly Unit _GBP = new Unit("GBP");
        public static readonly Unit _USD = new Unit("USD");
        
        public static readonly Unit _Shares = new Unit("Shares");
        public static readonly Unit _One  = new Unit(1);
        public static readonly Unit _None  = new Unit("NONE");

        private readonly object _value;

        public Unit(object value)
        {
            _value = value;
        }

        public bool IsCurrency()
        {
            return this != _Shares && this != _One;
        }

        public static Units operator * (Unit a, Unit b)
        {
            var res = new Units(new[]{a, b}, new Unit[]{});
            res.Simplify();
            return res;
        }

        public static Units operator / (Unit a, Unit b)
        {
            var res = new Units(new[]{a}, new[]{b});
            res.Simplify();
            return res;
        }
        
        public static bool operator == (Unit a, Unit b)
        {
            return a._value.Equals(b._value);
        }

        public static bool operator != (Unit a, Unit b)
        {
            return !(a == b);
        }

        public override string? ToString()
        {
            return _value.ToString();
        }
    }

    public struct Units
    {
        public static readonly Units DKK = new Units(Unit._DKK);
        public static readonly Units EUR = new Units(Unit._EUR);
        public static readonly Units GBP = new Units(Unit._GBP);                
        public static readonly Units USD = new Units(Unit._USD);
        
        public static readonly Units Shares = new Units(Unit._Shares);
        public static readonly Units One = new Units(Unit._One);

        public static readonly Units None = new Units(Unit._None);

        // private readonly Dictionary<Unit, int> _numerator; 
        // private readonly Dictionary<Unit, int> _denominator;
        public readonly Dictionary<Unit, int> _numerator; 
        public readonly Dictionary<Unit, int> _denominator;

        public Units(Unit unit)
        : this(new []{unit}, new Unit[]{})
        {
        }
        public Units(Unit[] numerator, Unit[] denominator)
        {
            _numerator = new Dictionary<Unit, int>();
            _denominator = new Dictionary<Unit, int>();
            
            foreach (var u in numerator)
                Incr(_numerator, u);
            foreach (var u in denominator)
                Incr(_denominator, u);
        }

        public Units(Dictionary<Unit, int>  numerator, Dictionary<Unit, int> denominator)
        {
            _numerator = new Dictionary<Unit, int>();
            _denominator = new Dictionary<Unit, int>();

            foreach (var u in numerator)
                Incr(_numerator, u.Key, u.Value);
            foreach (var u in denominator)
                Incr(_denominator, u.Key, u.Value);
        }

        public Units(Dictionary<Unit, int>  numerator)
        {
            _numerator = new Dictionary<Unit, int>();
            _denominator = new Dictionary<Unit, int>();

            foreach (var u in numerator)
                Incr(_numerator, u.Key, u.Value);
        }

        public Units NumeratorUnits()
        {
            return new Units(_numerator);
        }
        
        public Units DenominatorUnits()
        {
            return new Units(_denominator);
        }

        private static void Incr(Dictionary<Unit, int> d, Unit u, int value=1)
        { 
            d[u] = d.GetValueOrDefault(u, 0) + value;
        }

        private static void Decr(Dictionary<Unit, int> d, Unit u, int value = 1)
        {
            var current = d[u];
            if (current < value)
                throw new QuantLibraryException("internal error in Units");

            if (current == value)
                d.Remove(u);
            else
                d[u] = current - value;
        }
        
        public void Simplify()
        {
            // If we have the same unit in the numerator and denominator, cancel out opposing units
            foreach(var u in _numerator.Keys)
            {
                if (_denominator.ContainsKey(u))
                {
                    var reduceBy = Math.Min(_numerator[u], _denominator[u]);
                    Decr(_numerator, u, reduceBy);
                    Decr(_denominator, u, reduceBy);
                }
            }

            if (_numerator.ContainsKey(Unit._One))
            {
                // If we have higher orders of 1 in the numerator, reduce them to 1: 1 * 1 * .. == 1
                if (_numerator[Unit._One] > 1)
                {
                    _numerator[Unit._One] = 1;
                }
                // if we have a numerator in the form 1 * x, we can safely drop the 1
                else if (_numerator.Count > 1)
                {
                    _numerator.Remove(Unit._One);
                }
            }

            // denominator Ones get dropped; x / 1 == x
            if (_denominator.ContainsKey(Unit._One))
                _denominator.Remove(Unit._One);

            // We can have empty denominators (an implicit 1), but not empty numerators (something has to be divided)
            if (_numerator.Count == 0)
                _numerator[Unit._One] = 1;
        }

        public bool IsSimpleCurrency()
        {
            if (!IsTrivial(out var unit))
                return false;
            if (!unit.HasValue)
                return false;
            return unit.Value.IsCurrency();
        }

        public bool IsInvertedCurrency()
        {
            if (_numerator.Count != 1 && _numerator.First().Key != Unit._One)
                return false;
            if (_denominator.Count != 1)
                return false;
            return _denominator.First().Key.IsCurrency();
        }

        public bool IsTrivial() => IsTrivial(out var _);
        public bool IsTrivial(out Unit? unit)
        {
            if ( _denominator.Count == 0 
                && _numerator.Count == 1
                && _numerator.First().Value == 1)
            {
                unit = _numerator.Keys.First();
                return true;
            }
            unit = null;
            return false;
        }

        public static Amount operator * (Decimal value, Units units)
        {
            return new Amount(value, units);
        }
        public static Amount operator * (Units units, Decimal value)
        {
            return new Amount(value, units);
        }
        
        public static Units operator * (Unit a, Units b)
        {
            var res = new Units(b._numerator, b._denominator);
            Incr(res._numerator, a);
            res.Simplify();
            return res;
        }
        
        public static Units operator * (Units a, Unit b)
        {
            var res = new Units(a._numerator, a._denominator);
            Incr(res._numerator, b);
            res.Simplify();
            return res;
        }

        public static Units operator * (Units a, Units b)
        {
            var res = new Units(a._numerator, a._denominator);
            foreach (var u in b._numerator)
                Incr(res._numerator, u.Key, u.Value);
            foreach (var u in b._denominator)
                Incr(res._denominator, u.Key, u.Value);
            res.Simplify();
            return res;
        }

        public static Units operator / (Units a, Unit b)
        {
            var res = new Units(a._numerator, a._denominator);
            Incr(res._denominator, b);
            res.Simplify();
            return res;
        }
        
        public static Units operator / (Unit a, Units b)
        {
            var res = new Units(new[] { a }, new Unit[] { });
            foreach (var u in b._numerator)
                Incr(res._denominator, u.Key, u.Value);
            foreach (var u in b._denominator)
                Incr(res._numerator, u.Key, u.Value);
            res.Simplify();
            return res;
        }
        
        public static Units operator / (Units a, Units b)
        {
            var res = new Units(a._numerator, a._denominator);
            foreach (var u in b._numerator)
                Incr(res._denominator, u.Key, u.Value);
            foreach (var u in b._denominator)
                Incr(res._numerator, u.Key, u.Value);
            res.Simplify();
            return res;
        }
        
        public static bool operator == (Unit a, Units b)
        {
            return b == a;
        }
        
        public static bool operator == (Units a, Unit b)
        {
            if (a.IsTrivial(out var a_unit) && a_unit != null)
            {
                return a_unit.Value == b;
            }

            return false;
        }
        
        private static bool dict_equal(Dictionary<Unit, int> a, Dictionary<Unit, int> b)
        {
            if (a.Count != b.Count)
                return false;
            foreach (var key in a.Keys)
            {
                if (!b.TryGetValue(key, out var b_value))
                    return false;
                if (a[key] != b_value)
                    return false;
            }

            return true;
        }
        
        public static bool operator == (Units a, Units b)
        {
            return dict_equal(a._numerator, b._numerator) && dict_equal(a._denominator, b._denominator);
        }

        public static bool operator != (Unit a, Units b)
        {
            return !(a == b);
        }
        public static bool operator != (Units a, Unit b)
        {
            return !(a == b);
        }
        public static bool operator != (Units a, Units b)
        {
            return !(a == b);
        }
    
        public override bool Equals(object? obj)
        {
            if (!(obj is Units))
                return false;

            Units other = (Units)obj;
            return this == other;
        }

        public override int GetHashCode()
        {
            //return _numerator.GetHashCode() ^ _denominator.GetHashCode();
            return RepeatableDictHash(_numerator) ^ RepeatableDictHash(_denominator);
        }

        private int RepeatableDictHash(Dictionary<Unit, int> dict)
        {
            var accum = 0;
            foreach (var kv in dict)
            {
                accum += kv.GetHashCode();
            }

            return accum;
        }
        
        public override string ToString()
        {
            var s = new StringBuilder();
            if (_numerator.Values.Sum() > 1)
            {
                s.Append("[");
                foreach (var u in _numerator.Keys)
                    for (int i = 0; i < _numerator[u]; ++i)
                        s.Append($"{u} ");
                s.Append("]");
            }
            else
            {
                s.Append(_numerator.First().Key.ToString());
            }

            if (_denominator.Values.Sum() == 1)
            {
                s.Append($" / {_denominator.First().Key.ToString()}");
            }
            else if (_denominator.Values.Sum() >= 1)
            {
                s.Append(" / [");
                foreach (var u in _denominator.Keys)
                    for (int i = 0; i < _denominator[u]; ++i)
                        s.Append($"{u} ");
                s.Append("]");
            }

            return s.ToString();
        }
    }
}