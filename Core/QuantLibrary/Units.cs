namespace QuantLibrary
{
    public struct Unit
    {
        public static readonly Unit GBP = new Unit("GBP");
        public static readonly Unit EUR = new Unit("EUR");
        public static readonly Unit Shares = new Unit("Shares");
        public static readonly Unit One  = new Unit(1);

        private readonly object _value;

        public Unit(object value)
        {
            _value = value;
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
        
    }

    public class Units
    {
        private readonly Dictionary<Unit, int> _numerator = new(); 
        private readonly Dictionary<Unit, int> _denominator = new();

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

        public Units(Unit[] numerator, Unit[] denominator)
        {
            foreach (var u in numerator)
                Incr(_numerator, u);
            foreach (var u in denominator)
                Incr(_denominator, u);
        }

        public Units(Dictionary<Unit, int>  numerator, Dictionary<Unit, int>  denominator)
        {
            foreach (var u in numerator)
                Incr(_numerator, u.Key, u.Value);
            foreach (var u in denominator)
                Incr(_denominator, u.Key, u.Value);
        }

        
        public void Simplify()
        {
            foreach(var u in _numerator.Keys)
            {
                if (_denominator.ContainsKey(u))
                {
                    var reduceBy = Math.Min(_numerator[u], _denominator[u]);
                    Decr(_numerator, u, reduceBy);
                    Decr(_denominator, u, reduceBy);
                }
            }

            if (_numerator.ContainsKey(Unit.One) && _numerator.Count > 1)
                _numerator.Remove(Unit.One);
            if (_denominator.ContainsKey(Unit.One))
                _denominator.Remove(Unit.One);

            if (_numerator.Count == 0)
                _numerator[Unit.One] = 1;
        } 
        
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
            return _numerator.GetHashCode() ^ _denominator.GetHashCode();
        }
    }
}