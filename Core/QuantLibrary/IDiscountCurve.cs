using System;
using System.ComponentModel;
using NodaTime;

namespace QuantLibrary
{
    public interface IDiscountCurve
    {
        public LocalDate Today { get;  }
        public double r(LocalDate to);
        public double r(LocalDate from, LocalDate to);
        public double df(LocalDate to);
        public double df(LocalDate from, LocalDate to);
    }

    public class FlatDiscountCurve : IDiscountCurve
    {
        public LocalDate Today { get; }
        private double Rate; 

        public FlatDiscountCurve(LocalDate today, double r)
        {
            Today = today;
            Rate = r;

        }

        public double r(LocalDate to)
        {
            return Rate;
        }
        
        public double r(LocalDate from, LocalDate to)
        {
            return Rate;
        }
        
        public double df(LocalDate to)
        {
            return df(Today, to);
        }

        public double df(LocalDate from, LocalDate to)
        {
            if (from < Today)
                throw new QuantLibraryException("from date must be great or equal to today");  
                
            if (from > to)
                throw new QuantLibraryException("from date must be before to date");

            if (from > Today)
            {
                return df(to) / df(from);
            }

            var days = Period.Between(from, to, PeriodUnits.Days).Days;
            var t = days / 365.0;
            return Math.Exp(-Rate * t);
        }
    }
}