using System;
using CommandLine;
using System.IO;

namespace Accrete
{
    /// <summary>
    /// Generates a solar system and saves it to disk
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry method
        /// </summary>
        public static void Main(String[] args)
        {
            // Get the commandline Options
            Options options = new Options();
            Parser.Default.ParseArguments(args, options);

            // Generate a new Solar System
            SolarSystem system = new SolarSystem(options.Verbose, false, Console.Write);
            if (options.Seed != Int32.MinValue)
                SolarSystem.Generate(ref system, options.Seed, options.Count);
            else
                SolarSystem.Generate(ref system, options.Count);

            // Save it to file
            StreamWriter file = new StreamWriter(Directory.GetCurrentDirectory() + "/" + options.File, false);
            file.Write("                         SYSTEM  CHARACTERISTICS\n");
            file.Write("Mass of central star:          {0:F3} solar masses\n", system.stellar_mass_ratio);
            file.Write("Luminosity of central star:    {0:F3} (relative to the sun)\n", system.stellar_luminosity_ratio);
            file.Write("Total main sequence lifetime:  {0:F0} million years\n", (system.main_seq_life / 1.0E6));
            file.Write("Current age of stellar system: {0:F0} million years\n", (system.age / 1.0E6));
            file.Write("Radius of habitable ecosphere: {0:F3} AU\n\n", system.r_ecosphere);

            Planet node1 = system.planet_head;
            Int32 counter = 1;

            while (node1 != null)
            {
                file.Write("Planet #{0}:\n", counter);
                if (node1.gas_giant)
                    file.Write("Gas giant...\n");
                if (node1.resonant_period)
                    file.Write("In resonant period with primary.\n");

                file.Write("   Distance from primary star (in A.U.): {0:F3}\n", node1.a);
                file.Write("   Eccentricity of orbit:                {0:F3}\n", node1.e);
                file.Write("   Mass (in Earth masses):               {0:F3}\n", node1.mass * Constants.EARTH_MASSES_PER_SOLAR_MASS);
                file.Write("   Equatorial radius (in Km):            {0:F1}\n", node1.radius);
                file.Write("   Density (in g/cc):                    {0:F3}\n", node1.density);
                file.Write("   Escape Velocity (in km/sec):          {0:F2}\n", node1.escape_velocity / Constants.CM_PER_KM);
                file.Write("   Smallest molecular weight retained:   {0:F2}", node1.molecule_weight);

                if (node1.molecule_weight < Constants.MOLECULAR_HYDROGEN)
                    file.Write("   (H2)\n");
                else if (node1.molecule_weight < Constants.HELIUM)
                    file.Write("   (He)\n");
                else if (node1.molecule_weight < Constants.METHANE)
                    file.Write("   (CH4)\n");
                else if (node1.molecule_weight < Constants.AMMONIA)
                    file.Write("   (NH3)\n");
                else if (node1.molecule_weight < Constants.WATER_VAPOR)
                    file.Write("   (H2O)\n");
                else if (node1.molecule_weight < Constants.NEON)
                    file.Write("   (Ne)\n");
                else if (node1.molecule_weight < Constants.MOLECULAR_NITROGEN)
                    file.Write("   (N2)\n");
                else if (node1.molecule_weight < Constants.CARBON_MONOXIDE)
                    file.Write("   (CO)\n");
                else if (node1.molecule_weight < Constants.NITRIC_OXIDE)
                    file.Write("   (NO)\n");
                else if (node1.molecule_weight < Constants.MOLECULAR_OXYGEN)
                    file.Write("   (O2)\n");
                else if (node1.molecule_weight < Constants.HYDROGEN_SULPHIDE)
                    file.Write("   (H2S)\n");
                else if (node1.molecule_weight < Constants.ARGON)
                    file.Write("   (Ar)\n");
                else if (node1.molecule_weight < Constants.CARBON_DIOXIDE)
                    file.Write("   (CO2)\n");
                else if (node1.molecule_weight < Constants.NITROUS_OXIDE)
                    file.Write("   (N2O)\n");
                else if (node1.molecule_weight < Constants.NITROGEN_DIOXIDE)
                    file.Write("   (NO2)\n");
                else if (node1.molecule_weight < Constants.OZONE)
                    file.Write("   (O3)\n");
                else if (node1.molecule_weight < Constants.SULPHUR_DIOXIDE)
                    file.Write("   (SO2)\n");
                else if (node1.molecule_weight < Constants.SULPHUR_TRIOXIDE)
                    file.Write("   (SO3)\n");
                else if (node1.molecule_weight < Constants.KRYPTON)
                    file.Write("   (Kr)\n");
                else if (node1.molecule_weight < Constants.XENON)
                    file.Write("   (Xe)\n");
                else
                    file.Write("\n");

                file.Write("   Surface acceleration (in cm/sec2):    {0:F2}\n", node1.surface_accel);
                if (!(node1.gas_giant))
                {
                    file.Write("   Surface Gravity (in Earth gees):      {0:F2}\n", node1.surface_grav);
                    if (node1.boil_point > 0.1)
                        file.Write("   Boiling point of water (celcius):     {0:F1}\n", (node1.boil_point - Constants.KELVIN_CELCIUS_DIFFERENCE));
                    if (node1.surface_pressure > 0.00001)
                    {
                        file.Write("   Surface Pressure (in atmospheres):    {0:F3}", (node1.surface_pressure / 1000.0));
                        file.Write(node1.greenhouse_effect ? "     RUNAWAY GREENHOUSE EFFECT\n" : "\n");
                    }
                    file.Write("   Surface temperature (Celcius):        {0:F2}\n", (node1.surface_temp - Constants.KELVIN_CELCIUS_DIFFERENCE));
                    if (node1.hydrosphere > 0.01)
                        file.Write("   Hydrosphere percentage:               {0:F2}\n", (node1.hydrosphere * 100));
                    if (node1.cloud_cover > 0.01)
                        file.Write("   Cloud cover percentage:               {0:F2}\n", (node1.cloud_cover * 100));
                    if (node1.ice_cover > 0.01)
                        file.Write("   Ice cover percentage:                 {0:F2}\n", (node1.ice_cover * 100));
                }
                file.Write("   Axial tilt (in degrees):              {0}\n", node1.axial_tilt);
                file.Write("   Planetary albedo:                     {0:F3}\n", node1.albedo);
                file.Write("   Length of year (in years):            {0:F2}\n", (node1.orbital_period / 365.25));
                file.Write("   Length of day (in hours):             {0:F2}\n\n", node1.day);
                counter++;
                node1 = node1.next_planet;
            }
            file.Close();
            Console.WriteLine();
            Console.WriteLine("Done! System definition written to \"{0}\"", options.File);
        }
    }

    /// <summary>
    /// The options for the app
    /// </summary>
    public class Options
    {
        [Option('s', "seed", DefaultValue = Int32.MinValue, HelpText = "The seed that should be used for the system generation.")]
        public Int32 Seed { get; set; } = Int32.MinValue;

        [Option('v', "verbose", DefaultValue = false, HelpText = "Prints all messages to standard output.")]
        public Boolean Verbose { get; set; } = false;

        [Option('f', "file", DefaultValue = "New.System", HelpText = "The file that will store the system information.")]
        public String File { get; set; } = "New.System";

        [Option('c', "count", DefaultValue = Int32.MaxValue, HelpText = "The maximal amount of generated bodies.")]
        public Int32 Count { get; set; } = Int32.MaxValue;

    }
}
