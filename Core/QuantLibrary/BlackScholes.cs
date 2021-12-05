using System;
using QuantLibrary;

namespace QuantLibrary;

public class BlackScholes
{
    //////////////////////////////////////////////////////////////////////////
    // Function     : calcD1D2
    // Purpose      : calculate the common values d1 and d2
    //////////////////////////////////////////////////////////////////////////
    public static (double, double) calcD1D2 (
        double S,
        double K,
        double T,
        double r,
        double q,
        double sigma)
    {
        // PRECONDITION( S >= 0.0 );
        // PRECONDITION( K >  0.0 );
        // PRECONDITION( T >  0.0 );
        // PRECONDITION( sigma > 0.0 );
        double sigmaSqrtT = sigma * Math.Sqrt(T);
        double d1 = (Math.Log(S/K) + (r - q + 0.5 * sigma * sigma ) * T) / sigmaSqrtT;
        double d2 = d1 - sigmaSqrtT;

        return (d1, d2);
    }

    
    public static double CallPrice(double S, double K, double T, double r, double q, double sigma)
    {
        var (d1, d2) = calcD1D2(S, K, T, r, q, sigma);
        return Maths.N(d1) * S * Math.Exp (-q * T) - Math.Exp(-r * T) * Maths.N (d2) * K;
    }
    public static double PutPrice(double S, double K, double T, double r, double q, double sigma)
    {
        var (d1, d2) = calcD1D2(S, K, T, r, q, sigma);
        return - Maths.N(-d1) * S * Math.Exp (-q * T) + Math.Exp(-r * T) * Maths.N (-d2) * K;
    }

    public static double Price(bool isCall,  double S, double K,  double T, double r, double q, double sigma)
    {
        if (isCall)
            return CallPrice (S, K, T, r, q, sigma);
        else
            return PutPrice (S, K, T, r, q, sigma);
    }
}