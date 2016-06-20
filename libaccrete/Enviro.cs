using System;

namespace Accrete
{
    public class Enviro
    {
        /// <summary>
        /// Calculates the luminosity for a given mass ratio
        /// </summary>
        public static Double Luminosity(Double mass_ratio)
        {
            Double n;

            if (mass_ratio < 1.0)
                n = 1.75*(mass_ratio - 0.1) + 3.325;
            else
                n = 0.5*(2.0 - mass_ratio) + 4.4;
            return Math.Pow(mass_ratio, n);
        }

        /// <summary>
        /// This function, given the orbital radius of a planet in AU, returns the orbital 'zone' of the particle. 
        /// </summary>
        public static Int32 OrbitalZone(SolarSystem system, Double orbital_radius)
        {
            if (orbital_radius < (4.0*Math.Sqrt(system.stellar_luminosity_ratio)))
                return (1);
            if ((orbital_radius >= (4.0*Math.Sqrt(system.stellar_luminosity_ratio))) && (orbital_radius < (15.0*Math.Sqrt(system.stellar_luminosity_ratio))))
                return (2);
            return (3);
        }

        /// <summary>
        /// The mass is in units of solar masses, and the density is in units of grams/cc.  The radius returned is in units of km.
        /// </summary>
        public static Double VolumeRadius(Double mass, Double density)
        {
            mass = mass*Constants.SOLAR_MASS_IN_GRAMS;
            Double volume = mass/density;
            return (Math.Pow((3.0*volume)/(4.0*Math.PI), (1.0/3.0))/Constants.CM_PER_KM);
        }

        /// <summary>
        /// Returns the radius of the planet in kilometers.
        /// The mass passed in is in units of solar masses, the orbital radius
        /// in A.U.    
        /// This formula is listed as eq.9 in Fogg's article, although some typos
        /// crop up in that eq.  See "The Internal Constitution of Planets", by
        /// Dr. D. S. Kothari, Mon. Not. of the Royal Astronomical Society, vol 96
        /// pp.833-843, 1936 for the derivation.  Specifically, this is Kothari's
        /// eq.23, which appears on page 840. 
        /// </summary>
        public static Double KothariRadius(Double mass, Double orbital_radius, Boolean giant, Int32 zone)
        {
            Double atomic_weight, atomic_num;

            if (zone == 1)
            {
                if (giant)
                {
                    atomic_weight = 9.5;
                    atomic_num = 4.5;
                }
                else
                {
                    atomic_weight = 15.0;
                    atomic_num = 8.0;
                }
            }
            else if (zone == 2)
            {
                if (giant)
                {
                    atomic_weight = 2.47;
                    atomic_num = 2.0;
                }
                else
                {
                    atomic_weight = 10.0;
                    atomic_num = 5.0;
                }
            }
            else
            {
                if (giant)
                {
                    atomic_weight = 7.0;
                    atomic_num = 4.0;
                }
                else
                {
                    atomic_weight = 10.0;
                    atomic_num = 5.0;
                }
            }
            Double temp = atomic_weight*atomic_num;
            temp = (2.0*Constants.BETA_20*Math.Pow(Constants.SOLAR_MASS_IN_GRAMS, (1.0/3.0)))/(Constants.A1_20*Math.Pow(temp, (1.0/3.0)));
            Double temp2 = Constants.A2_20*Math.Pow(atomic_weight, (4.0/3.0))*Math.Pow(Constants.SOLAR_MASS_IN_GRAMS, (2.0/3.0));
            temp2 = temp2*Math.Pow(mass, (2.0/3.0));
            temp2 = temp2/(Constants.A1_20*Math.Pow(atomic_num, 2.0));
            temp2 = 1.0 + temp2;
            temp = temp/temp2;
            temp = (temp*Math.Pow(mass, (1.0/3.0)))/Constants.CM_PER_KM;
            return (temp);
        }

        /// <summary>
        /// The mass passed in is in units of solar masses, and the orbital radius
        /// is in units of AU.  The density is returned in units of grams/cc.
        /// </summary>
        public static Double EmpiricalDensity(SolarSystem system, Double mass, Double orbital_radius, Boolean gas_giant)
        {
            Double temp = Math.Pow(mass*Constants.EARTH_MASSES_PER_SOLAR_MASS, (1.0/8.0));
            temp = temp*Math.Pow(system.r_ecosphere/orbital_radius, (1.0/4.0));
            if (gas_giant)
                return (temp*1.2);
            else
                return (temp*5.5);
        }

        /// <summary>
        /// The mass passed in is in units of solar masses, and the equatorial
        /// radius is in km.  The density is returned in units of grams/cc.
        /// </summary>
        public static Double VolumeDensity(Double mass, Double equatorial_radius)
        {
            mass = mass*Constants.SOLAR_MASS_IN_GRAMS;
            equatorial_radius = equatorial_radius*Constants.CM_PER_KM;
            Double volume = (4.0*Math.PI*Math.Pow(equatorial_radius, 3.0))/3.0;
            return (mass/volume);
        }

        /// <summary>
        /// The separation is in units of AU, and both masses are in units of solar
        /// masses.  The period returned is in terms of Earth days.
        /// </summary>
        public static Double Period(Double separation, Double small_mass, Double large_mass)
        {
            Double period_in_years = Math.Sqrt(Math.Pow(separation, 3.0)/(small_mass + large_mass));
            return (period_in_years*Constants.DAYS_IN_A_YEAR);
        }

        /// <summary>
        /// Fogg's information for this routine came from Dole "Habitable Planets
        /// for Man", Blaisdell Publishing Company, NY, 1964.  From this, he came
        /// up with his eq.12, which is the equation for the base_angular_velocity
        /// below.  Going a bit further, he found an equation for the change in
        /// angular velocity per time (dw/dt) from P. Goldreich and S. Soter's paper
        /// "Q in the Solar System" in Icarus, vol 5, pp.375-389 (1966).  Comparing
        /// to the change in angular velocity for the Earth, we can come up with an
        /// approximation for our new planet (his eq.13) and take that into account.
        /// </summary>
        public static Double DayLength(ref SolarSystem system, Double mass, Double radius, Double orbital_period, Double eccentricity, Boolean giant)
        {
            system.spin_resonance = false;
            Double k2 = giant ? 0.24 : 0.33;
            Double planetary_mass_in_grams = mass*Constants.SOLAR_MASS_IN_GRAMS;
            Double equatorial_radius_in_cm = radius*Constants.CM_PER_KM;
            Double base_angular_velocity = Math.Sqrt(2.0*Constants.J*(planetary_mass_in_grams)/(k2*Math.Pow(equatorial_radius_in_cm, 2.0)));
            /*   This next term describes how much a planet's rotation is slowed by    */
            /*  it's moons.  Find out what dw/dt is after figuring out Goldreich and   */
            /*  Soter's Q'.                                                            */
            Double change_in_angular_velocity = 0.0;
            Double temp = base_angular_velocity + (change_in_angular_velocity*system.age);
            /*   'temp' is now the angular velocity. Now we change from rad/sec to     */
            /*  hours/rotation.                               */
            temp = 1.0/((temp/SolarSystem.radians_per_rotation)*Constants.SECONDS_PER_HOUR);
            if (!(temp >= orbital_period)) return (temp);
            Double spin_resonance_period = ((1.0 - eccentricity)/(1.0 + eccentricity))*orbital_period;
            system.Callback("...maybe: " + spin_resonance_period + "\n");
            if (eccentricity > 0.01)
            {
                system.Callback("...resonance...\n");
                temp = spin_resonance_period;
                system.spin_resonance = true;
            }
            else
                temp = orbital_period;
            return (temp);
        }

        /// <summary>
        /// The orbital radius is expected in units of Astronomical Units (AU).
        /// Inclination is returned in units of degrees.
        /// </summary>
        public static Int32 Inclination(SolarSystem system, Double orbital_radius)
        {
            Int32 temp = (int) (Math.Pow(orbital_radius, 0.2)*system.random.About(Constants.EARTH_AXIAL_TILT, 0.4));
            return (temp%360);
        }

        /// <summary>
        /// This function implements the escape velocity calculation.  Note that
        /// it appears that Fogg's eq.15 is incorrect. 
        /// The mass is in units of solar mass, the radius in kilometers, and the
        /// velocity returned is in cm/sec.
        /// </summary>
        public static Double EscapeVel(Double mass, Double radius)
        {
            Double mass_in_grams = mass*Constants.SOLAR_MASS_IN_GRAMS;
            Double radius_in_cm = radius*Constants.CM_PER_KM;
            return (Math.Sqrt(2.0*Constants.GRAV_CONSTANT*mass_in_grams/radius_in_cm));
        }

        /// <summary>
        /// This is Fogg's eq.16.  The molecular weight (usually assumed to be N2) 
        /// is used as the basis of the Root Mean Square velocity of the molecule
        /// or atom.  The velocity returned is in cm/sec. 
        /// </summary>
        public static Double RmsVel(Double molecular_weight, Double orbital_radius)
        {
            Double exospheric_temp = Constants.EARTH_EXOSPHERE_TEMP/Math.Pow(orbital_radius, 2.0);
            return (Math.Sqrt((3.0*Constants.MOLAR_GAS_CONST*exospheric_temp)/molecular_weight)*Constants.CM_PER_METER);
        }
        
        /// <summary> 
        /// This function returns the smallest molecular weight retained by the
        /// body, which is useful for determining the atmosphere composition.   
        /// Orbital radius is in A.U.(ie: in units of the earth's orbital radius),
        /// mass is in units of solar masses, and equatorial radius is in units of
        /// kilometers.                                                         
        /// </summary>
        public static Double MoleculeLimit(double orbital_radius, double mass, double equatorial_radius)
        {
            Double escape_velocity = EscapeVel(mass, equatorial_radius);
            return ((3.0* Math.Pow(Constants.GAS_RETENTION_THRESHOLD * Constants.CM_PER_METER, 2.0) * Constants.MOLAR_GAS_CONST * Constants.EARTH_EXOSPHERE_TEMP) / Math.Pow(escape_velocity, 2.0));
        }
        
        /// <summary> 
        /// This function calculates the surface acceleration of a planet.  The
        /// mass is in units of solar masses, the radius in terms of km, and the
        /// acceleration is returned in units of cm/sec2.                       
        /// </summary>
        public static Double Acceleration(Double mass, Double radius)
        {
            return (Constants.GRAV_CONSTANT * (mass * Constants.SOLAR_MASS_IN_GRAMS) / Math.Pow(radius * Constants.CM_PER_KM, 2.0));
        }
        
        /// <summary> 
        /// This function calculates the surface gravity of a planet.  The     
        /// acceleration is in units of cm/sec2, and the gravity is returned in 
        /// units of Earth gravities.                                           
        /// </summary>
        public static Double Gravity(Double acceleration)
        {
            return (acceleration / Constants.EARTH_ACCELERATION);
        }
        
        /// <summary> 
        /// Note that if the orbital radius of the planet is greater than or equal  */
        /// to R_inner, 99% of it's volatiles are assumed to have been deposited in */
        /// surface reservoirs (otherwise, it suffers from the greenhouse effect).  */
        /// </summary>
        public static Boolean Greenhouse(Int32 zone, Double orbital_radius, Double greenhouse_radius)
        {
            return (orbital_radius < greenhouse_radius) && (zone == 1);
        }
        
        /// <summary> 
        ///  This implements Fogg's eq.17.  The 'inventory' returned is unitless.
        /// </summary>
        public static Double VolInventory(SolarSystem system, Double mass, Double escape_vel, Double rms_vel, Double stellar_mass, Int32 zone, Boolean greenhouse_effect)
        {
            Double proportion_const;
            Double velocity_ratio = escape_vel/rms_vel;
            if (!(velocity_ratio >= Constants.GAS_RETENTION_THRESHOLD)) return (0.0);
            switch (zone)
            {
                case 1:
                    proportion_const = 100000.0;
                    break;
                case 2:
                    proportion_const = 75000.0;
                    break;
                case 3:
                    proportion_const = 250.0;
                    break;
                default:
                    proportion_const = 10.0;
                    system.Callback("Error: orbital zone not initialized correctly!\n");
                    break;
            }
            Double mass_in_earth_units = mass * Constants.EARTH_MASSES_PER_SOLAR_MASS;
            Double temp1 = (proportion_const*mass_in_earth_units)/stellar_mass;
            Double temp2 = system.random.About(temp1, 0.2);
            if (greenhouse_effect)
                return (temp2);
            return (temp2/100.0);
        }


        /// <summary> 
        ///  This implements Fogg's eq.18.  The pressure returned is in units of 
        /// millibars (mb).  The gravity is in units of Earth gravities, the radius */
        /// in units of kilometers.                                             
        /// </summary>
        public static Double pressure(Double volatile_gas_inventory, Double equatorial_radius, Double gravity)
        {
            equatorial_radius = Constants.EARTH_RADIUS_IN_KM / equatorial_radius;
            return (volatile_gas_inventory*gravity/Math.Pow(equatorial_radius, 2.0));
        }

        /// <summary> 
        /// This function returns the boiling point of water in an atmosphere of   */
        /// pressure 'surface_pressure', given in millibars.  The boiling point is */
        /// returned in units of Kelvin.  This is Fogg's eq.21.                
        /// </summary>
        public static Double BoilingPoint(Double surface_pressure)
        {
            Double surface_pressure_in_bars = surface_pressure/ Constants.MILLIBARS_PER_BAR;
            return (1.0/(Math.Log(surface_pressure_in_bars)/-5050.5 + 1.0/373.0));
        }

        /// <summary>
        /// This function is Fogg's eq.22.  Given the volatile gas inventory and
        /// planetary radius of a planet (in Km), this function returns the
        /// fraction of the planet covered with water.
        /// I have changed the function very slightly:  the fraction of Earth's
        /// surface covered by water is 71%, not 75% as Fogg used.
        /// </summary>
        public static Double HydrosphereFraction(Double volatile_gas_inventory, Double planetary_radius)
        {
            Double temp = (0.71*volatile_gas_inventory/1000.0)*Math.Pow(Constants.EARTH_RADIUS_IN_KM/planetary_radius, 2.0);
            return temp >= 1.0 ? (1.0) : (temp);
        }


        /// <summary>
        /// Given the surface temperature of a planet (in Kelvin), this function
        /// returns the fraction of cloud cover available.  This is Fogg's eq.23.
        /// See Hart in "Icarus" (vol 33, pp23 - 39, 1978) for an explanation.
        /// This equation is Hart's eq.3.
        /// I have modified it slightly using constants and relationships from
        /// Glass's book "Introduction to Planetary Geology", p.46.
        /// The 'CLOUD_COVERAGE_FACTOR' is the amount of surface area on Earth
        /// covered by one Kg. of cloud.
        /// </summary>
        public static Double CloudFraction(Double surface_temp, Double smallest_MW_retained, Double equatorial_radius, Double hydrosphere_fraction)
        {
            if (smallest_MW_retained > Constants.WATER_VAPOR)
                return (0.0);
            Double surface_area = 4.0*Math.PI*Math.Pow(equatorial_radius, 2.0);
            Double hydrosphere_mass = hydrosphere_fraction*surface_area*Constants.EARTH_WATER_MASS_PER_AREA;
            Double water_vapor_in_kg = (0.00000001*hydrosphere_mass)*Math.Exp(Constants.Q2_36*(surface_temp - 288.0));
            Double fraction = Constants.CLOUD_COVERAGE_FACTOR*water_vapor_in_kg/surface_area;
            return fraction >= 1.0 ? (1.0) : (fraction);
        }

        /// <summary>
        /// Given the surface temperature of a planet (in Kelvin), this function
        /// returns the fraction of the planet's surface covered by ice.  This is
        /// Fogg's eq.24.  See Hart[24] in Icarus vol.33, p.28 for an explanation.
        /// I have changed a constant from 70 to 90 in order to bring it more in
        /// line with the fraction of the Earth's surface covered with ice, which
        /// is approximatly .016 (=1.6%). 
        /// </summary>
        public static Double IceFraction(Double hydrosphere_fraction, Double surface_temp)
        {
            if (surface_temp > 328.0)
                surface_temp = 328.0;
            Double temp = Math.Pow(((328.0 - surface_temp)/90.0), 5.0);
            if (temp > (1.5*hydrosphere_fraction))
                temp = (1.5*hydrosphere_fraction);
            return temp >= 1.0 ? (1.0) : (temp);
        }

        /// <summary>
        /// This is Fogg's eq.19.  The ecosphere radius is given in AU, the orbital
        /// radius in AU, and the temperature returned is in Kelvin.
        /// </summary>
        public static Double EffTemp(Double ecosphere_radius, Double orbital_radius, Double albedo)
        {
            return (Math.Sqrt(ecosphere_radius/orbital_radius)*Math.Pow((1.0 - albedo)/0.7, 0.25)*Constants.EARTH_EFFECTIVE_TEMP);
        }

        /// <summary>
        /// This is Fogg's eq.20, and is also Hart's eq.20 in his "Evolution of 
        /// Earth's Atmosphere" article.  The effective temperature given is in
        /// units of Kelvin, as is the rise in temperature produced by the
        /// greenhouse effect, which is returned. 
        /// </summary>
        public static Double GreenRise(Double optical_depth, Double effective_temp, Double surface_pressure)
        {
            Double convection_factor = Constants.EARTH_CONVECTION_FACTOR*Math.Pow((surface_pressure/Constants.EARTH_SURF_PRES_IN_MILLIBARS), 0.25);
            return (Math.Pow((1.0 + 0.75*optical_depth), 0.25) - 1.0)*effective_temp*convection_factor;
        }

        /// <summary>
        /// The surface temperature passed in is in units of Kelvin.
        /// The cloud adjustment is the fraction of cloud cover obscuring each
        /// of the three major components of albedo that lie below the clouds.
        /// </summary>
        public static Double PlanetAlbedo(SolarSystem system, Double water_fraction, Double cloud_fraction, Double ice_fraction, Double surface_pressure)
        {
            Double rock_contribution, ice_contribution;

            Double rock_fraction = 1.0 - water_fraction - ice_fraction;
            Double components = 0.0;
            if (water_fraction > 0.0)
                components = components + 1.0;
            if (ice_fraction > 0.0)
                components = components + 1.0;
            if (rock_fraction > 0.0)
                components = components + 1.0;
            Double cloud_adjustment = cloud_fraction/components;
            if (rock_fraction >= cloud_adjustment)
                rock_fraction = rock_fraction - cloud_adjustment;
            else
                rock_fraction = 0.0;
            if (water_fraction > cloud_adjustment)
                water_fraction = water_fraction - cloud_adjustment;
            else
                water_fraction = 0.0;
            if (ice_fraction > cloud_adjustment)
                ice_fraction = ice_fraction - cloud_adjustment;
            else
                ice_fraction = 0.0;
            Double cloud_contribution = cloud_fraction*system.random.About(Constants.CLOUD_ALBEDO, 0.2);
            if (surface_pressure == 0.0)
                rock_contribution = rock_fraction*system.random.About(Constants.AIRLESS_ROCKY_ALBEDO, 0.3);
            else
                rock_contribution = rock_fraction*system.random.About(Constants.ROCKY_ALBEDO, 0.1);
            Double water_contribution = water_fraction*system.random.About(Constants.WATER_ALBEDO, 0.2);
            if (surface_pressure == 0.0)
                ice_contribution = ice_fraction*system.random.About(Constants.AIRLESS_ICE_ALBEDO, 0.4);
            else
                ice_contribution = ice_fraction*system.random.About(Constants.ICE_ALBEDO, 0.1);
            return (cloud_contribution + rock_contribution + water_contribution + ice_contribution);
        }

        /// <summary>
        /// This function returns the dimensionless quantity of optical depth,
        /// which is useful in determining the amount of greenhouse effect on a
        /// planet.
        /// </summary>
        public static Double Opacity(Double molecular_weight, Double surface_pressure)
        {
            Double optical_depth = 0.0;
            if ((molecular_weight >= 0.0) && (molecular_weight < 10.0))
                optical_depth = optical_depth + 3.0;
            if ((molecular_weight >= 10.0) && (molecular_weight < 20.0))
                optical_depth = optical_depth + 2.34;
            if ((molecular_weight >= 20.0) && (molecular_weight < 30.0))
                optical_depth = optical_depth + 1.0;
            if ((molecular_weight >= 30.0) && (molecular_weight < 45.0))
                optical_depth = optical_depth + 0.15;
            if ((molecular_weight >= 45.0) && (molecular_weight < 100.0))
                optical_depth = optical_depth + 0.05;
            if (surface_pressure >= (70.0*Constants.EARTH_SURF_PRES_IN_MILLIBARS))
                optical_depth = optical_depth*8.333;
            else if (surface_pressure >= (50.0*Constants.EARTH_SURF_PRES_IN_MILLIBARS))
                optical_depth = optical_depth*6.666;
            else if (surface_pressure >= (30.0*Constants.EARTH_SURF_PRES_IN_MILLIBARS))
                optical_depth = optical_depth*3.333;
            else if (surface_pressure >= (10.0*Constants.EARTH_SURF_PRES_IN_MILLIBARS))
                optical_depth = optical_depth*2.0;
            else if (surface_pressure >= (5.0*Constants.EARTH_SURF_PRES_IN_MILLIBARS))
                optical_depth = optical_depth*1.5;
            return (optical_depth);
        }

        /// <summary>
        /// The temperature calculated is in degrees Kelvin.
        /// Quantities already known which are used in these calculations:
        ///   planet->molecule_weight
        ///   planet->surface_pressure
        ///   R_ecosphere
        ///   planet->a
        ///   planet->volatile_gas_inventory
        ///   planet->radius
        ///   planet->boil_point    
        /// </summary>
        public static void IterateSurfaceTemp(SolarSystem system, ref Planet planet)
        {
            Double albedo = 0.0, water = 0.0, clouds = 0.0, ice = 0.0;

            Double optical_depth = Opacity(planet.molecule_weight, planet.surface_pressure);
            Double effective_temp = EffTemp(system.r_ecosphere, planet.a, Constants.EARTH_ALBEDO);
            Double greenhouse_rise = GreenRise(optical_depth, effective_temp, planet.surface_pressure);
            Double surface_temp = effective_temp + greenhouse_rise;
            Double previous_temp = surface_temp - 5.0;
            while ((Math.Abs(surface_temp - previous_temp) > 1.0))
            {
                previous_temp = surface_temp;
                water = HydrosphereFraction(planet.volatile_gas_inventory, planet.radius);
                clouds = CloudFraction(surface_temp, planet.molecule_weight, planet.radius, water);
                ice = IceFraction(water, surface_temp);
                if ((surface_temp >= planet.boil_point) || (surface_temp <= Constants.FREEZING_POINT_OF_WATER))
                    water = 0.0;
                albedo = PlanetAlbedo(system, water, clouds, ice, planet.surface_pressure);
                optical_depth = Opacity(planet.molecule_weight, planet.surface_pressure);
                effective_temp = EffTemp(system.r_ecosphere, planet.a, albedo);
                greenhouse_rise = GreenRise(optical_depth, effective_temp, planet.surface_pressure);
                surface_temp = effective_temp + greenhouse_rise;
            }
            planet.hydrosphere = water;
            planet.cloud_cover = clouds;
            planet.ice_cover = ice;
            planet.albedo = albedo;
            planet.surface_temp = surface_temp;
        }

    }
}
 