using System;

namespace Accrete
{
    public static class Utils
    {
        /// <summary>
        /// This function returns a random real number between the specified inner and outer bounds.   
        /// </summary>
        public static Double Range(this Random random, Double inner, Double outer)
        {
            Double delta = Math.Abs(outer - inner);
            if (inner < outer)
                return (inner + delta*random.NextDouble());
            return (outer + delta*random.NextDouble());
        }

        /// <summary>
        /// This function returns a value within a certain variation of the exact value given it in 'value'.       
        /// </summary>
        public static Double About(this Random random, Double value, Double variation)
        {
            Double inner = value - variation;
            return (inner + 2.0*variation*random.NextDouble());
        }

        public static Double Eccentricity(this Random random)
        {
            return 1.0 - Math.Pow(random.NextDouble(), Constants.ECCENTRICITY_COEFF);
        }
    }
}
 