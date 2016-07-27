using System;

namespace Accrete
{
    /// <summary>
    /// Mathematical constanst
    /// </summary>
    public class Constants
    {
        public const Double ECCENTRICITY_COEFF = (0.077);           /* Dole's was 0.077         */
        public const Double PROTOPLANET_MASS = (1.0E-15);           /* Units of solar masses    */
        public const Double SOLAR_MASS_IN_GRAMS = (1.989E33);       /* Units of grams           */
        public const Double EARTH_MASS_IN_GRAMS = (5.977E27);       /* Units of grams           */
        public const Double EARTH_RADIUS = (6.378E6);               /* Units of cm              */
        public const Double EARTH_RADIUS_IN_KM = (6378.0);          /* Units of km              */
        public const Double EARTH_ACCELERATION = (981.0);           /* Units of cm/sec2         */
        public const Double EARTH_AXIAL_TILT = (23.4);              /* Units of degrees         */
        public const Double EARTH_EXOSPHERE_TEMP = (1273.0);        /* Units of degrees Kelvin  */
        public const Double SUN_MASS_IN_EARTH_MASSES = 332775.64;
        public const Double EARTH_MASSES_PER_SOLAR_MASS = (332775.64);
        public const Double EARTH_EFFECTIVE_TEMP = (255.0);         /* Units of degrees Kelvin  */
        public const Double EARTH_ALBEDO = (0.39);
        public const Double CLOUD_COVERAGE_FACTOR = (1.839E-8);     /* Km2/kg                   */
        public const Double EARTH_WATER_MASS_PER_AREA = (3.83E15);  /* grams per square km     */
        public const Double EARTH_SURF_PRES_IN_MILLIBARS = (1000.0);
        public const Double EARTH_CONVECTION_FACTOR = (0.43);       /* from Hart, eq.20         */
        public const Double FREEZING_POINT_OF_WATER = (273.0);      /* Units of degrees Kelvin  */
        public const Double DAYS_IN_A_YEAR = (365.256);             /* Earth days per Earth year*/
        public const Double GAS_RETENTION_THRESHOLD = (5.0);        /* ratio of esc vel to RMS vel */
        public const Double GAS_GIANT_ALBEDO = (0.5);               /* albedo of a gas giant    */
        public const Double CLOUD_ALBEDO = (0.52);
        public const Double AIRLESS_ROCKY_ALBEDO = (0.07);
        public const Double ROCKY_ALBEDO = (0.15);
        public const Double WATER_ALBEDO = (0.04);
        public const Double AIRLESS_ICE_ALBEDO = (0.5);
        public const Double ICE_ALBEDO = (0.7);
        public const Double SECONDS_PER_HOUR = (3600.0);
        public const Double CM_PER_AU = (1.495978707E13);           /* number of cm in an AU    */
        public const Double CM_PER_KM = (1.0E5);                    /* number of cm in a km     */
        public const Double KM_PER_AU = CM_PER_AU / CM_PER_KM;
        public const Double CM_PER_METER = (100.0);
        public const Double MILLIBARS_PER_BAR = (1000.0);
        public const Double KELVIN_CELCIUS_DIFFERENCE = (273.0);
        public const Double GRAV_CONSTANT = (6.672E-8);             /* units of dyne cm2/gram2  */
        public const Double GREENHOUSE_EFFECT_CONST = (0.93);       /* affects inner radius..   */
        public const Double MOLAR_GAS_CONST = (8314.41);            /* units: g*m2/(sec2*K*mol) */
        public const Double K = (50.0);                             /* K = gas/dust ratio       */
        public const Double B = (1.2E-5);                           /* Used in Crit_mass calc   */
        public const Double DUST_DENSITY_COEFF = (2.0E-3);          /* A in Dole's paper        */
        public const Double ALPHA = (5.0);                          /* Used in density calcs    */
        public const Double N = (3.0);                              /* Used in density calcs    */
        public const Double J = (1.46E-19);                         /* Used in day-length calcs (cm2/sec2 g) */
        public const Double INCREDIBLY_LARGE_NUMBER = (9.9999E37);


        /*  Now for a few molecular weights (used for RMS velocity calcs):     */
        /*  This table is from Dole's book "Habitable Planets for Man", p. 38  */
        public const Double ATOMIC_HYDROGEN = (1.0);                /* H   */
        public const Double MOLECULAR_HYDROGEN = (2.0);             /* H2  */
        public const Double HELIUM = (4.0);                         /* He  */
        public const Double ATOMIC_NITROGEN = (14.0);               /* N   */
        public const Double ATOMIC_OXYGEN = (16.0);                 /* O   */
        public const Double METHANE = (16.0);                       /* CH4 */
        public const Double AMMONIA = (17.0);                       /* NH3 */
        public const Double WATER_VAPOR = (18.0);                   /* H2O */
        public const Double NEON = (20.2);                          /* Ne  */
        public const Double MOLECULAR_NITROGEN = (28.0);            /* N2  */
        public const Double CARBON_MONOXIDE = (28.0);               /* CO  */
        public const Double NITRIC_OXIDE = (30.0);                  /* NO  */
        public const Double MOLECULAR_OXYGEN = (32.0);              /* O2  */
        public const Double HYDROGEN_SULPHIDE = (34.1);             /* H2S */
        public const Double ARGON = (39.9);                         /* Ar  */
        public const Double CARBON_DIOXIDE = (44.0);                /* CO2 */
        public const Double NITROUS_OXIDE = (44.0);                 /* N2O */
        public const Double NITROGEN_DIOXIDE = (46.0);              /* NO2 */
        public const Double OZONE = (48.0);                         /* O3  */
        public const Double SULPHUR_DIOXIDE = (64.1);               /* SO2 */
        public const Double SULPHUR_TRIOXIDE = (80.1);              /* SO3 */
        public const Double KRYPTON = (83.8);                       /* Kr  */
        public const Double XENON = (131.3);                        /* Xe  */

        /*  The following defines are used in the kothari_radius function in    */
        /*  file Enviro.cs.                            */
        public const Double A1_20 = (6.485E12);                     /* All units are in cgs system.  */
        public const Double A2_20 = (4.0032E-8);                    /*   ie: cm, g, dynes, etc.      */
        public const Double BETA_20 = (5.71E12);


        /*  The following defines are used in determining the fraction of a planet  */
        /*  covered with clouds in function cloud_fraction in file Enviro.cs.         */
        public const Double Q1_36 = (1.258E19);                     /* grams    */
        public const Double Q2_36 = (0.0698);                       /* 1/Kelvin */
    }
}