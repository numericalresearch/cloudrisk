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

    public static BlackScholesGreeks Call(double S, double K, double T, double r, double q, double sigma)
    {
        var (d1, d2) = calcD1D2(S, K, T, r, q, sigma);
        var risk = new BlackScholesGreeks();
        risk.PV = callPrice(S, K, T, r, q, sigma, d1, d2);
        risk.Delta = callDelta(S, K, T, r, q, sigma, d1, d2);
        risk.Gamma = callGamma(S, K, T, r, q, sigma, d1, d2);
        risk.Vega = callVega(S, K, T, r, q, sigma, d1, d2);
        risk.Theta = callTheta(S, K, T, r, q, sigma, d1, d2);
        risk.Rho = callRho(S, K, T, r, q, sigma, d1, d2);

        return risk;
    }
   static double callPrice (double S, double K, double T,
                       double r, double q, double sigma,
                       double d1, double d2)
    {
        double c = Maths.N(d1) * S * Math.Exp (-q * T) - Math.Exp(-r * T) * Maths.N (d2) * K;
        return c;
    }

    static double callDelta  (double S, double K, double T,
                       double r, double q, double sigma,
                       double d1, double d2)
    {
        double delta = Math.Exp (-q * T) * Maths.N (d1);
        return delta;
    }

    static double callGamma (double S, double K, double T,
                       double r, double q, double sigma,
                       double d1, double d2)
    {
        double gamma = (Maths.normalpdf(d1) * Math.Exp (-q * T)) / (S * sigma * Math.Sqrt(T));
        return gamma;
    }

    static double callVega  (double S, double K, double T,
                       double r, double q, double sigma,
                       double d1, double d2)
    {
        double vega = S * Math.Sqrt (T) * Maths.normalpdf(d1) * Math.Exp (-q * T);
        return vega;
    }

    static double callRho (double S, double K, double T,
                       double r, double q, double sigma,
                       double d1, double d2)
    {
        double rho = K * T * Math.Exp (-r * T) * Maths.N (d2);
        return rho;
    }

    static double callRho_q (double S, double K, double T,
                       double r, double q, double sigma,
                       double d1, double d2)
    {
        double rho = K * T * Math.Exp (-q * T) * Maths.N (d2);
        return rho;
    }

    static double callTheta (double S, double K, double T,
                       double r, double q, double sigma,
                       double d1, double d2)
    {
        double expqT = Math.Exp (-q * T);
        double term1 = - (S * Maths.normalpdf (d1) * sigma * expqT) / (2 * Math.Sqrt(T));
        double term2 = - r * K * Math.Exp (-r * T) * Maths.N (d2);
        double term3 = q * S * Maths.N (d1) * expqT;

        double theta = term1 + term2 + term3;
        return theta;
     }
    

    public static BlackScholesGreeks Put(double S, double K, double T, double r, double q, double sigma)
    {
        var (d1, d2) = calcD1D2(S, K, T, r, q, sigma);
        var risk = new BlackScholesGreeks();
        risk.PV = putPrice(S, K, T, r, q, sigma, d1, d2);
        risk.Delta = putDelta(S, K, T, r, q, sigma, d1, d2);
        risk.Gamma = putGamma(S, K, T, r, q, sigma, d1, d2);
        risk.Vega = putVega(S, K, T, r, q, sigma, d1, d2);
        risk.Theta = putTheta(S, K, T, r, q, sigma, d1, d2);
        risk.Rho = putRho(S, K, T, r, q, sigma, d1, d2);
        
        return risk;
    }
    
    static double putPrice (double S, double K, double T,
                  double r, double q, double sigma,
                  double d1, double d2)
    {
        double p = - Maths.N(-d1) * S * Math.Exp (-q * T) + Math.Exp(-r * T) * Maths.N (-d2) * K;
        return p;
    }

    static double  putDelta  (double S, double K, double T,
                       double r, double q, double sigma,
                       double d1, double d2)
    {
        double delta = Math.Exp (-q * T) * (Maths.N (d1) - 1);
        return delta;
    }

    static double  putGamma (double S, double K, double T,
                       double r, double q, double sigma,
                       double d1, double d2)
    {
        double gamma = (Maths.normalpdf(d1) * Math.Exp (-q * T)) / (S * sigma * Math.Sqrt(T));
        return gamma;
    }

    static double  putVega  (double S, double K, double T,
                       double r, double q, double sigma,
                       double d1, double d2)
    {
        double vega = S * Math.Sqrt(T) * Maths.normalpdf(d1) * Math.Exp (-q * T);
        return vega;
    }

    static double  putRho (double S, double K, double T,
                       double r, double q, double sigma,
                       double d1, double d2)
    {
        double rho = - K * T * Math.Exp (-r * T) * Maths.N (-d2);
        return rho;
    }

    static double  putRho_q (double S, double K, double T,
                       double r, double q, double sigma,
                       double d1, double d2)
    {
        double rho = - K * T * Math.Exp (-q * T) * Maths.N (-d2);
        return rho;
    }

    static double  putTheta (double S, double K, double T,
                       double r, double q, double sigma,
                       double d1, double d2)
    {
        double expqT = Math.Exp (-q * T);
        double term1 = - (S * Maths.normalpdf(d1) * sigma * expqT) / (2 * Math.Sqrt(T));
        double term2 = r * K * Math.Exp (-r * T) * Maths.N (-d2);
        double term3 = - q * S * Maths.N (-d1) * expqT;

        double theta = term1 + term2 + term3;
        return theta;
     }
}