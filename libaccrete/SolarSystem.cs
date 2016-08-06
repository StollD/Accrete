using System;
using System.Linq;
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

        // Star values
        public Double stellar_mass_ratio;
        public Double stellar_radius_ratio;
        public Double stellar_luminosity_ratio;
        public Double stellar_temp;
        public Double main_seq_life;
        public Double age;
        public Double r_ecosphere;
        public Double r_greenhouse;
        public StellarType type;

        // Planetary values
        public Planet planet_head;
        public Planet[] Bodies;
        public Single anum;
        public Boolean spin_resonance;

        // Settings and utility
        public Random random;
        public Boolean verbose = false;
        public Action<String> Callback = s => { };
        public Boolean moons;

        // Now for some variables internal to the accretion process
        internal Boolean dust_left;
        internal Double r_inner;
        internal Double r_outer;
        internal Double reduced_mass;
        internal Double dust_density;
        internal Double cloud_eccentricity;
        internal Dust dust_head;
        internal SortedSet<Planet> bodies;

        // Index Accessor
        public Planet this[Int32 index]
        {
            get { return Bodies[index]; }
        }

        public SolarSystem(Boolean verbose, Boolean moons, Action<String> callback)
        {
            bodies = new SortedSet<Planet>();
            this.verbose = verbose;
            this.moons = moons;
            this.Callback = callback;
        }

        /// <summary>
        /// Generates the solar system
        /// </summary>
        public static void Generate(ref SolarSystem system)
        {
            system.random = new Random();
            GenerateInternal(ref system, Int32.MaxValue);
        }

        /// <summary>
        /// Generates the solar system
        /// </summary>
        public static void Generate(ref SolarSystem system, Int32 Count)
        {
            system.random = new Random();
            GenerateInternal(ref system, Count);
        }

        /// <summary>
        /// Generates the solar system
        /// </summary>
        public static void Generate(ref SolarSystem system, Int32 Seed, Int32 Count = Int32.MaxValue)
        {
            system.random = new Random(Seed);
            GenerateInternal(ref system, Count);
        }

        /// <summary>
        /// Generates the solar system
        /// </summary>
        private static void GenerateInternal(ref SolarSystem system, Int32 Count)
        {
            // Describe the star
            system.stellar_mass_ratio = system.random.Range(0.6, 1.3);
            system.stellar_radius_ratio = Math.Floor(system.random.About(Math.Pow(system.stellar_mass_ratio, 1.0 / 3.0), 0.05) * 1000.0) / 1000.0;
            system.stellar_luminosity_ratio = Enviro.Luminosity(system.stellar_mass_ratio);
            system.stellar_temp = Math.Floor(5650 * Math.Sqrt(Math.Sqrt(system.stellar_luminosity_ratio) / system.stellar_radius_ratio));
            system.main_seq_life = 1.0E10 * (system.stellar_mass_ratio / system.stellar_luminosity_ratio);
            if (system.main_seq_life > 6.0E9)
                system.age = system.random.Range(1.0E9, 6.0E9);
            else if (system.main_seq_life > 1.0E9)
                system.age = system.random.Range(1.0E9, system.main_seq_life);
            else
                system.age = system.random.Range(system.main_seq_life / 10, system.main_seq_life);
            system.age = system.random.Range(1.0E9, (system.main_seq_life >= 6.0E9) ? 6.0E9 : system.main_seq_life);
            system.r_ecosphere = Math.Sqrt(system.stellar_luminosity_ratio);
            system.r_greenhouse = system.r_ecosphere * Constants.GREENHOUSE_EFFECT_CONST;
            system.type = StellarType.GetStellarTypeTemp(system.stellar_temp);

            // Create the first Planet
            Planet planet = Accretation.DistributePlanetaryMasses(ref system, system.stellar_mass_ratio, system.stellar_luminosity_ratio, 0.0, Accretation.StellarDustLimit(system, system.stellar_mass_ratio));
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
                    planet.greenhouse_effect = false;
                    planet.hydrosphere = Constants.INCREDIBLY_LARGE_NUMBER;
                    planet.albedo = system.random.About(Constants.GAS_GIANT_ALBEDO, 0.1);
                }
                planet.surface_grav = Enviro.Gravity(planet.surface_accel);
                planet.greenhouse_effect = Enviro.Greenhouse(planet.orbit_zone, planet.a, system.r_greenhouse);
                planet.volatile_gas_inventory = Enviro.VolInventory(system, planet.mass, planet.escape_velocity, planet.rms_velocity, system.stellar_mass_ratio, planet.orbit_zone, planet.greenhouse_effect);
                planet.surface_pressure = Enviro.pressure(planet.volatile_gas_inventory, planet.radius, planet.surface_grav);
                planet.boil_point = (planet.surface_pressure == 0.0) ? 0.0 : Enviro.BoilingPoint(planet.surface_pressure);
                Enviro.IterateSurfaceTemp(system, ref planet);

                // Moons
                if (system.moons)
                {
                    planet.first_moon = Accretation.DistributeMoonMasses(ref system, planet.mass, planet.radius);

                    Planet moon = planet.first_moon;
                    while (moon != null)
                    {
                        moon.radius = Enviro.KothariRadius(moon.mass, 0, false, planet.orbit_zone);
                        moon.density = Enviro.VolumeDensity(moon.mass, moon.radius);
                        moon.density = system.random.Range(1.5, moon.density * 1.1);
                        if (moon.density < 1.5)
                            moon.density = 1.5;
                        moon.radius = Enviro.VolumeRadius(moon.mass, moon.density);
                        moon.orbital_period = Enviro.Period(moon.a, moon.mass, planet.mass);
                        moon.day = Enviro.DayLength(ref system, moon.mass, moon.radius, moon.orbital_period, moon.e, false);
                        moon.resonant_period = system.spin_resonance;
                        moon.axial_tilt = Enviro.Inclination(system, moon.a);
                        moon.escape_velocity = Enviro.EscapeVel(moon.mass, moon.radius);
                        moon.surface_accel = Enviro.Acceleration(moon.mass, moon.radius);
                        moon.rms_velocity = Enviro.RmsVel(Constants.MOLECULAR_NITROGEN, planet.a);
                        moon.molecule_weight = Enviro.MoleculeLimit(moon.a, moon.mass, moon.radius);
                        moon.surface_grav = Enviro.Gravity(moon.surface_accel);
                        moon.greenhouse_effect = Enviro.Greenhouse(planet.orbit_zone, planet.a, system.r_greenhouse);
                        moon.volatile_gas_inventory = Enviro.VolInventory(system, moon.mass, moon.escape_velocity, moon.rms_velocity, system.stellar_mass_ratio, planet.orbit_zone, moon.greenhouse_effect);
                        moon.surface_pressure = Enviro.pressure(moon.volatile_gas_inventory, moon.radius, moon.surface_grav);
                        if (moon.surface_pressure == 0.0)
                            moon.boil_point = 0.0;
                        else
                            moon.boil_point = Enviro.BoilingPoint(moon.surface_pressure);
                        Enviro.IterateSurfaceTemp(system, planet, ref moon);
                        planet.bodies_orbiting.Add(moon);
                        moon = moon.next_planet;
                    }
                    planet.BodiesOrbiting = planet.bodies_orbiting.ToArray();
                }

                if (i + 1 < Count)
                {
                    planet = planet.next_planet;
                    i++;
                }
                else
                {
                    planet.next_planet = null;
                    system.bodies.RemoveWhere(p => p.a > planet.a);
                }
            }
            system.Bodies = system.bodies.ToArray();
        }
    }
}