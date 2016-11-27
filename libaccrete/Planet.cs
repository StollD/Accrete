using System;
using System.Collections.Generic;

namespace Accrete
{
    /// <summary>
    /// The definition for a Planet in the System
    /// </summary>
    public class Planet : IComparable<Planet>
    {
        public Double a;                    /* semi-major axis of the orbit (in AU)*/
        public Double e;                    /* eccentricity of the orbit         */
        public Double mass;                 /* mass (in solar masses)         */
        public Boolean gas_giant;           /* TRUE if the planet is a gas giant */
        public Int32 orbit_zone;            /* the 'zone' of the planet          */
        public Double radius;               /* equatorial radius (in km)         */
        public Double density;              /* density (in g/cc)             */
        public Double orbital_period;       /* length of the local year (days)   */
        public Double day;                  /* length of the local day (hours)   */
        public Boolean resonant_period;     /* TRUE if in resonant rotation   */
        public Int32 axial_tilt;            /* units of degrees             */
        public Double escape_velocity;      /* units of cm/sec             */
        public Double surface_accel;        /* units of cm/sec2             */
        public Double surface_grav;         /* units of Earth gravities         */
        public Double rms_velocity;         /* units of cm/sec             */
        public Double molecule_weight;      /* smallest molecular weight retained*/
        public Double volatile_gas_inventory;
        public Double surface_pressure;     /* units of millibars (mb)         */
        public Boolean greenhouse_effect;   /* runaway greenhouse effect?    */
        public Double boil_point;           /* the boiling point of water (Kelvin)*/
        public Double albedo;               /* albedo of the planet             */
        public Double surface_temp;         /* surface temperature in Kelvin     */
        public Double hydrosphere;          /* fraction of surface covered         */
        public Double cloud_cover;          /* fraction of surface covered         */
        public Double ice_cover;            /* fraction of surface covered         */
        internal SortedSet<Planet> bodies_orbiting = new SortedSet<Planet>();
        public Planet[] BodiesOrbiting;
        public Planet first_moon;
        public Planet next_planet;

        // Index Accessor
        public Planet this[Int32 index]
        {
            get { return BodiesOrbiting[index]; }
        }

        public int CompareTo(Planet p)
        {
            return a - p.a > 0d ? 1 : -1;
        }
    }
}