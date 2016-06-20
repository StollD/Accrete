using System;
using System.Collections.Generic;

namespace Accrete
{
    /// <summary>
    /// A class that stores the definition for a solar system
    /// </summary>
    public class SolarSystem
    {
        // These are all of the global variables used during accretion
        public const Double radians_per_rotation = 2.0 * Math.PI;
        public Int32 Seed;
        public Double stellar_mass_ratio;
        public Double stellar_luminosity_ratio;
        public Double main_seq_life;
        public Double age;
        public Double r_ecosphere;
        public Planet planet_head;
        public Single anum;
        public Double r_greenhouse;
        public Boolean spin_resonance;
        public Random random;
        public Boolean verbose = false;
        public Action<String> Callback = s => { }; 

        // Now for some variables internal to the accretion process
        internal Boolean dust_left;
        internal Double r_inner;
        internal Double r_outer;
        internal Double reduced_mass;
        internal Double dust_density;
        internal Double cloud_eccentricity;
        internal Dust dust_head;

        /// <summary>
        /// Generates the solar system
        /// </summary>
        public static SolarSystem Generate(Int32 Seed)
        {
            return Generate(Seed, Int32.MaxValue);
        }

        /// <summary>
        /// Generates the solar system
        /// </summary>
        public static SolarSystem Generate(Int32 Seed, Int32 Count)
        {
            SolarSystem system = new SolarSystem
            {
                random = new Random(Seed)
            };
            system.stellar_mass_ratio = system.random.Range(0.6, 1.3);
            system.stellar_luminosity_ratio = Enviro.Luminosity(system.stellar_mass_ratio);
            Planet planet = Accretation.DistributePlanetaryMasses(ref system, system.stellar_mass_ratio, system.stellar_luminosity_ratio, 0.0, Accretation.StellarDustLimit(system, system.stellar_mass_ratio));
            system.main_seq_life = 1.0E10 * (system.stellar_mass_ratio / system.stellar_luminosity_ratio);
            system.age = system.random.Range(1.0E9, (system.main_seq_life >= 6.0E9) ? 6.0E9 : system.main_seq_life);
            system.r_ecosphere = Math.Sqrt(system.stellar_luminosity_ratio);
            system.r_greenhouse = system.r_ecosphere * Constants.GREENHOUSE_EFFECT_CONST;
            Int32 i = 0;
            while (planet != null && i < Count)
            {
                planet.orbit_zone = Enviro.OrbitalZone(system, planet.a);
                if (planet.gas_giant)
                {
                    planet.density = Enviro.EmpiricalDensity(system, planet.mass, planet.a, planet.gas_giant);
                    planet.radius = Enviro.VolumeRadius(planet.mass, planet.density);
                }
                else
                {
                    planet.radius = Enviro.KothariRadius(planet.mass, planet.a, planet.gas_giant, planet.orbit_zone);
                    planet.density = Enviro.VolumeRadius(planet.mass, planet.radius);
                }
                planet.orbital_period = Enviro.Period(planet.a, planet.mass, system.stellar_mass_ratio);
                planet.day = Enviro.DayLength(ref system, planet.mass, planet.radius, planet.orbital_period, planet.e, planet.gas_giant);
                planet.resonant_period = system.spin_resonance;
                planet.axial_tilt = Enviro.Inclination(system, planet.a);
                planet.escape_velocity = Enviro.EscapeVel(planet.mass, planet.radius);
                planet.surface_accel = Enviro.Acceleration(planet.mass, planet.radius);
                planet.rms_velocity = Enviro.RmsVel(Constants.MOLECULAR_NITROGEN, planet.a);
                planet.molecule_weight = Enviro.MoleculeLimit(planet.a, planet.mass, planet.radius);
                if ((planet.gas_giant))
                {
                    planet.surface_grav = Constants.INCREDIBLY_LARGE_NUMBER;
                    planet.greenhouse_effect = false;
                    planet.volatile_gas_inventory = Constants.INCREDIBLY_LARGE_NUMBER;
                    planet.surface_pressure = Constants.INCREDIBLY_LARGE_NUMBER;
                    planet.boil_point = Constants.INCREDIBLY_LARGE_NUMBER;
                    planet.hydrosphere = Constants.INCREDIBLY_LARGE_NUMBER;
                    planet.albedo = system.random.About(Constants.GAS_GIANT_ALBEDO, 0.1);
                    planet.surface_temp = Constants.INCREDIBLY_LARGE_NUMBER;
                }
                else
                {
                    planet.surface_grav = Enviro.Gravity(planet.surface_accel);
                    planet.greenhouse_effect = Enviro.Greenhouse(planet.orbit_zone, planet.a, system.r_greenhouse);
                    planet.volatile_gas_inventory = Enviro.VolInventory(system, planet.mass, planet.escape_velocity, planet.rms_velocity, system.stellar_mass_ratio, planet.orbit_zone, planet.greenhouse_effect);
                    planet.surface_pressure = Enviro.pressure(planet.volatile_gas_inventory, planet.radius, planet.surface_grav);
                    planet.boil_point = (planet.surface_pressure == 0.0) ? 0.0 : Enviro.BoilingPoint(planet.surface_pressure);
                    Enviro.IterateSurfaceTemp(system, ref planet);
                }
                planet = planet.next_planet;
                i++;
            }
            return system;
        }
    }
}