using System;
using System.Collections.Generic;

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
        public Planet root;
        public Single anum;
        public Double r_greenhouse;
        public Double radians_per_rotation; // TODO: Const?
        public Boolean spin_resonance;
        public Random random;

        /// <summary>
        /// Initializes a new Solar System with a seed
        /// </summary>
        /// <param name="seed">The seed.</param>
        public SolarSystem(Int32 seed)
        {
            Seed = seed;
        }

        /// <summary>
        /// Generates the solar system
        /// </summary>
        public void Generate()
        {
            random = new Random(Seed);
            radians_per_rotation = 2.0 * Math.PI;
            stellar_mass_ratio = random.Range(0.6, 1.3);
            stellar_luminosity_ratio = Enviro.Luminosity(stellar_mass_ratio);
            //planet = distribute_planetary_masses(stellar_mass_ratio, stellar_luminosity_ratio, 0.0, stellar_dust_limit(stellar_mass_ratio));
            main_seq_life = 1.0E10 * (stellar_mass_ratio / stellar_luminosity_ratio);
        }
    }
}