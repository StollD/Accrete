using System;

namespace Accrete
{
    /// <summary>
    /// A class that stores the definition for a solar system
    /// </summary>
    public class SolarSystem
    {
        public readonly Int32 Seed;
        public Double stellar_mass_ratio;
        public Double stellar_luminosity_ratio;
        public Double main_seq_life;
        public Double age;
        public Double r_ecosphere;
        private Random random;

        /// <summary>
        /// Initializes a new Solar System with a seed
        /// </summary>
        /// <param name="seed">The seed.</param>
        public SolarSystem(Int32 seed)
        {
            Seed = seed;
            random = new Random(seed);
        }
    }
}