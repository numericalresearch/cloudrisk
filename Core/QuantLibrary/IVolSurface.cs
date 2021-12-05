using NodaTime;

namespace QuantLibrary
{
    public interface IVolSurface
    {
        public double Vol(LocalDate date, double strike);
    }

    class FlatVolSurface : IVolSurface
    {
        private double vol;

        public FlatVolSurface(double flatVol)
        {
            vol = flatVol;
        }
        
        public double Vol(LocalDate date, double strike)
        {
            return vol;
        }
    }
}