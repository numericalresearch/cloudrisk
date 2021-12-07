//////////////////////////////////////////////////////////////////////////////
// Qerfc
//      Approximation for erfc based on an implementation in 
//      "A Numerical Library in C for Scientists and Engineers", H.T. Lau,
//      CRC Press, 1995, ISBN 0-8493-7376-X
//      With some adapations and simplifications
//      
// Performance : 
//      comparable with Sun's standard erfc
//
// Accuracy    :
//      fits Sun's erfc function with a maximum relative error of ca. 2.6E-07
//      / absolut error of approx 2.5E-15 in the interval [-20,20]. Good 
//      asymptotic behaviour, too.
//
// Arguments : 
//      like erfc
//
// Miscelaneous : 
//      A QCumNorm based on this function consistently approximates an erfc-based
//      QCumNorm better than the approximation in "Numerical Recipes".
//
//////////////////////////////////////////////////////////////////////////////
using System;

namespace QuantLibrary
{
    public class Maths
    {
        private static double nonexperfc( double x)
        {
            double absx = Math.Abs(x);
            if (absx <= 0.5)
            {
                double erfcx = erfc(x);
                return Math.Exp(x*x)*erfcx;
            } 
            else
            {
                double c = absx;
                double p=((((((-0.136864857382717e-6*c+0.564195517478974e0)*c+
                                    0.721175825088309e1)*c+0.431622272220567e2)*c+
                                  0.152989285046940e3)*c+0.339320816734344e3)*c+
                                0.451918953711873e3)*c+0.300459261020162e3;
                double q= ((((((c+0.127827273196294e2)*c+0.770001529352295e2)*c+
                                   0.277585444743988e3)*c+0.638980264465631e3)*c+
                                 0.931354094850610e3)*c+0.790950925327898e3)*c+
                               0.300459260956983e3;
                return ((x > 0.0) ? p/q : Math.Exp(x*x)*2.0-p/q);
            }
        }

        static double erfc(double x)
        {
            double erfc;
            if (x < -5.5)
            {
                erfc = 2.0;
            }
            else
            {
                double absx = Math.Abs(x);
                if (absx <= 0.5)
                {
                    double c = x * x;
                    double p=((-0.356098437018154e-1*c+0.699638348861914e1)*c+
                              0.219792616182942e2)*c+0.242667955230532e3;
                    double q=((c+0.150827976304078e2)*c+0.911649054045149e2)*c+
                             0.215058875869861e3;
                    double erf = x*p/q;
                    erfc = 1.0-(erf);
                }
                else
                {
                    erfc = Math.Exp(-x*x) * nonexperfc(absx);
                    if (x < 0.0)
                    {
                        erfc = 2.0-(erfc);
                    }
                }
            }
            return erfc;
        }

         
        // Cumulative normal
        public static double N(double x)
        {
            double root2 = Math.Sqrt(2);
            if(x >= 0.0)
            {
                return 1.0 - erfc(x / root2) * 0.5;
            }
            else
            {
                return erfc(-x / root2) * 0.5;
            }
        }
        
        const double invroot2pi = // (1.0 / sqrt(2.0 * pi))
            0.3989422804014326779399460599343818684758566882199982663918071175769;
        // standard normal probability density function
        public static double normalpdf (double x)
        {
            return invroot2pi * Math.Exp(-0.5 * x * x);
        }
        
    }
}